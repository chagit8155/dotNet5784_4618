namespace DalApi;

public interface IDal
{
    public DateTime? StartProjectDate { get; set;}
    public DateTime? EndProjectDate { get; set;}
    IEngineer Engineer { get; }
    IDependency Dependency { get; }
    ITask Task { get; }

}
