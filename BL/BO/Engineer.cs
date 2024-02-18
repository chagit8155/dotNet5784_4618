namespace BO;
/// <summary>
///  Main Logical Entity of an Engineer - For Engineer List and Engineer Details screens
/// </summary>
public class Engineer
{
    public int Id { get; init; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public EngineerExperience Level { get; set; }
    public double? Cost { get; set; }
    public TaskInEngineer? Task { get; set; } //ID and alias of the current task
    public override string ToString() => this.ToStringProperty();
}
