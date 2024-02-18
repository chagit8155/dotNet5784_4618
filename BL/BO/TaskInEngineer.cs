namespace BO;
/// <summary>
///help entity of  Task In Engineer:  ID and alias of the current task
/// </summary>
public class TaskInEngineer
{
    public int Id { get; set; }
    public string? Alias { get; set; }
    public override string ToString() => this.ToStringProperty();
}
