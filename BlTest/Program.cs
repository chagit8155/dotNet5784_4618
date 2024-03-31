namespace BlTest;
using Dal;
using System.Collections.Generic;
using System.Xml.Linq;

internal class Program //Bl
{
    static readonly DalApi.IDal s_dal = DalApi.Factory.Get;
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    private static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y")
            {
                deletedAllData();
                DalTest.Initialization.Do();
                Console.WriteLine("The initialization is done.");
            }

            showMainMenu();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    static void showMainMenu()
    {
        try
        {
            bool isNotExit = true;
            while (isNotExit)
            {
                //  Console.Clear();
                Console.WriteLine("========== Main Menu ==========");
                Console.WriteLine("Hey, select an entity you want to check:");
                Console.WriteLine("0 -> Exit main menu");
                Console.WriteLine("1 -> Entity Engineer");
                Console.WriteLine("2 -> Entity Task");
                Console.WriteLine("3 -> Create schedual");
                //  Console.WriteLine("Data Initialization -> enter 4");
                string choose = Console.ReadLine()!;
                BO.MainMenu chooseToString = (BO.MainMenu)int.Parse(choose);
                if (chooseToString == BO.MainMenu.Engineer)
                    showEngineerSubMenu(chooseToString);
                if (chooseToString == BO.MainMenu.Task)
                    showTaskSubMenu(chooseToString);
                if (chooseToString == BO.MainMenu.Schedual)
                {
                    Console.WriteLine("Enter start project Date:");
                    DateTime start = DateTime.TryParse(Console.ReadLine(), out DateTime st) ? st : throw new BO.BlInvalidInputFormatException("Enter in dateTime format");
                    s_bl.StartProjectDate = start;
                    XElement root = XMLTools.LoadListFromXMLElement("data-config").Element("StartProjectDate")!;
                    root.Value = s_bl.StartProjectDate.ToString()!;
                    XMLTools.SaveListToXMLElement(root, "data-config");
                    Console.WriteLine("If you want to create a date manually, then enter an ID-Task , if not enter -1");
                    int taskId = int.Parse(Console.ReadLine()!);
                    if (taskId == -1)
                    {
                        s_bl.CreateSchedule(s_bl.Clock);
                        continue;
                    }
                    else
                    {
                        s_bl.CreateSchedule(s_bl.Clock, BO.CreateScheduleOption.Manually, taskId);
                        continue;
                    }
                }
                else if (chooseToString == BO.MainMenu.Exit)
                {
                    isNotExit = false;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            if (ex.InnerException != null)
                Console.WriteLine(ex.InnerException);
        }
    }

    /// <summary>
    /// A method that displays a submenu for engineer entity
    /// </summary>
    /// <param name="entity">the choose in the main menu</param>
    static void showEngineerSubMenu(BO.MainMenu entity)
    {
        try
        {
            bool isNotExit = true;
            while (isNotExit)
            {
                Console.WriteLine("");
                Console.WriteLine("Now, select the method you want to perform, to:");
                Console.WriteLine("0 -> Exit main menu");
                Console.WriteLine("1 -> Create a new object ");
                Console.WriteLine("2 -> Read an object by ID ");
                Console.WriteLine("3 -> Read all the list of all objects ");
                Console.WriteLine("4 -> Update an existing object ");
                Console.WriteLine("5 -> Delete an existing object from a list  ");
                string choose = Console.ReadLine()!;
                BO.EngineerSubMenu chooseToString = (BO.EngineerSubMenu)int.Parse(choose);
                switch (chooseToString)
                {
                    case BO.EngineerSubMenu.Exit:
                        isNotExit = false;
                        break;

                    case BO.EngineerSubMenu.Create:
                        createEntity(entity);
                        break;

                    case BO.EngineerSubMenu.Read:
                        readSEntity(entity);
                        break;

                    case BO.EngineerSubMenu.ReadAll:
                        readAllEntites(entity);
                        break;

                    case BO.EngineerSubMenu.Update:
                        updateEntity(entity);
                        break;

                    case BO.EngineerSubMenu.Delete:
                        deleteEntity(entity);
                        break;

                    default:
                        throw new BO.BlInvalidInputFormatException("Enter a number between 0-5");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    /// <summary>
    /// A method that displays a submenu for task entity
    /// </summary>
    /// <param name="entity">the choose in the main menu</param>
    static void showTaskSubMenu(BO.MainMenu entity)
    {
        try
        {
            bool isNotExit = true;
            while (isNotExit)
            {
                Console.WriteLine("");
                Console.WriteLine("Now, select the method you want to perform, to:");
                Console.WriteLine("0 -> Exit main menu ");
                Console.WriteLine("1 -> Create a new object");
                Console.WriteLine("2 -> Read an object by ID ");
                Console.WriteLine("3 -> Read all the list of all objects ");
                Console.WriteLine("4 -> Update an existing object");
                Console.WriteLine("5 -> Delete an existing object from a list ");
                Console.WriteLine("6 -> Add a dependency ");
                Console.WriteLine("7 -> Creates a start date for the specified task");
                string choose = Console.ReadLine()!;
                BO.TaskSubMenu chooseToString = (BO.TaskSubMenu)int.Parse(choose);
                switch (chooseToString)
                {
                    case BO.TaskSubMenu.Exit:
                        isNotExit = false;
                        break;

                    case BO.TaskSubMenu.Create:
                        createEntity(entity);
                        break;

                    case BO.TaskSubMenu.Read:
                        readSEntity(entity);
                        break;

                    case BO.TaskSubMenu.ReadAll:
                        readAllEntites(entity);
                        break;

                    case BO.TaskSubMenu.Update:
                        updateEntity(entity);
                        break;

                    case BO.TaskSubMenu.Delete:
                        deleteEntity(entity);
                        break;
                    case BO.TaskSubMenu.AddDependency:

                        Console.WriteLine("Enter Id task:");
                        int idT = checkValidId();
                        Console.WriteLine("Enter Id of the dependent task:");
                        int idD = checkValidId();
                        s_bl.Task.AddDependency(idT, idD);
                        break;
                    case BO.TaskSubMenu.CreateStartDate:
                        Console.WriteLine("Enter Id task:");
                        int idTask = checkValidId();
                        Console.WriteLine("Enter star date:");
                        DateTime start = DateTime.TryParse(Console.ReadLine(), out DateTime st) ? st : throw new BO.BlInvalidInputFormatException("Enter in dateTime format");
                        s_bl.Task.CreateStartDate(idTask, start);
                        break;

                    default:
                        throw new BO.BlInvalidInputFormatException("Enter a number between 0-7");

                }
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.GetType().Name);  //?
            Console.WriteLine(ex.Message);
            if (ex.InnerException is not null)
                Console.WriteLine(ex.InnerException);
        }
    }

    /// <summary>
    /// Add method of some received entity
    /// </summary>
    /// <param name="entity"></param>
    static void createEntity(BO.MainMenu entity)
    {

        switch (entity)
        {
            case BO.MainMenu.Engineer:
                Console.WriteLine(s_bl.Engineer.Create(addEngineer()));
                break;

            case BO.MainMenu.Task:
                Console.WriteLine(s_bl!.Task.Create(addTask()));
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// An object presentation method of some entity received as a parameter
    /// </summary>
    /// <param name="entity"></param>
    /// <exception cref="Exception"></exception>
    static void readSEntity(BO.MainMenu entity)
    {
        switch (entity)
        {
            case BO.MainMenu.Engineer:
                Console.WriteLine("Enter Id:");
                int idE = checkValidId();
                Console.WriteLine(s_bl.Engineer.Read(idE));
                break;

            case BO.MainMenu.Task:
                Console.WriteLine("Enter Id:");
                int idT = checkValidId();
                Console.WriteLine(s_bl.Task!.Read(idT));
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// A method for displaying the list of all objects of some entity received as a parameter
    /// </summary>
    /// <param name="entity"></param>
    static void readAllEntites(BO.MainMenu entity)
    {
        switch (entity)
        {
            case BO.MainMenu.Exit:
                return;

            case BO.MainMenu.Engineer:
                foreach (var _engineer in s_bl.Engineer.ReadAll())
                    Console.WriteLine(_engineer);
                break;

            case BO.MainMenu.Task:
                foreach (var _task in s_bl.Task!.ReadAll())
                    Console.WriteLine(_task);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// An object update method of some entity received as a parameter
    /// </summary>
    /// <param name="entity"></param>
    static void updateEntity(BO.MainMenu entity)
    {
        switch (entity)
        {
            case BO.MainMenu.Engineer:

                Console.WriteLine("Enter the task's id that the engineer working on:");
                int taskId = int.Parse(Console.ReadLine() ?? "");
                BO.Engineer updateEngineer = addEngineer();
                updateEngineer.Task = new BO.TaskInEngineer() { Id = taskId };
                s_bl.Engineer!.Update(updateEngineer);
                break;

            case BO.MainMenu.Task:
                Console.WriteLine("Enter task's id you want to update");
                int idT = int.Parse(Console.ReadLine() ?? "0");
                BO.Task updateTask = addTask(idT);
                Console.WriteLine("Enter the engineer's id that is working on the task, enter -1 if you don't want to update an engineer");
                int engineerId = int.Parse(Console.ReadLine()!);
                if (engineerId != -1)
                {
                    BO.EngineerInTask engineer = new BO.EngineerInTask() { Id = engineerId };
                    updateTask.Engineer = engineer;
                }
                Console.WriteLine("Do you want a start date? (Y/N)");
                string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
                if (ans == "Y")
                {
                    Console.WriteLine("Enter Start Date");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                        throw new BO.BlInvalidInputFormatException("Enter in dateTime format");
                    updateTask.StartDate = startDate;
                }
                s_bl!.Task.Update(updateTask);
                break;

            default:
                break;
        }
    }


    /// <summary>
    /// A method to delete an object of some entity received as a parameter
    /// </summary>
    /// <param name="entity"></param>
    static void deleteEntity(BO.MainMenu entity)
    {
        switch (entity)
        {
            case BO.MainMenu.Exit:
                return;
            case BO.MainMenu.Engineer:
                Console.WriteLine("Enter Id:");
                int idE = checkValidId();
                s_bl.Engineer.Delete(idE);
                break;

            case BO.MainMenu.Task:
                Console.WriteLine("Enter Id:");
                int idT = checkValidId();
                s_bl.Task.Delete(idT);
                break;
            default:
                break;
        }
    }

    /// <summary>
    ///Helper method, Addition method of an engineer entity
    /// </summary>
    /// <returns></returns>
    static BO.Engineer addEngineer()
    {
        //Receiving the data from the user and inserting it into the variables
        Console.WriteLine("Enter id");
        int _id = int.Parse(Console.ReadLine() ?? "0");
        Console.WriteLine("Enter name");
        string _name = Console.ReadLine() ?? "";
        Console.WriteLine("Enter cost");
        double _cost = int.Parse(Console.ReadLine() ?? "0");
        Console.WriteLine("Enter email");
        string _email = Console.ReadLine() ?? "";
        Console.WriteLine("Enter the Level of the engineer");
        BO.EngineerExperience _level = (BO.EngineerExperience)int.Parse(Console.ReadLine() ?? "0");
        // Calling the constructor action with all the variables and returning the object
        BO.Engineer newEng = new BO.Engineer() { Id = _id, Name = _name, Cost = _cost, Email = _email, Level = _level };
        return newEng;
    }

    /// <summary>
    /// Helper method, Addition method of an Task entity
    /// </summary>
    /// <returns></returns>
    static BO.Task addTask(int id = 0)
    {
        //Receiving the data from the user and inserting it into the variables     
        Console.WriteLine("Enter alias: ");
        string _alias = Console.ReadLine() ?? "";
        Console.WriteLine("Enter description:");
        string _description = Console.ReadLine() ?? "";
        Console.WriteLine("Enter required effort time: ");
        if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan _requiredEffortTime))
            throw new BO.BlInvalidInputFormatException("Invalid TimeSpan format");
        Console.WriteLine("Enter deliverables:");
        string _deliverables = Console.ReadLine() ?? "";
        Console.WriteLine("Enter remarks: (possible)");
        string _remarks = Console.ReadLine() ?? "";
        //Console.WriteLine("Enter engineer's id: ");
        //int _engineerId = int.Parse(Console.ReadLine() ?? "");
        //Console.WriteLine("Enter engineer's name: ");
        //string _engineerName = Console.ReadLine() ?? "";
        //BO.EngineerInTask? _engineer = new BO.EngineerInTask() { Id = _engineerId, Name = _engineerName };
        Console.WriteLine("Enter complexity (engineer's level experience):");
        BO.EngineerExperience _complexity = (BO.EngineerExperience)int.Parse(Console.ReadLine() ?? "0");
        if ((int)_complexity > 4)
            throw new BO.BlInvalidInputFormatException($"There is no task complexity level for the input number : {(int)_complexity}");
        List<BO.TaskInList>? _dependencies = null;
        Console.WriteLine("Enter the ID of the tasks that this task will depend on, and to finish, enter -1");
        int numDep = int.Parse(Console.ReadLine() ?? "0");
        while (numDep != -1)
        {
            if (numDep >= 0)
            {
                _dependencies = new List<BO.TaskInList>();
                BO.Task? tempTask = s_bl.Task.Read(numDep);
                _dependencies.Add(new BO.TaskInList() { Id = tempTask.Id, Alias = tempTask.Alias, Description = tempTask.Description, Status = tempTask.Status });
            }
            numDep = int.Parse(Console.ReadLine() ?? "0");
        }
        //A constructive call to action with all variables and returning the object

        BO.Task newTask = new BO.Task() { Id = id, Alias = _alias, Description = _description, RequiredEffortTime = _requiredEffortTime, Deliverables = _deliverables, Remarks = _remarks /*, Engineer = _engineer*/, Complexity = _complexity, Dependencies = _dependencies };
        return newTask;
    }
    private static int checkValidId()
    {
        bool flag;
        flag = int.TryParse(Console.ReadLine(), out int id);
        return flag ? id : throw new BO.BlInvalidInputFormatException("Invalid input - Id is not a number");
    }

    /// <summary>
    /// A method that deleted all the data of the entities
    /// </summary>
    static void deletedAllData()
    {
        s_dal.Dependency.DeleteAll();
        s_dal.Engineer.DeleteAll();
        s_dal.Task.DeleteAll();
    }
   
}
// Hey Sara, I hope you are having a good day!:)