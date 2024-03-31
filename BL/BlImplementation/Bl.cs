namespace BlImplementation;
using BlApi;
using System;
using System.Runtime.InteropServices;

internal class Bl : IBl
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public IEngineer Engineer => new EngineerImplementation(this);
    public ITask Task => new TaskImplementation(this);
    //  public DateTime? StartProjectDate { get => _dal.StartProjectDate; set => _dal.StartProjectDate = value; }
    //  public DateTime? EndProjectDate { get => EndProjectDate; init => EndProjectDate = value; }
    public void InitializeDB() => DalTest.Initialization.Do();
    public void ResetDB() => DalTest.Initialization.Reset();

    private static DateTime s_Clock = DateTime.Now;
    public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }
    public DateTime? StartProjectDate
    {
        get => _dal.StartProjectDate is null ? Clock.Date : _dal.StartProjectDate;
        set
        {
            if (value <= Clock)
                throw new BO.BlCannotBeUpdatedException("The project can't be started before today's date");
            _dal.StartProjectDate = value;
        }
    }

    public void PromoteTime(BO.Time addTime)
    {
        switch (addTime)
        {
            case BO.Time.Hour:
                Clock = Clock.AddHours(1);
                break;
            case BO.Time.Day:
                Clock = Clock.AddDays(1);
                break;
            case BO.Time.Year:
                Clock = Clock.AddYears(1);
                break;
            default:
                break;
        }
    }

    public void ResetClock()
    {
        Clock = DateTime.Now;
    }

    /// <summary>
    /// Retrieves the status of the project.
    /// </summary>
    /// <returns>The status of the project.</returns>
    public BO.ProjectStatus GetProjectStatus()
    {
        if (StartProjectDate is null)
            return BO.ProjectStatus.Planing;
        if (Task.ReadAll(item => item.ScheduledDate is null).Any())
            return BO.ProjectStatus.Execution;
        return BO.ProjectStatus.MidLevel;
    }

    /// <summary>
    /// Creates a schedule for the project tasks.
    /// </summary>
    private void AutoSchedule()
    {
        // Retrieve tasks without dependencies
        //    var haveNoDependency = Task.ReadAll(boTask => boTask.Dependencies?.Count() == 0).ToList(); //Task.ReadAll(item => /**/item.Dependencies is null);
        var haveNoDependency = Task.ReadAll(item => item.Dependencies is null).ToList();
        // Start date of the project
        DateTime startDate = _dal.StartProjectDate!.Value;
        // Set scheduled date for tasks without dependencies
        foreach (var item in haveNoDependency)
        {
            var task = Task.Read(item.Id);
            task.ScheduledDate = startDate;
            Task.Update(task);
        }
        // Update scheduled dates in dependencies
        foreach (var item in haveNoDependency)
        {
            updateScheduledDateInDep(item.Id);
        }
    }
    /// <summary>
    /// Update scheduled dates in task dependencies recursively
    /// </summary>
    /// <param name="id">id of task that other tasks depend on</param>
    private void updateScheduledDateInDep(int id)
    {
        // Retrieve tasks that depend on the given task id
        var dependencies = Task.Read(id).Dependencies;
        if (dependencies is null)
            return;
        // Retrieve the task object of the given id
        BO.Task dep = Task.Read(id);
        foreach (var item in dependencies)
        {
            // Retrieve the task object for the dependent task
            BO.Task taskToDoStartDate = Task.Read(item.Id);

            // If the scheduled date for the dependent task is null, set it to the forecast date of the task it depends on
            if (taskToDoStartDate.ScheduledDate is null)
                taskToDoStartDate.ScheduledDate = dep.ForecastDate;
            // Otherwise, set it to the maximum of the dependent task's current scheduled date and the forecast date of the task it depends on
            else
                taskToDoStartDate.ScheduledDate = (dep.ScheduledDate > taskToDoStartDate.ScheduledDate) ? dep.ForecastDate : taskToDoStartDate.ScheduledDate;

            // Update the scheduled date for the dependent task in the database
            Task.Update(taskToDoStartDate);

            // Recursively call UpdateScheduledDateInDep for the dependent task to update its dependencies
            updateScheduledDateInDep(taskToDoStartDate.Id);

        }
    }
    private void ManuallySchedule(int taskId)
    {
        BO.Task task = Task.Read(taskId);
        if (task.Dependencies == null)
        {
            task.ScheduledDate = StartProjectDate;
        }
        else
        {
            DateTime? maxForecast = Clock;
            foreach (var d in task.Dependencies!)
            {
                BO.Task readTask = Task.Read(d.Id);
                if (readTask.ForecastDate == null)
                    throw new BO.BlCannotBeUpdatedException("It is not possible to update a task to the previous task no forecast date has been set.");
                if (readTask.ForecastDate > maxForecast)
                    maxForecast = readTask.ForecastDate;
            }
            task.ScheduledDate = maxForecast;
        }
        Task.Update(task);
    }
    public void CreateSchedule(DateTime date, BO.CreateScheduleOption option = BO.CreateScheduleOption.Automatically, int taskId = -1)
    {
        switch (option)
        {
            case BO.CreateScheduleOption.Automatically:
                AutoSchedule();
                break;
            case BO.CreateScheduleOption.Manually:
                ManuallySchedule(taskId);
                break;
        }
    }

}


