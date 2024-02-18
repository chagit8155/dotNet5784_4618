namespace BO;
/// <summary>
/// Dependencies list(task-in-list type) - relevant in the task creation phase
/// </summary>
public class TaskInList
{
    public int Id { get; init; }
    public string? Alias { get; set; }
    public string? Description { get; set; }
    public BO.Status Status { get; set; }
    public override string ToString() => this.ToStringProperty();
}
