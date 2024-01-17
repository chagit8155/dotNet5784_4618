namespace DalTest;
using DalApi;
using DO;
using System;
/// <summary>
/// Access the data lists of each entity
/// </summary>
public static class Initialization
{
    //Defining a field for each interface to access the interface methods.
    private static IDal? s_dal; //stage 2
    private static readonly Random s_rand = new(); //Field for the random data draws

    /// <summary>
    /// The initialization method of an entity Engineers
    /// </summary>
    private static void CreateEngineers()
    {
        string[] engineerNames =
        {
        "Dani Levi", "Eli Amar", "Yair Cohen",
        "Ariela Levin", "Dina Klein", "Shira Israelof"
        };

        foreach (var _name in engineerNames)
        {
            int _id;
            double _cost;
            string? _email;
            EngineerExperience _level;
            do
                _id = s_rand.Next(200000000, 400000000);
            while (s_dal!.Engineer.Read(_id) != null);//Make sure that this fusion object does not already exist in the list

            _cost = s_rand.Next(100, 500);
            _level = (EngineerExperience)s_rand.Next(0, 4);
            _email = _name.Replace(" ", "") + "@gmail.com";
            Engineer newEngineer = new(_id, _name, _cost, _email, _level);
            s_dal!.Engineer.Create(newEngineer); //stage 2
        }
    }
    /// <summary>
    /// The initialization method of an entity Dependency
    /// </summary>
    private static void CreateDependences()
    {
        //there is 20 tasks 
        int _dependentTask, _dependsOnTask;
        for (int i = 0; i < 38; i++)
        {

            _dependentTask = s_rand.Next(1, 18);
            _dependsOnTask = s_rand.Next(1, 18);
            Dependency newDepn = new() { DependentTask = _dependentTask, DependsOnTask = _dependsOnTask };
            s_dal!.Dependency.Create(newDepn); //stage 2
        }

        Dependency newDep = new() { DependentTask = 19, DependsOnTask = 5 }; //There must be at least two tasks whose dependent lists are the same.
        s_dal!.Dependency.Create(newDep); //stage 2
        newDep = new Dependency() { DependentTask = 20, DependsOnTask = 5 };
        s_dal!.Dependency.Create(newDep); //stage 2
    }

    /// <summary>
    /// The initialization method of an entity Task
    /// </summary>
    private static void CreateTasks()
    {
        string[] taskAlias =
            {
                "UI/UX Planning and Development",
                "CRUD for Tasks",
                "Project Management",
                "Task Scheduler",
                "Contribution and Collaboration",
                "Profile Management",
                "Task Assignment",
                "Document Management",
                "Daily Monitoring",
                "Video Calls",
                "Notifications and Reminders",
                "User Authentication",
                "Automation",
                "Performance Reports",
                "Public API",
                "Version Control",
                "User Verification",
                "Technological Upgrade",
                "Code Stability",
                "Bug Tracking",
                "Risk Analysis"
              };

        string[] taskDescriptions =
          {
                "User Interface Development: Designing and developing a user-friendly and modern user interface."
                ,"Task Management System Development: Creating a mechanism for adding, updating, and deleting tasks."
                ,"Project Tracking: Displaying status reports and progress by projects."
                ,"Scheduler: Creating a scheduling system with reminders for important tasks."
                ,"Contribution Mechanisms Development: Developing a feature to add groups and contribute to projects."
                ,"Profile Management: Creating and managing personal profiles for users."
                ,"Task Assignment: Assigning tasks to users based on skills and profiles."
                ,"Document Management: Managing and sharing documents within the system."
                ,"Daily Activity Monitoring: Generating daily reports and monitoring daily activity."
                ,"Online Video Calls: Integrating video calls for face-to-face communication."
                ,"Task Notifications System: Implementing a notification system for important tasks."
                ,"User Authentication: Adding an authentication mechanism and access control for users."
                ,"Automation in Task Management: Developing automation for routine tasks."
                ,"Analysis Reports: Generating business analysis reports and statistics."
                ,"REST API Development: Creating a public REST API for external usage and integration."
                ,"Version Control: Developing a system to manage and preserve versions of the source code."
                ,"User Verification:: Adding a verification mechanism and user access management."
                ,"Technological Upgrade: Checking and upgrading key technologies and libraries.."
                ,"Technological Upgrade: Checking and upgrading key technologies and libraries."
                ,"Bug Tracking: Implementing a bug tracking and error management system."
                ,"The project's risk analysis"
        };
        DateTime startDate = new(2024, 1, 15);
        for (int i = 0; i < taskAlias.Length; i++)
        {
            EngineerExperience _copmlexity = (EngineerExperience)s_rand.Next(0, 4);
            int day = s_rand.Next(3, 21);
            // int month = s_rand.Next(1, 12);
            DateTime _creatAtDate = startDate.AddDays(day);
            // DateTime _scheduledDate = startDate.AddMonths(month);
            Task newTa = new() { Alias = taskAlias[i], Description = taskDescriptions[i], CreatedAtDate = _creatAtDate, /*ScheduledDate = _scheduledDate,*/  Copmlexity = _copmlexity };
            s_dal!.Task.Create(newTa); //stage 2
        }
    }
    //.... להוסיף עוד לבנאי?
    /// <summary>
    /// A public method, which will schedule the private methods we prepared and trigger the initialization of the lists
    /// </summary>
    /// <param name="dalEngineer"></param>
    /// <param name="dalDependency"></param>
    /// <param name="dalTask"></param>
    /// <exception cref="NullReferenceException"></exception>
    public static void Do(IDal dal)
    {
        s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2
        //Calling all private methods for initializing all lists
        CreateEngineers();
        CreateDependences();
        CreateTasks();
    }
}
