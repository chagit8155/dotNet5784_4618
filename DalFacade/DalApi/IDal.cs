namespace DalApi;
using DO;
public interface IDal
{
    IEngineer Engineer { get; }
    IDependency Dependency { get; }
    ITask Task { get; }
}
