namespace DalTest;
using Dal;
using DalApi;
using DO;
using System;
using System.Xml.Linq;

/// <summary>
/// Access the data of each entity
/// </summary>
public static class Initialization
{
    //Defining a field for interface to access the interface methods.
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
            DO.EngineerExperience _level;
            do
                _id = s_rand.Next(200000000, 400000000);
            while (s_dal!.Engineer.Read(_id) != null); // Make sure that this fusion object does not already exist in the list

            _cost = s_rand.Next(100, 500);
            _level = (DO.EngineerExperience)s_rand.Next(0, 4);
            _email = _name.Replace(" ", "") + "@gmail.com";
            DO.Engineer newEngineer = new(_id, _name, _cost, _email, _level);
            s_dal!.Engineer.Create(newEngineer); // stage 2
        }
    }
    /// <summary>
    /// The initialization method of an entity Dependency
    /// </summary>
    private static void CreateDependencies()
    {
        ////there is 20 tasks 
        s_dal!.Dependency.Create(new Dependency(0, 1, 2));
        s_dal!.Dependency.Create(new Dependency(0, 1, 3));
        s_dal!.Dependency.Create(new Dependency(0, 1, 4));
        s_dal!.Dependency.Create(new Dependency(0, 1, 5));
        s_dal!.Dependency.Create(new Dependency(0, 2, 3));
        s_dal!.Dependency.Create(new Dependency(0, 2, 4));
        s_dal!.Dependency.Create(new Dependency(0, 2, 5));
        s_dal!.Dependency.Create(new Dependency(0, 3, 4));
        s_dal!.Dependency.Create(new Dependency(0, 3, 5));
        s_dal!.Dependency.Create(new Dependency(0, 3, 6));
        s_dal!.Dependency.Create(new Dependency(0, 4, 5));
        s_dal!.Dependency.Create(new Dependency(0, 4, 6));
        s_dal!.Dependency.Create(new Dependency(0, 4, 7));
        s_dal!.Dependency.Create(new Dependency(0, 5, 6));
        s_dal!.Dependency.Create(new Dependency(0, 5, 7));
        s_dal!.Dependency.Create(new Dependency(0, 5, 8));
        s_dal!.Dependency.Create(new Dependency(0, 6, 7));
        s_dal!.Dependency.Create(new Dependency(0, 6, 8));
        s_dal!.Dependency.Create(new Dependency(0, 6, 9));
        s_dal!.Dependency.Create(new Dependency(0, 7, 8));
        s_dal!.Dependency.Create(new Dependency(0, 7, 9));
        s_dal!.Dependency.Create(new Dependency(0, 7, 10));
        s_dal!.Dependency.Create(new Dependency(0, 8, 9));
        s_dal!.Dependency.Create(new Dependency(0, 8, 10));
        s_dal!.Dependency.Create(new Dependency(0, 8, 11));
        s_dal!.Dependency.Create(new Dependency(0, 9, 10));
        s_dal!.Dependency.Create(new Dependency(0, 9, 11));
        s_dal!.Dependency.Create(new Dependency(0, 9, 12));
        s_dal!.Dependency.Create(new Dependency(0, 10, 11));
        s_dal!.Dependency.Create(new Dependency(0, 10, 12));
        s_dal!.Dependency.Create(new Dependency(0, 10, 13));
        s_dal!.Dependency.Create(new Dependency(0, 11, 12));
        s_dal!.Dependency.Create(new Dependency(0, 11, 13));
        s_dal!.Dependency.Create(new Dependency(0, 12, 13));
        s_dal!.Dependency.Create(new Dependency(0, 17, 19));
        s_dal!.Dependency.Create(new Dependency(0, 17, 20));
        s_dal!.Dependency.Create(new Dependency(0, 1, 17));
        s_dal!.Dependency.Create(new Dependency(0, 18, 19));
        s_dal!.Dependency.Create(new Dependency(0, 18, 20));
        s_dal!.Dependency.Create(new Dependency(0, 1, 20));

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
            DO.EngineerExperience _copmlexity = (DO.EngineerExperience)s_rand.Next(0, 4);
            int day = s_rand.Next(3, 21);
            DateTime _creatAtDate = startDate.AddDays(day);
            TimeSpan _requiredEffortTime = new TimeSpan(s_rand.Next(1, 4), 0, 0, 0);
            DO.Task newTa = new() { Alias = taskAlias[i], Description = taskDescriptions[i], CreatedAtDate = _creatAtDate, /*ScheduledDate = _scheduledDate,*/  Copmlexity = _copmlexity, RequiredEffortTime = _requiredEffortTime };
            s_dal!.Task.Create(newTa); //stage 2
        }

    }

    /// <summary>//Calling all private methods for initializing all lists
    /// A public method, which will schedule the private methods we prepared and trigger the initialization of the lists
    /// </summary>
    public static void Do()
    {
        //s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2
        s_dal = DalApi.Factory.Get; //stage 4
        XElement? item = new XElement("config",
           new XElement("NextDependencyId", 1),
           new XElement("NextTaskId", 1),
           new XElement("StartProjectDate", ""),
           new XElement("EndProjectDate", ""));
        XMLTools.SaveListToXMLElement(item, "data-config");
        CreateEngineers();
        CreateDependencies();
        CreateTasks();
    }
    public static void Reset()
    {
        s_dal = DalApi.Factory.Get;
        s_dal.Engineer.DeleteAll();
        s_dal.Task.DeleteAll();
        s_dal.Dependency.DeleteAll();
    }
}
