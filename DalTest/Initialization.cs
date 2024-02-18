namespace DalTest;
using Dal;
using DalApi;
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
        //int _dependentTask, _dependsOnTask;
        //for (int i = 0; i < 38; i++)
        //{
        //    _dependentTask = s_rand.Next(1, 18);
        //    _dependsOnTask = s_rand.Next(1, 18);
        //    Dependency newDepn = new() { DependentTask = _dependentTask, DependsOnTask = _dependsOnTask };
        //    s_dal!.Dependency.Create(newDepn); // stage 2
        //}
        //Dependency newDep = new() { DependentTask = 19, DependsOnTask = 5 }; //There must be at least two tasks whose dependent lists are the same.
        //s_dal!.Dependency.Create(newDep); //stage 2
        //newDep = new Dependency() { DependentTask = 20, DependsOnTask = 5 };
        //s_dal!.Dependency.Create(newDep); //stage 2
   //     s_dal!.Dependency.Create(new DO.Dependency(0, 1, 2)); // User Interface Development depends on Task Management System Development
     //   s_dal!.Dependency.Create(new DO.Dependency(0, 1, 3)); // User Interface Development depends on Project Tracking
       // s_dal!.Dependency.Create(new DO.Dependency(0, 1, 4)); // User Interface Development depends on Scheduler
        s_dal!.Dependency.Create(new DO.Dependency(0, 2, 7)); // Task Management System Development depends on Task Assignment
        s_dal!.Dependency.Create(new DO.Dependency(0, 2, 8)); // Task Management System Development depends on Document Management
        s_dal!.Dependency.Create(new DO.Dependency(0, 2, 11)); // Task Management System Development depends on Task Notifications System
        s_dal!.Dependency.Create(new DO.Dependency(0, 3, 15)); // Project Tracking depends on REST API Development
        s_dal!.Dependency.Create(new DO.Dependency(0, 4, 9)); // Scheduler depends on Daily Activity Monitoring
        s_dal!.Dependency.Create(new DO.Dependency(0, 5, 7)); // Contribution Mechanisms Development depends on Task Assignment
        s_dal!.Dependency.Create(new DO.Dependency(0, 6, 5)); // Profile Management depends on Contribution Mechanisms Development
        s_dal!.Dependency.Create(new DO.Dependency(0, 7, 2)); // Task Assignment depends on Task Management System Development
        s_dal!.Dependency.Create(new DO.Dependency(0, 7, 6)); // Task Assignment depends on Profile Management
        s_dal!.Dependency.Create(new DO.Dependency(0, 7, 10)); // Task Assignment depends on Online Video Calls
        s_dal!.Dependency.Create(new DO.Dependency(0, 8, 9)); // Document Management depends on Daily Activity Monitoring
        s_dal!.Dependency.Create(new DO.Dependency(0, 8, 10)); // Document Management depends on Online Video Calls
        s_dal!.Dependency.Create(new DO.Dependency(0, 9, 8)); // Daily Activity Monitoring depends on Document Management
        //s_dal!.Dependency.Create(new DO.Dependency(0, 10, 1)); // Online Video Calls depends on User Interface Development
        s_dal!.Dependency.Create(new DO.Dependency(0, 11, 2)); // Task Notifications System depends on Task Management System Development
        s_dal!.Dependency.Create(new DO.Dependency(0, 11, 12)); // Task Notifications System depends on User Authentication
        s_dal!.Dependency.Create(new DO.Dependency(0, 12, 18)); // User Authentication depends on Technological Upgrade
        s_dal!.Dependency.Create(new DO.Dependency(0, 13, 2)); // Automation in Task Management depends on Task Management System Development
        s_dal!.Dependency.Create(new DO.Dependency(0, 13, 4)); // Automation in Task Management depends on Scheduler
        s_dal!.Dependency.Create(new DO.Dependency(0, 14, 13)); // Analysis Reports depends on Automation in Task Management
        s_dal!.Dependency.Create(new DO.Dependency(0, 14, 15)); // Analysis Reports depends on REST API Development
        s_dal!.Dependency.Create(new DO.Dependency(0, 15, 16)); // REST API Development depends on Version Control
        s_dal!.Dependency.Create(new DO.Dependency(0, 16, 18)); // Version Control depends on Technological Upgrade
        s_dal!.Dependency.Create(new DO.Dependency(0, 17, 12)); // User Verification depends on User Authentication
        s_dal!.Dependency.Create(new DO.Dependency(0, 18, 17)); // Technological Upgrade depends on User Verification
        s_dal!.Dependency.Create(new DO.Dependency(0, 19, 16)); // Bug Tracking depends on Version Control
        s_dal!.Dependency.Create(new DO.Dependency(0, 20, 19)); // Project's Risk Analysis depends on Bug Tracking
        s_dal!.Dependency.Create(new DO.Dependency(0, 20, 14)); // Project's Risk Analysis depends on Analysis Reports
        s_dal!.Dependency.Create(new DO.Dependency(0, 20, 13)); // Project's Risk Analysis depends on Automation in Task Management
        s_dal!.Dependency.Create(new DO.Dependency(0, 3, 2)); // Project Tracking depends on Task Management System Development
        s_dal!.Dependency.Create(new DO.Dependency(0, 4, 2)); // Scheduler depends on Task Management System Development
        s_dal!.Dependency.Create(new DO.Dependency(0, 5, 3)); // Contribution Mechanisms Development depends on Project Tracking
        //s_dal!.Dependency.Create(new DO.Dependency(0, 6, 1)); // Profile Management depends on User Interface Development
  //      s_dal!.Dependency.Create(new DO.Dependency(0, 7, 1)); // Task Assignment depends on User Interface Development
        s_dal!.Dependency.Create(new DO.Dependency(0, 8, 2)); // Document Management depends on Task Management System Development
        s_dal!.Dependency.Create(new DO.Dependency(0, 9, 10)); // Daily Activity Monitoring depends on Online Video Calls
        s_dal!.Dependency.Create(new DO.Dependency(0, 10, 7)); // Online Video Calls depends on Task Assignment
        s_dal!.Dependency.Create(new DO.Dependency(0, 11, 9)); // Task Notifications System depends on Daily Activity Monitoring
        s_dal!.Dependency.Create(new DO.Dependency(0, 12, 2)); // User Authentication depends on Task Management System Development
        s_dal!.Dependency.Create(new DO.Dependency(0, 13, 7)); // Automation in Task Management depends on Task Assignment
        s_dal!.Dependency.Create(new DO.Dependency(0, 14, 2)); // Analysis Reports depends on Task Management System Development
        //s_dal!.Dependency.Create(new DO.Dependency(0, 15, 1)); // REST API Development depends on User Interface Development
        s_dal!.Dependency.Create(new DO.Dependency(0, 16, 15)); // Version Control depends on REST API Development
        s_dal!.Dependency.Create(new DO.Dependency(0, 17, 12)); // User Verification depends on User Authentication
        s_dal!.Dependency.Create(new DO.Dependency(0, 18, 15)); // Technological Upgrade depends on REST API Development
        s_dal!.Dependency.Create(new DO.Dependency(0, 19, 16)); // Bug Tracking depends on Version Control
        s_dal!.Dependency.Create(new DO.Dependency(0, 20, 15)); // Project's Risk Analysis depends on REST API Development
        s_dal!.Dependency.Create(new DO.Dependency(0, 14, 13)); // Analysis Reports depends on Automation in Task Management

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
}
