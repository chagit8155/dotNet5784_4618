namespace DalTest;
using DalApi;
using DO;
using System;


internal class Program
{
    // static readonly IDal s_dal = new DalList(); //stage 2
    // static readonly IDal s_dal = new DalXml(); //stage 3
    static readonly IDal s_dal = Factory.Get; //stage 4, Initializes the appropriate class according to the configuration file
    public static void Main(string[] args)
    {
        try
        {
            //Initialization.Do(s_dal); //stage 2
            showMainMenu();
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
                Console.WriteLine("Exit main menu -> enter 0");
                Console.WriteLine("Entity Engineer -> enter 1");
                Console.WriteLine("Entity Dependency -> enter 2");
                Console.WriteLine("Entity Task -> enter 3");
                Console.WriteLine("Data Initialization -> enter 4");
                string choose = Console.ReadLine()!;
                MainMenu chooseToString = (MainMenu)int.Parse(choose);
                if (chooseToString == MainMenu.Initialization) //if the user entered 4
                {
                    Console.WriteLine("Would you like to create Initial data? (Y/N)"); //stage 3
                    string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
                    if (ans == "Y") //stage 3
                    {
                        deletedAllData(); //delete all the data in the xml files
                        Initialization.Do(); //stage 4 
                        Console.WriteLine("The initialization is done.");
                        continue;
                    }
                    else if (ans == "N") { continue; }
                }
                if (chooseToString != MainMenu.Exit)
                    subMenuDisplay(chooseToString);
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
    static void subMenuDisplay(MainMenu entity)
    {
        try
        {
            bool isNotExit = true;
            while (isNotExit)
            {
                Console.WriteLine("");
                Console.WriteLine("Now, select the method you want to perform, to:");
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
                        createEntity(entity);
                        break;

                    case SubMenu.Read:
                        readSEntity(entity);
                        break;

                    case SubMenu.ReadAll:
                        readAllEntites(entity);
                        break;

                    case SubMenu.Update:
                        updateEntity(entity);
                        break;

                    case SubMenu.Delete:
                        deleteEntity(entity);
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
    static void createEntity(MainMenu entity)
    {

        switch (entity)
        {
            case MainMenu.Engineer:
                Console.WriteLine(s_dal.Engineer!.Create(addEngineer()));
                break;

            case MainMenu.Dependency:
                Console.WriteLine(s_dal.Dependency!.Create(addDependency()));
                break;

            case MainMenu.Task:
                Console.WriteLine(s_dal!.Task.Create(addTask()));
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Helper method, Addition method of an Dependency entity
    /// </summary>
    /// <returns></returns>
    static Dependency addDependency()
    {
        //Receiving the data from the user and inserting it into the variables
        Console.WriteLine("Enter Dependent Task, and the Depends On Task");
        int _dependentTask = int.Parse(Console.ReadLine() ?? "0");
        int _dependsOnTask = int.Parse(Console.ReadLine() ?? "0");
        // Calling the constructor action with all the variables and returning the object
        Dependency newDep = new(0, _dependentTask, _dependsOnTask);
        return newDep;
    }
    /// <summary>
    ///Helper method, Addition method of an engineer entity
    /// </summary>
    /// <returns></returns>

    static Engineer addEngineer()
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
        Engineer newEng = new(_id, _name, _cost, _email, _level);
        return newEng;
    }

    /// <summary>
    /// Helper method, Addition method of an Task entity
    /// </summary>
    /// <returns></returns>
    static DO.Task addTask()
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
        DO.Task newTask = new(0, _engineerId, _isMileStone, _alias, _description, _createdInDate, _scheduledDate, _startDate, _requiredEffortTime, _complexity, _deadline, _completeDate, _deliverables, _remarks);
        return newTask;
    }

    /// <summary>
    /// An object presentation method of some entity received as a parameter
    /// </summary>
    /// <param name="entity"></param>
    /// <exception cref="Exception"></exception>
    static void readSEntity(MainMenu entity)
    {
        switch (entity)
        {
            case MainMenu.Engineer:
                Console.WriteLine("Enter Id:");
                int idE = int.Parse(Console.ReadLine()!);
                if (s_dal.Engineer!.Read(idE) != null)
                    Console.WriteLine(s_dal.Engineer!.Read(idE));
                else
                    throw new DalDoesNotExistsException($"Engineer with the ID={idE} was not found");
                break;

            case MainMenu.Dependency:
                Console.WriteLine("Enter Id:");
                int idD = int.Parse(Console.ReadLine()!);
                if (s_dal.Dependency!.Read(idD) != null)
                    Console.WriteLine(s_dal.Dependency!.Read(idD));
                else
                    throw new DalDoesNotExistsException($"Dependency with the ID={idD} was not found");
                break;

            case MainMenu.Task:
                Console.WriteLine("Enter Id:");
                int idT = int.Parse(Console.ReadLine()!);
                if (s_dal.Task!.Read(idT) != null)
                    Console.WriteLine(s_dal.Task!.Read(idT));
                else
                    throw new DalDoesNotExistsException($"Task with the ID={idT} was not found");
                break;
            default:
                break;

        }
    }

    /// <summary>
    /// A method for displaying the list of all objects of some entity received as a parameter
    /// </summary>
    /// <param name="entity"></param>
    static void readAllEntites(MainMenu entity)
    {
        switch (entity)
        {
            case MainMenu.Exit:
                return;

            case MainMenu.Engineer:
                foreach (var _engineer in s_dal.Engineer!.ReadAll())
                    Console.WriteLine(_engineer);
                break;

            case MainMenu.Dependency:
                foreach (var _dependency in s_dal.Dependency!.ReadAll())
                    Console.WriteLine(_dependency);
                break;

            case MainMenu.Task:
                foreach (var _task in s_dal.Task!.ReadAll())
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
    static void updateEntity(MainMenu entity)
    {
        switch (entity)
        {
            case MainMenu.Engineer:
                s_dal.Engineer!.Update(addEngineer());
                break;

            case MainMenu.Dependency:
                s_dal.Dependency!.Update(addDependency());
                break;
            case MainMenu.Task:

                s_dal.Task!.Update(addTask());
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
    static void deleteEntity(MainMenu entity)
    {
        switch (entity)
        {
            case MainMenu.Exit:
                return;
            case MainMenu.Engineer:
                Console.WriteLine("Enter Id:");
                int idE = int.Parse(Console.ReadLine()!);
                if (s_dal.Engineer!.Read(idE) != null)
                    s_dal.Engineer.Delete(idE);
                else
                    throw new DalDoesNotExistsException($"Engineer with the ID={idE} was not found");
                break;
            case MainMenu.Dependency:
                int idD = int.Parse(Console.ReadLine()!);
                if (s_dal.Dependency!.Read(idD) != null)
                    s_dal.Dependency.Delete(idD);
                else
                    throw new DalDoesNotExistsException($"Dependency with the ID={idD} was not found");
                break;
            case MainMenu.Task:
                int idT = int.Parse(Console.ReadLine()!);
                if (s_dal.Task!.Read(idT) != null)
                    s_dal.Task.Delete(idT);
                else
                    throw new DalDoesNotExistsException($"Task with the ID={idT} was not found");
                break;
            default:
                break;
        }
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

