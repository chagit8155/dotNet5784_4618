namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Linq;

internal class Bl : IBl
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public IEngineer Engineer => new EngineerImplementation(this);
    public ITask Task => new TaskImplementation(this);
    public void InitializeDB() => DalTest.Initialization.Do();
    public void ResetDB() => DalTest.Initialization.Reset();

    private static DateTime s_Clock = DateTime.Now;
    public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }
    public DateTime? StartProjectDate
    {
        get => _dal.StartProjectDate;
        set
        {
            if (value < Clock.Date)
                throw new BO.BlincorrectDateOrderException("The project can't be started before today's date");
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
        if (Task.ReadAll(item => item.ScheduledDate is null).Any() == false)
            return BO.ProjectStatus.Execution;
        return BO.ProjectStatus.MidLevel;
    }
    public void CreateSchedule(DateTime date, CreateScheduleOption option/*=CreateScheduleOption.Manually*/, int taskId)
    {
        switch (option)
        {
            case CreateScheduleOption.Automatically:
                createScheduleAuto();
                break;
            case CreateScheduleOption.Manually:
                DateTime? temp = createScheduleOptionManually(taskId);
                if (date < temp)
                    throw new BlCannotBeUpdatedException($"Can't set schedule date before {temp}");
                BO.Task task = Task.Read(taskId);
                task.ScheduledDate = date;
                Task.Update(task);
                break;
        }

    }

    /// <summary>
    /// Creates a schedule for a task manually based on its dependencies.
    /// </summary>
    /// <param name="taskId">The ID of the task for manual scheduling.</param>
    /// <exception cref="BlDoesNotExistException">Thrown when a task or its dependencies do not exist.</exception>
    /// <exception cref="BlCanNotBeNullException">Thrown when a task's forecast date is not set for a dependency.</exception>
    private DateTime? createScheduleOptionManually(int taskId)
    {
        BO.Task task = Task.Read(taskId);
        if (task.Dependencies?.Count() == 0)
        {
            return StartProjectDate;
        }
        DateTime? maxForecast = Clock;
        foreach (var d in task.Dependencies!)
        {
            BO.Task readTask = Task.Read(d.Id);
            if (readTask.ForecastDate == null)
                throw new BlCannotBeUpdatedException("It is not possible to update a task to the previous task no forecast date has been set.");
            if (readTask.ForecastDate > maxForecast)
                maxForecast = readTask.ForecastDate;
        }
        return maxForecast;
    }

    /// <summary>
    /// Creates a schedule for tasks automatically based on their dependencies and project start date.
    /// </summary>
    /// <remarks>
    /// Assumptions:
    /// - The project start date is available.
    /// - Each task has a duration and complexity level.
    /// - Tasks may have dependencies.
    /// - This method updates the scheduled date for each task based on its dependencies and project start date.
    /// </remarks>
    /// <exception cref="BlDoesNotExistException">Thrown when a task or its dependencies do not exist.</exception>
    private void createScheduleAuto()
    {
        // Assumptions:
        // - The project start date is available.
        // - Each task has a duration and complexity level.
        // - Tasks may have dependencies.
        // - This method updates the scheduled date for each task based on its dependencies and project start date.

        DateTime start = _dal.StartProjectDate!.Value;
        IEnumerable<TaskInList> tasksWithoutDep = Task.ReadAll(boTask => boTask.Dependencies?.Count() == 0).ToList();
        // Update scheduled dates for tasks without dependencies to project start date
        foreach (TaskInList task in tasksWithoutDep)
        {
            BO.Task taskWithStartDate = Task.Read(task.Id);
            taskWithStartDate.ScheduledDate = start;
            Task.Update(taskWithStartDate);
        }

        // Update scheduled dates for dependent tasks
        foreach (TaskInList task in tasksWithoutDep)
        {
            updateSceduledDateInDep(task.Id);
        }
    }

    /// <summary>
    /// Updates the scheduled date for tasks dependent on a given task recursively.
    /// </summary>
    /// <param name="id">The ID of the task whose dependents need to have their scheduled dates updated.</param>
    /// <remarks>
    /// This method recursively updates the scheduled date for tasks that depend on the task with the specified ID.
    /// It iterates through all dependent tasks, updating their scheduled dates based on the forecast date of the given task.
    /// If a dependent task already has a scheduled date, it compares the forecast date of the given task with the current scheduled date of the dependent task
    /// and updates the scheduled date if necessary to ensure it reflects the latest possible start date.
    /// </remarks>
    private void updateSceduledDateInDep(int id)
    {
        IEnumerable<TaskInList> dependOnTasks = Task.ReadAll(boTask => boTask.Dependencies!.FirstOrDefault(item => item.Id == id) != null).ToList();
        BO.Task dep = Task.Read(id);
        if (dependOnTasks == null || dependOnTasks.Count() == 1)
            return;
        if (GetProjectStatus() == BO.ProjectStatus.Execution)
            return;
        foreach (TaskInList task in dependOnTasks!)
        {
            BO.Task taskTODoStartDate = Task.Read(task.Id);
            if (taskTODoStartDate.ScheduledDate is null)
                taskTODoStartDate.ScheduledDate = dep.ForecastDate;
            else
                taskTODoStartDate.ScheduledDate = (dep.ForecastDate > taskTODoStartDate.ScheduledDate) ? dep.ForecastDate : taskTODoStartDate.ScheduledDate;
            Task.Update(taskTODoStartDate);
            updateSceduledDateInDep(taskTODoStartDate.Id);
        }
        var bdika = Task.ReadAll();
    }

    public IEnumerable<BO.Task> UpdateManuallyList()
    {
        Queue<int> q = new Queue<int>();
        List<BO.Task> tasks = new List<BO.Task>();
        IEnumerable<TaskInList> tasksWithoutDep = Task.ReadAll(boTask => boTask.Dependencies?.Count() == 0).ToList();
        foreach (TaskInList task in tasksWithoutDep)
        {
            q.Enqueue(task.Id);
        }

        while (q.Count > 0)
        {
            BO.Task currentTask = Task.Read(q.First());
            IEnumerable<TaskInList> dependOnTasks = Task.ReadAll(boTask => boTask.Dependencies!.FirstOrDefault(item => item.Id == currentTask.Id) != null).ToList();
            if (dependOnTasks.Count() != 0)
                foreach (var item in dependOnTasks)
                {
                    q.Enqueue(item.Id);
                }
            tasks.RemoveAll(t => t.Id == q.First());
            tasks.Add(currentTask);
            q.Dequeue();
        }
        return tasks;
    }
    public IEnumerable<TaskForGant>? CreateGantList()
    {
        var lst = from item in Task.ReadAll()
                  let task = Task.Read(item.Id)
                  select new TaskForGant
                  {
                      TaskId = task.Id,
                      TaskAlias = task.Alias,
                      EngineerId = task.Engineer?.Id,
                      EngineerName = task.Engineer?.Name,
                      StartDate = calculateStartDate(task.StartDate, task.ScheduledDate),
                      CompleteDate = calculateCompleteDate(task.ForecastDate, task.CompleteDate),
                      DependentTasks = lstDependentId(task.Dependencies),
                      Status = task.Status
                  };
        return lst.OrderBy(task => task.StartDate);

    }
    private IEnumerable<int> lstDependentId(IEnumerable<TaskInList>? dependencies)
    {
        return from dep in dependencies
               select dep.Id;
    }

    private DateTime calculateStartDate(DateTime? startDate, DateTime? scheduledDate)
    {
        if (startDate == null)
            return scheduledDate ?? Clock;
        else
            return startDate ?? Clock;
    }

    private DateTime calculateCompleteDate(DateTime? forecastDate, DateTime? completeDate)
    {
        if (completeDate == null)
            return forecastDate ?? Clock;
        else
            return completeDate ?? Clock;
    }

    ///// <summary>
    ///// פונקציה זו מקבלת את רשימת המשימות ואת רשימת התלויות ביניהן ומבצעת מיון טופולוגי על המשימות.
    ///// מיון טופולוגי מאפשר לנו לסדר את המשימות בסדר הנכון שבו המשימות ניתנות לביצוע, 
    ///// כך שלכל משימה תהיה תלות רק במשימות שכבר בוצעו או שאין להן תלות.
    ///// </summary>
    ///// <param name="tasks">רשימת המשימות למיון</param>
    ///// <param name="dependencies">רשימת התלויות בין המשימות</param>
    ///// <returns>רשימת המשימות ממוינת על פי מיון טופולוגי</returns>
    //public IEnumerable<TaskInList?> TopologicalSort()
    //{
    //    IEnumerable<BO.TaskInList> tasks = Task.ReadAll();

    //    IEnumerable<DO.Dependency> dependencies = _dal.Dependency.ReadAll()!;
    //    // יצירת גרף ריק
    //    Dictionary<int, List<int?>> graph = new Dictionary<int, List<int?>>();
    //    // מילוי הגרף בהתאם לתלויות
    //    foreach (var task in tasks)
    //    {
    //        graph.Add(task.Id, new List<int?>());
    //    }
    //    foreach (var dependency in dependencies)
    //    {
    //        if (dependency.DependentTask != null)
    //        {
    //            graph[(int)dependency.DependentTask].Add(dependency.DependsOnTask);
    //        }
    //    }

    //    // מיון טופולוגי
    //    Stack<int?> stack = new Stack<int?>();
    //    HashSet<int> visited = new HashSet<int>();
    //    foreach (var key in graph.Keys)
    //    {
    //        if (!visited.Contains(key))
    //        {
    //            TopologicalSortUtil(key, graph, stack, visited);
    //        }
    //    }

    //    // בניית רשימת המשימות מסודרת על פי מיון טופולוגי
    //    List<TaskInList?> sortedTasks = new List<TaskInList?>();
    //    //while (stack.Any())
    //    //{
    //    //   // int id = stack.TryPop();

    //    //    sortedTasks.Add(tasks.FirstOrDefault(t => stack.Pop(t.Id)));
    //    //}
    //    for (int i = 0; i < stack.Count(); i++)
    //    {
    //        // if
    //        int? x = stack.Pop();

    //        sortedTasks.Add(tasks.FirstOrDefault(t => x == t.Id));
    //    }

    //    return sortedTasks;
    //}

    ///// <summary>
    ///// פונקציה עזר למיון טופולוגי.
    ///// פונקציה זו מבצעת סריקה רקורסיבית של הגרף ומוסיפה את הצמתים לערימה בסדר הנכון לפי מיון טופולוגי.
    ///// </summary>
    ///// <param name="v">הצומת הנוכחית</param>
    ///// <param name="graph">הגרף שבו מבוצע הסריקה</param>
    ///// <param name="stack">הערימה שבה מתבצע הסידור לפי מיון טופולוגי</param>
    ///// <param name="visited">רשימת הצמתים שנבדקו</param>
    //private void TopologicalSortUtil(int v, Dictionary<int, List<int?>> graph, Stack<int?> stack, HashSet<int> visited)
    //{
    //    // הוספת הצומת לרשימת הצמתים שביקרנו בהם
    //    visited.Add(v);
    //    // עבור כל שכני הצומת
    //    foreach (var neighbor in graph[v])
    //    {
    //        if (neighbor == null) continue;
    //        else
    //        {
    //            // אם השכן לא נבדק עדיין, נבצע עליו רקורסיה
    //            if (!visited.Contains((int)neighbor))
    //            {
    //                TopologicalSortUtil((int)neighbor, graph, stack, visited);
    //            }
    //        }
    //    }
    //    // כשסיימנו לבדוק את כל השכנים, נכניס את הצומת לערימה
    //    stack.Push(v);
    //}

}



