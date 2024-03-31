namespace BlImplementation;
using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

internal class TaskImplementation : ITask
{
    private readonly IBl bl;
    internal TaskImplementation(IBl bl) => this.bl = bl;
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private BlApi.IBl _bl = BlApi.Factory.Get();

    //private bool isCircularDependency(int idDependent , int )
    //{

    //}

    /// <summary>
    /// Adds a dependency for a task on another task.
    /// </summary>
    /// <param name="dependencyTask">The ID of the task that depends on another task.</param>
    /// <param name="dependsOnTask">The ID of the task that the dependency is added to.</param>
    /// <exception cref="BO.BlTheProjectTimeDoesNotAllowException">Thrown when trying to add a dependency after the project start date has been set.</exception>
    /// <exception cref="BO.BlDoesNotExistException">Thrown when either of the tasks involved in the dependency does not exist.</exception>
    public void AddDependency(int dependencyTask, int dependsOnTask)
    {
        if (_bl.GetProjectStatus() == BO.ProjectStatus.Execution)
            throw new BO.BlTheProjectTimeDoesNotAllowException("Impossible action at this stage of the project - The project start date has already been set");
        if (_dal.Task.Read(dependencyTask) is null)

            throw new BO.BlDoesNotExistException($"Task with ID={dependencyTask} was not found");
        if (_dal.Task.Read(dependsOnTask) is null)
            throw new BO.BlDoesNotExistException($"Task with ID={dependsOnTask} was not found");
        _dal.Dependency.Create(new DO.Dependency(0, dependencyTask, dependsOnTask));
    }

    /// <summary>
    /// Creates a new task.
    /// </summary>
    /// <param name="boTask">The task object containing details of the task to be created.</param>
    /// <returns>The ID of the newly created task.</returns>
    /// <exception cref="BO.BlTheProjectTimeDoesNotAllowException">Thrown when trying to add a task after the project start date has been set.</exception>
    /// <exception cref="BO.BlInvalidInputFormatException">Thrown when the input data is invalid.</exception>
    /// <exception cref="BO.BlAlreadyExistsException">Thrown when trying to create a task with an ID that already exists.</exception>
    public int Create(BO.Task boTask)
    {
        // Check if the project start date has already been set
        if (_bl.GetProjectStatus() == BO.ProjectStatus.Execution)
            throw new BO.BlTheProjectTimeDoesNotAllowException("Impossible action at this stage of the project - The project start date has already been set");

        // Check for invalid input data
        if (isValidID(boTask.Id) || isValidString(boTask.Alias))
            throw new BO.BlInvalidInputFormatException("Wrong data");

        // Create a new DO.Task object based on the provided BO.Task
        int? engineerId = boTask.Engineer is not null ? boTask.Engineer.Id : null;
        DO.Task doTask = new DO.Task(boTask.Id, engineerId, false, boTask.Alias, boTask.Description, boTask.CreatedAtDate, boTask.ScheduledDate, boTask.StartDate, boTask.RequiredEffortTime, (DO.EngineerExperience)boTask.Complexity, null, boTask.CompleteDate, boTask.Deliverables, boTask.Remarks);

        // Call the DAL to create the task and get the new task's ID
        int idNewTask = _dal.Task.Create(doTask);

        // If the task has dependencies, add them
        if (boTask.Dependencies != null)
        {
            foreach (var dependencyTask in boTask.Dependencies)
                AddDependency(boTask.Id, dependencyTask.Id);
        }
        return idNewTask;
    }


    /// <summary>
    /// Creates a start date for the specified task.
    /// </summary>
    /// <param name="idTask">The ID of the task to create the start date for.</param>
    /// <param name="date">The start date to set for the task.</param>
    /// <exception cref="BO.BlDoesNotExistException">Thrown when the specified task does not exist.</exception>
    /// <exception cref="BO.BlCannotBeUpdatedException">Thrown when not all start dates of previous tasks of the specified task have been updated.</exception>
    /// <exception cref="BO.BlincorrectDateOrderException">Thrown when the date is not earlier than all the estimated end dates of all tasks dependent on the specified task.</exception>
    public void CreateStartDate(int idTask, DateTime date)
    {
        // Read the task from the data access layer
        DO.Task task = _dal!.Task.Read(idTask) ?? throw new BO.BlDoesNotExistException($"Task with ID={idTask} was not found");

        // Retrieve dependencies and check if all their scheduled dates are updated


        if (getDependenciesInList(idTask) != null)
        {
            var dependencies = getDependenciesInList(idTask)!.Select(item => _dal.Task.Read(item.Id));
            // dependencies.Select(item => _dal.Task.Read(item.Id));
            if (dependencies.Select(item => item?.ScheduledDate is null).Any())
                throw new BO.BlCannotBeUpdatedException($"Not all start dates of previous tasks of the task with ID={idTask} are updated");

            // Check if the specified date is earlier than the estimated end dates of all dependent tasks
            if (dependencies.Any(item => item?.CompleteDate < date))
                throw new BO.BlincorrectDateOrderException($"The date is not earlier than all the estimated end dates of all tasks dependent on the task with ID={idTask}");
        }
        try
        {
            // Update the task's scheduled date in the data access layer
            _dal.Task.Update(task with { ScheduledDate = date });
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={idTask} does not exist", ex);
        }
    }
    public void Delete(int idTask) //  אי אפשר למחוק משימות לאחר יצירת לו"ז הפרויקט
    {

        if (_bl.GetProjectStatus() == BO.ProjectStatus.Execution)
            throw new BO.BlTheProjectTimeDoesNotAllowException("Impossible action at this stage of the project - The project start date has already been set");

        //      DO.Task task = _dal!.Task.Read(idTask) ?? throw new BO.BlDoesNotExistException($"Task with ID={idTask} was not found");

        if (!isHaveDependency(idTask))
            throw new BO.BlInvalidInputFormatException("Wrong data");

        try
        {
            _dal.Task.Delete(idTask);
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={idTask} was not found", ex);
        }

    }

    /// <summary>
    /// Retrieves the task with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the task to retrieve.</param>
    /// <returns>The task with the specified ID.</returns>
    /// <exception cref="BO.BlDoesNotExistException">Thrown when the task with the specified ID does not exist.</exception>
    public BO.Task Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does not exist");
        return convertDOtoBO(doTask);
    }

    /// <summary>
    /// Retrieves a task based on the provided filter.
    /// </summary>
    /// <param name="filter">A function used to filter tasks.</param>
    /// <returns>The task that matches the filter.</returns>
    public BO.Task Read(Func<BO.Task, bool>? filter = null)
    {
        if (filter == null)
            return _dal.Task.ReadAll().Select(item => convertDOtoBO(item!)).FirstOrDefault() ?? throw new BO.BlDoesNotExistException("There are no task in the data");
        else
            return _dal.Task.ReadAll().Select(item => convertDOtoBO(item!)).FirstOrDefault(filter) ?? throw new BO.BlDoesNotExistException($"Engineer with {filter} was not found");

    }

    /// <summary>
    /// Retrieves all tasks that match the provided filter.
    /// </summary>
    /// <param name="filter">A function used to filter tasks.</param>
    /// <returns>An enumerable collection of tasks that match the filter.</returns>
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        //if (filter != null)
        //{
        //    return from DO.Task doTask in _dal.Task.ReadAll()
        //           let botask = convertDOtoBO(doTask)
        //           where filter(botask)
        //           select new BO.TaskInList { Id = botask.Id, Alias = botask.Alias, Description = botask.Description, Status = botask.Status };
        //}
        //return from DO.Task doTask in _dal.Task.ReadAll()
        //       let botask = convertDOtoBO(doTask)
        //       select new BO.TaskInList { Id = botask.Id, Alias = botask.Alias, Description = botask.Description, Status = botask.Status };




        if (filter != null)
        {
            // Apply the filter condition to the collection of tasks
            IEnumerable<BO.Task> isFilter = from item in _dal.Task.ReadAll()
                                            let convertItem = convertDOtoBO(item)
                                            where filter(convertItem)
                                            select convertItem;
            // Convert filtered tasks to TaskInList objects
            return (from item in isFilter
                    select new BO.TaskInList { Id = item.Id, Alias = item.Alias, Description = item.Description, Status = item.Status });
        }
        else
            // Retrieve all tasks from the data layer and convert them to TaskInList objects
            return (from item in _dal.Task.ReadAll()
                    let convertItem = convertDOtoBO(item)
                    select new BO.TaskInList { Id = convertItem.Id, Alias = convertItem.Alias, Description = convertItem.Description, Status = convertItem.Status });

    }


    /// <summary>
    /// Updates the details of a task.
    /// </summary>
    /// <param name="boTask">The task object containing the updated details.</param>
    /// <exception cref="BO.BlDoesNotExistException">Thrown when the task to be updated does not exist.</exception>
    /// <exception cref="BO.BlInvalidInputFormatException">Thrown when the provided data is invalid.</exception>
    public void Update(BO.Task boTask)
    {
        // Validate if the task exists
        if (Read(item => item.Id == boTask.Id) is null)
            throw new BO.BlDoesNotExistException($"Task with ID={boTask.Id} was not found");

        // Validate input data
        if (!isValidString(boTask.Alias))
            throw new BO.BlInvalidInputFormatException("Wrong data");

        // Validate engineer existence
        if (boTask.Engineer is not null && _dal.Engineer.Read(boTask.Id) is null)
            throw new BO.BlDoesNotExistException($"Engineer with ID={boTask.Engineer?.Id} was not found");

        // Validate complexity criteria
        DO.Engineer? engineerInTask = _dal.Engineer.Read(item => item?.Id == boTask.Engineer?.Id);
        if (engineerInTask is not null)
        {
            if ((int)engineerInTask!.Level < (int)boTask.Complexity)
                throw new BO.BlInvalidInputFormatException("Fail to update Complexity - Does not fit the criteria");
        }
        try
        {
            // Update task based on project status
            if (_bl.GetProjectStatus() == BO.ProjectStatus.Planing)
            {
                updateDependencies(boTask);
                int? engineerId = boTask.Engineer is not null ? boTask.Engineer.Id : null;
                DO.Task doTask = new DO.Task(boTask.Id, engineerId, false, boTask.Alias, boTask.Description, boTask.CreatedAtDate, boTask.ScheduledDate, boTask.StartDate, boTask.RequiredEffortTime, (DO.EngineerExperience)boTask.Complexity, null, boTask.CompleteDate, boTask.Deliverables, boTask.Remarks);
                _dal.Task.Update(doTask);
            }
            else if (_bl.GetProjectStatus() == BO.ProjectStatus.Execution)
            {
                int? engineerId = boTask.Engineer is not null ? boTask.Engineer.Id : null;
                //     DO.Task doTask = new DO.Task { Id = boTask.Id, Alias = boTask.Alias, EngineerId = engineerId, Deliverables = boTask.Deliverables, Remarks = boTask.Remarks, Description = boTask.Description };
                DO.Task doTask = new DO.Task(boTask.Id, engineerId, false, boTask.Alias, boTask.Description, boTask.CreatedAtDate, boTask.ScheduledDate,
                    bl.Clock.Date, boTask.RequiredEffortTime, (DO.EngineerExperience)boTask.Complexity, null, boTask.CompleteDate, boTask.Deliverables, boTask.Remarks);

                _dal.Task.Update(doTask);
            }
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={boTask.Id} does not exist", ex);
        }
    }


    /// <summary>
    /// Validates an ID to ensure it is greater than 0.
    /// </summary>
    /// <param name="_id">The ID to validate.</param>
    /// <returns>True if the ID is valid; otherwise, false.</returns>
    private bool isValidID(int _id)
    {
        return _id >= 0;
    }

    /// <summary>
    /// Validates a string to ensure it is not empty.
    /// </summary>
    /// <param name="_string">The string to validate.</param>
    /// <returns>True if the string is not empty; otherwise, false.</returns>
    private bool isValidString(string? _string)
    {
        return !string.IsNullOrEmpty(_string);
    }

    /// <summary>
    /// Checks if a task with the given ID already exists.
    /// </summary>
    /// <param name="_id">The ID of the task to check.</param>
    /// <returns>True if no task with the ID exists; otherwise, false.</returns>
    private bool isValidTask(int _id)
    {
        return _dal.Task.Read(_id) == null;
    }

    /// <summary>
    /// Checks if a task with the given ID has dependencies.
    /// </summary>
    /// <param name="_id">The ID of the task to check.</param>
    /// <returns>True if the task has dependencies; otherwise, false.</returns>
    private bool isHaveDependency(int _id)
    {
        return _dal.Dependency.ReadAll(item => item.DependsOnTask == _id) != null;
    }

    /// <summary>
    /// Calculates the status of a task based on its scheduled, start, and completion dates.
    /// </summary>
    /// <param name="task">The task to calculate the status for.</param>
    /// <returns>The status of the task.</returns>
    private BO.Status calculateStatus(DO.Task task)
    {
        if (task.ScheduledDate == null)
            return BO.Status.Unscheduled;
        if (task.StartDate == null)
        {
            if (_bl.Clock.Date > task.ScheduledDate)
                return BO.Status.InJeopredy;
            return BO.Status.Scheduled;
        }
        if (task.CompleteDate == null)
            return BO.Status.OnTrack;
        return BO.Status.Done;
    }


    private BO.EngineerInTask? getEngineerInTask(DO.Task task)
    {
        //    BO.EngineerInTask? engineerInTask = null;
        // Check if an engineer is assigned to the task
        if (task.EngineerId == null || task.EngineerId == 0)
            return null;
        // Retrieve the engineer from the data layer
        DO.Engineer eng = _dal.Engineer.ReadAll().FirstOrDefault(e => e?.Id == task.EngineerId)!; //?? throw new BO.BlDoesNotExistException($"Engineer with ID={task.EngineerId} does Not exist");
                                                                                                  // Return an EngineerInTask object representing the assigned engineer
        return new BO.EngineerInTask() { Id = eng.Id, Name = eng.Name };

    }

    /// <summary>
    /// Converts a data object (DO.Task) to a business object (BO.Task).
    /// </summary>
    /// <param name="item">The data object to convert.</param>
    /// <returns>The converted business object.</returns>
    private BO.Task convertDOtoBO(DO.Task item)
    {
        return new BO.Task
        {
            Id = item.Id,
            Alias = item.Alias,
            Description = item.Description,
            CreatedAtDate = item.CreatedAtDate,
            Status = calculateStatus(item),
            Dependencies = getDependenciesInList(item.Id),
            RequiredEffortTime = item.RequiredEffortTime,
            StartDate = item.StartDate,
            ScheduledDate = item.ScheduledDate,
            ForecastDate = getForecast(item.StartDate, item.ScheduledDate, item.RequiredEffortTime),
            CompleteDate = item.CompleteDate,
            Deliverables = item.Deliverables,
            Remarks = item.Remarks,
            Engineer = getEngineerInTask(item),
            Complexity = (BO.EngineerExperience)item.Copmlexity
        };
    }

    /// <summary>
    /// Calculates the forecasted completion date based on the start date, scheduled date, and required effort time.
    /// </summary>
    /// <param name="_startDate">The start date of the task.</param>
    /// <param name="_scheduledDate">The scheduled date of the task.</param>
    /// <param name="_requiredEffortTime">The amount of time required to complete the task.</param>
    /// <returns>The forecasted completion date of the task.</returns>
    private DateTime? getForecast(DateTime? _startDate, DateTime? _scheduledDate, TimeSpan? _requiredEffortTime)
    {
        // Choose the later of the scheduled date or start date as the reference date
        DateTime? temp = (_scheduledDate > _startDate) ? _scheduledDate : _startDate;

        // Calculate the forecasted completion date by adding the required effort time to the reference date
        return temp + _requiredEffortTime;
    }

    /// <summary>
    /// Retrieves a list of tasks that depend on the task with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the task to retrieve dependencies for.</param>
    /// <returns>A list of tasks that depend on the specified task.</returns>
    private List<BO.TaskInList>? getDependenciesInList(int id) // returns all the tasks that dapends on the task with the id that given
    {
        List<BO.TaskInList>? depTaskInLists = new List<BO.TaskInList>();

        var depList = _dal.Dependency.ReadAll(item => item.DependentTask == id).Select(item => item!.DependsOnTask)!.ToList();

        if (!depList.Any())
        {
            return null;
        }
        // Iterate through dependent task IDs and retrieve corresponding tasks
        foreach (var item in depList)
        {
            // Retrieve dependent task from the data layer
            DO.Task depTask = _dal.Task.ReadAll().FirstOrDefault(t => t?.Id == item) ?? throw new BO.BlDoesNotExistException($"Task with ID={item} does Not exist");
            // Add dependent task to the list with calculated status
            depTaskInLists.Add(new BO.TaskInList { Id = depTask.Id, Alias = depTask.Alias, Description = depTask.Description, Status = calculateStatus(depTask) });
        }
        return depTaskInLists;
    }

    /// <summary>
    /// Converts a data object representing a task to a business object representing a task in list form.
    /// </summary>
    /// <param name="doTask">The data object representing the task to convert.</param>
    /// <returns>A business object representing the task in list form.</returns>
    private BO.TaskInList convertTasktoTaskInList(DO.Task doTask)
    {
        return new BO.TaskInList() { Id = doTask.Id, Alias = doTask.Alias, Description = doTask.Description, Status = calculateStatus(doTask) };
    }

    private void updateDependencies(BO.Task task)
    {
        if (task.Dependencies is not null)
        {
            foreach (var d in task.Dependencies)
            {
                bool isExist = false;
                foreach (var dep in _dal.Dependency.ReadAll())
                {
                    if (dep?.DependentTask == task.Id && dep.DependsOnTask == d.Id)
                        isExist = true;
                }
                if (!isExist)
                    _dal.Dependency.Create(new DO.Dependency(0, task.Id, d.Id));
            }
        }
    }
    //private bool isDependsOnTask(int dependencyTask, int dependsOnTask) //is task with the id 'dependencyTask' depends on the task with the id 'dependsOnTask'
    //{
    //    return _dal.Dependency.Read(dependencyTask)!.DependsOnTask == dependsOnTask;
    //}
    public bool AreAllPreviousTasksCompleted(int id)
    {
        List<BO.TaskInList>? prevTask = getDependenciesInList(id)?.ToList();
        if (prevTask is null)
            return true;
        foreach (var item in prevTask)
        {
            if (item.Status != BO.Status.Done)
                return false;
        }
        return true;

    }
}
