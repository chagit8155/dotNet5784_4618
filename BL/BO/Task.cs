namespace BO;
/// <summary>
/// Main logical entity of a task - for a task details screen
/// </summary>
public class Task //Bo
{
    public int Id { get; init; } //Unique ID number (automatic runner number)
    public string? Alias { get; set; }
    public string? Description { get; set; }
    public BO.Status Status { get; set; }
    public List<TaskInList>? Dependencies { get; set; } //Dependencies list(task-in-list type) - relevant in the task creation phase
    public DateTime? CreatedAtDate { get; init; }//Task's creation date
    public TimeSpan? RequiredEffortTime { get; set; } //The amount of time required to perform the task
    public DateTime? StartDate { get; set; } //Actual begin date
    public DateTime? ScheduledDate { get; set; } //Planned date for work to begin
    public DateTime? ForecastDate { get; set; } //Estimated finish date - calculated field
    public DateTime? DeadlineDate { get; set; } //Possible final end date
    public DateTime? CompleteDate { get; set; } //Actual end date 
    public string? Deliverables { get; set; } // products
    public string? Remarks { get; set; }
    public EngineerInTask? Engineer { get; set; } //The ID and name of the engineer's assigned to the task
    public EngineerExperience Complexity { get; set; } //Task's difficulty level
    public override string ToString() => this.ToStringProperty();

}
