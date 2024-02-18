namespace BO;
/// <summary>
///help entity of  Engineer In Task  :The ID and name of the engineer assigned to the task
/// </summary>
public class EngineerInTask
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public override string ToString() => this.ToStringProperty();
}
