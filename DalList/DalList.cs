namespace Dal;
using DalApi;
sealed internal class DalList : IDal
{  
    private DalList() { }
    public static IDal Instance { get; } = new DalList();
    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementationcs();

    public ITask Task => new TaskImplementation();
  
    public DateTime? StartProjectDate
    {
        
        get { return DataSource.Config.StartProjectDate; }
        set { DataSource.Config.StartProjectDate = value; }
    }

    public DateTime? EndProjectDate
    {
        get { return DataSource.Config.EndProjectDate; }
        set { DataSource.Config.EndProjectDate = value; }
    }
  

}

