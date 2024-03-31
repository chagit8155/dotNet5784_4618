namespace BO;
public enum EngineerExperience
{
    Beginner,
    AdvancedBeginner,
    Intermediate,
    Advanced,
    Expert,
    None
};
public enum Status
{
    Unscheduled,
    Scheduled,
    OnTrack,
    InJeopredy,
    Done,
    None
};
public enum ProjectStatus
{
    Planing,
    MidLevel,
    Execution
};
public enum MainMenu
{
    Exit,
    Engineer,
    Task,
    Schedual
};
public enum EngineerSubMenu
{
    Exit,
    Create,
    Read,
    ReadAll,
    Update,
    Delete
};
public enum TaskSubMenu
{
    Exit,
    Create,
    Read,
    ReadAll,
    Update,
    Delete,
    AddDependency,
    CreateStartDate
};

public enum CreateScheduleOption 
{
    Automatically,
    Manually
};
public enum Time
{
    Hour,
    Day,
    Year
};
public enum User
{
    Manager,
    Engineer
};