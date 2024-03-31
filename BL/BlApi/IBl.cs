using BO;

namespace BlApi;
public interface IBl
{
    public IEngineer Engineer { get; }
    public ITask Task { get; }
    public DateTime? StartProjectDate { get; set; }
   // public DateTime? EndProjectDate { get; set; } //
    public BO.ProjectStatus GetProjectStatus();
    public void CreateSchedule(DateTime date, BO.CreateScheduleOption option = BO.CreateScheduleOption.Automatically, int taskId = -1);

    #region Clock
    public DateTime Clock { get; }
    public void PromoteTime(Time addTime);
    public void ResetClock();
    #endregion

    public void InitializeDB();
    public void ResetDB();
}

