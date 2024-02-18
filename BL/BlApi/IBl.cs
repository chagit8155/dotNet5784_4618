namespace BlApi;
public interface IBl
{
    public IEngineer Engineer { get; }
    public ITask Task { get; }
    public DateTime? StartProjectDate { get; set; }
   // public DateTime? EndProjectDate { get; set; } //
    public BO.ProjectStatus GetProjectStatus();
    public void CreateSchedule(BO.CreateScheduleOption option = BO.CreateScheduleOption.Automatically, int taskId = -1);
}

