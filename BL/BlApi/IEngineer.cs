namespace BlApi;
public interface IEngineer
{
    public int Create(BO.Engineer item);
    public void Delete(int id);
    public BO.Engineer Read(int id);
    public BO.Engineer Read(Func<BO.Engineer, bool>? filter = null);
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null);

    public void Update(BO.Engineer item);
}
