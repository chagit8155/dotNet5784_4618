namespace BlApi;

public interface ITask
{
    public int Create(BO.Task item);  
    public void CreateStartDate(int id, DateTime date);
    public void Delete(int id);
    public BO.Task Read(int id);
    public BO.Task Read(Func<BO.Task, bool>? filter = null);
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null);

    public void Update(BO.Task item);
    public void AddDependency(int dependencyTask ,int dependencyOnTask );
    public bool AreAllPreviousTasksCompleted(int id);
    public bool isInJeoprady(int id);
}
