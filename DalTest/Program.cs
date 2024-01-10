namespace DalTest;
using Dal;
using DalApi;
using DO;
using System.Diagnostics.Metrics;
using System.Dynamic;
using System.Reflection.Emit;
using System.Xml.Linq;

internal class Program
{
    private static ITask? s_dalTask = new TaskImplementation();
    private static IEngineer? s_dalEngineer = new EngineerImplementationcs();
    private static IDependency? s_dalDependency = new DependencyImplementation();

    public static void Main(string[] args)
    {
        try
        {
            Initialization.Do(s_dalEngineer, s_dalDependency, s_dalTask);
            MainMenuDisplay();
           // Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    /// <summary>
    /// A method that displays a main menu
    /// </summary>
    static void MainMenuDisplay()
    {
        try
        {
            bool isNotExit = true;
            while (isNotExit)
            {
                Console.Clear();
                Console.WriteLine("========== Main Menu ==========");
                Console.WriteLine("Hey, select an entity you want to check:");
                Console.WriteLine("Exit main menu -> enter 0");
                Console.WriteLine("Entity Engineer -> enter 1");
                Console.WriteLine("Entity Dependency -> enter 2");
                Console.WriteLine("Entity Task -> enter 3");
                string choose = Console.ReadLine()!;
                MainMenu chooseToString = (MainMenu)int.Parse(choose);
                if (chooseToString != MainMenu.Exit)
                    SubMenuDisplay(chooseToString);
                else 
                {
                    isNotExit = false;
                }                   
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    /// <summary>
    /// A method that displays a submenu for some entity
    /// </summary>
    /// <param name="entity">the choose in the main menu</param>
    static void SubMenuDisplay(MainMenu entity)
    {
        try
        {
            bool isNotExit = true;
            while (isNotExit)
            {
                Console.WriteLine("");
                Console.WriteLine("Now, select the method you want to perform:");
                Console.WriteLine("Exit main menu -> enter 0");
                Console.WriteLine("Create a new object -> enter 1");
                Console.WriteLine("Read an object by ID -> enter 2");
                Console.WriteLine("Read all the list of all objects -> enter 3");
                Console.WriteLine("Update an existing object -> enter 4");
                Console.WriteLine("Delete an existing object from a list -> enter 5");
                string choose = Console.ReadLine()!;
                SubMenu chooseToString = (SubMenu)int.Parse(choose);
                switch (chooseToString)
                {
                    case SubMenu.Exit:
                        isNotExit = false;
                        break;

                    case SubMenu.Create:
                        CreateEntity(entity);
                        break;

                    case SubMenu.Read:
                        ReadSEntity(entity);
                        break;

                    case SubMenu.ReadAll:
                        ReadAllEntity(entity);
                        break;

                    case SubMenu.Update:
                        UpdateEntity(entity);
                        break;

                    case SubMenu.Delete:
                        DeleteEntity(entity);
                        break;

                    default:
                        break;
                }
            }
        }
        catch (Exception ex) 
        {
            Console.WriteLine(ex.Message);
        }
    }
    /// <summary>
    /// Add method of some received entity
    /// </summary>
    /// <param name="entity"></param>
    static void CreateEntity(MainMenu entity)
    {

        switch (entity)
        {
            case MainMenu.Engineer:
                Console.WriteLine(s_dalEngineer!.Create(AddEngineer()));
                break;

            case MainMenu.Dependency:
                Console.WriteLine(s_dalDependency!.Create(AddDependency()));
                break;

            case MainMenu.Task:
                Console.WriteLine(s_dalTask!.Create(AddTask()));
                break;

            default:
                break;
        }
    }
    /// <summary>
    /// Helper method, Addition method of an Dependency entity
    /// </summary>
    /// <returns></returns>
    static Dependency AddDependency()
    {
        //Receiving the data from the user and inserting it into the variables
        Console.WriteLine("Enter Dependent Task, and the Depends On Task");
        int _dependentTask = int.Parse(Console.ReadLine() ?? "0");
        int _dependsOnTask = int.Parse(Console.ReadLine() ?? "0");
       // Calling the constructor action with all the variables and returning the object
        Dependency newDep = new (0, _dependentTask, _dependsOnTask);
        return newDep;
    }
    /// <summary>
    ///Helper method, Addition method of an engineer entity
    /// </summary>
    /// <returns></returns>
    static Engineer AddEngineer()
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
        EngineerExperience _level = (EngineerExperience)int.Parse(Console.ReadLine() ?? "0");
        // Calling the constructor action with all the variables and returning the object
        Engineer newEng = new (_id, _name, _cost, _email, _level);
        return newEng;
    }
    /// <summary>
    /// Helper method, Addition method of an Task entity
    /// </summary>
    /// <returns></returns>
    static DO.Task AddTask()
    {
        //Receiving the data from the user and inserting it into the variables
        Console.WriteLine("Enter engineer's id: ");
        int _engineerId = int.Parse(Console.ReadLine() ?? "");
        Console.WriteLine("Is the task is milestone? (Enter true/false)");
        bool _isMileStone;
        bool flag = bool.TryParse(Console.ReadLine(), out _isMileStone);
        _isMileStone = flag ? _isMileStone : false;
        Console.WriteLine("Enter alias: ");
        string _alias = Console.ReadLine() ?? "";
        Console.WriteLine("Enter description:");
        string _description = Console.ReadLine() ?? "";
        Console.WriteLine("Enter created date:");
        DateTime _createdInDate;
        flag = DateTime.TryParse(Console.ReadLine(), out _createdInDate);
        _createdInDate = flag ? _createdInDate : DateTime.Today;
        Console.WriteLine("Enter scheduled date(null):");
        DateTime _scheduledDate;
        flag = DateTime.TryParse(Console.ReadLine(), out _scheduledDate);
        _scheduledDate = flag ? _scheduledDate : DateTime.Today;
        Console.WriteLine("Enter start date:");
        DateTime _startDate;
        flag = DateTime.TryParse(Console.ReadLine(), out _startDate);
        _startDate = flag ? _startDate : DateTime.Today;
        Console.WriteLine("Enter required effort time: ");
        TimeSpan _requiredEffortTime;
        flag = TimeSpan.TryParse(Console.ReadLine(), out _requiredEffortTime);
        _requiredEffortTime = flag ? _requiredEffortTime : TimeSpan.Zero;
        Console.WriteLine("Enter complete date:");
        DateTime _completeDate;
        flag = DateTime.TryParse(Console.ReadLine(), out _completeDate);
        _completeDate = flag ? _completeDate : DateTime.Today;
        Console.WriteLine("Enter deadline date:");
        DateTime _deadline;
        flag = DateTime.TryParse(Console.ReadLine(), out _deadline);
        _deadline = flag ? _deadline : DateTime.Today;
        Console.WriteLine("Enter complexity (engineer's level experienc):");
        EngineerExperience _complexity = (EngineerExperience)int.Parse(Console.ReadLine() ?? "0");
        Console.WriteLine("Enter deliverables:");
        string _deliverables = Console.ReadLine() ?? "";
        Console.WriteLine("Enter remarks: (possible)");
        string _remarks = Console.ReadLine() ?? "";
        //A constructive call to action with all variables and returning the object
        DO.Task newTask = new DO.Task(0, _engineerId, _isMileStone, _alias, _description, _createdInDate, _scheduledDate, _startDate, _requiredEffortTime, _complexity, _deadline, _completeDate, _deliverables, _remarks);
        return newTask;
    }
    /// <summary>
    /// An object presentation method of some entity received as a parameter
    /// </summary>
    /// <param name="entity"></param>
    /// <exception cref="Exception"></exception>
    static void ReadSEntity(MainMenu entity)
    {
        switch (entity)
        {
            case MainMenu.Engineer:
                Console.WriteLine("Enter Id:");
                int idE = int.Parse(Console.ReadLine()!);
                if (s_dalEngineer!.Read(idE) != null)
                    Console.WriteLine(s_dalEngineer!.Read(idE));
                else
                    throw new Exception($"The engineer with the ID={idE} was not found");
                break;

            case MainMenu.Dependency:
                Console.WriteLine("Enter Id:");
                int idD = int.Parse(Console.ReadLine()!);
                if (s_dalDependency!.Read(idD) != null)
                    Console.WriteLine(s_dalDependency!.Read(idD));
                else
                    throw new Exception($"The dependency with the ID={idD} was not found");
                break;

            case MainMenu.Task:
                Console.WriteLine("Enter Id:");
                int idT = int.Parse(Console.ReadLine()!);
                if (s_dalTask!.Read(idT) != null)
                    Console.WriteLine(s_dalTask!.Read(idT));
                else
                    throw new Exception($"The task with the ID={idT} was not found");
                break;
            default:
                break;

        }
    }
    /// <summary>
    /// A method for displaying the list of all objects of some entity received as a parameter
    /// </summary>
    /// <param name="entity"></param>
    static void ReadAllEntity(MainMenu entity)
    {
        switch (entity)
        {
            case MainMenu.Exit:
                return;

            case MainMenu.Engineer:
                foreach (var _engineer in s_dalEngineer!.ReadAll())
                    Console.WriteLine(_engineer);
                break;

            case MainMenu.Dependency:
                foreach (var _dependency in s_dalDependency!.ReadAll())
                    Console.WriteLine(_dependency);
                break;

            case MainMenu.Task:
                foreach (var _task in s_dalTask!.ReadAll())
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
    static void UpdateEntity(MainMenu entity)
    {
        switch (entity)
        {
            case MainMenu.Engineer:
                s_dalEngineer!.Update(AddEngineer());
                break;

            case MainMenu.Dependency:
                s_dalDependency!.Update(AddDependency());
                break;
            case MainMenu.Task:

                s_dalTask!.Update(AddTask());
                break;

            default:
                break;
        }
    }
    //static void UpdateEngineer(MainMenu entity)
    //{

    //    Console.WriteLine("Enter Id:");
    //    int idE = int.Parse(Console.ReadLine()!);
    //    if (s_dalEngineer!.Read(idE) != null)
    //        Console.WriteLine(s_dalEngineer!.Read(idE));

    //}
    /// <summary>
    /// A method to delete an object of some entity received as a parameter
    /// </summary>
    /// <param name="entity"></param>
    /// <exception cref="Exception"></exception>
    static void DeleteEntity(MainMenu entity)
    {
        switch (entity)
        {
            case MainMenu.Exit:
                return;
            case MainMenu.Engineer:
                Console.WriteLine("Enter Id:");
                int idE = int.Parse(Console.ReadLine()!);
                if (s_dalEngineer!.Read(idE) != null)
                    s_dalEngineer.Delete(idE);
                else
                    throw new Exception($"The engineer with the ID={idE} was not found");
                break;
            case MainMenu.Dependency:
                int idD = int.Parse(Console.ReadLine()!);
                if (s_dalDependency!.Read(idD) != null)
                    s_dalDependency.Delete(idD);
                else
                    throw new Exception($"The dependency with the ID={idD} was not found");
                break;
            case MainMenu.Task:
                int idT = int.Parse(Console.ReadLine()!);
                if (s_dalTask!.Read(idT) != null)
                    s_dalTask.Delete(idT);
                else
                    throw new Exception($"The task with the ID={idT} was not found");
                break;
            default:
                break;
        }
    }
}
// Hey there, tester, I hope you have a good day!:)

