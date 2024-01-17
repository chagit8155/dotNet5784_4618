namespace DalApi;
public interface ICrud<T> where T : class
{
    int Create(T item); //Creates new entity object in DAL
    T? Read(int id); //Reads entity object by its ID 
    T? Read(Func<T, bool> filter); //Reads entity object by A pointer to a boolean function and will return the first object in the list on which the function returns True. 
    IEnumerable<T?> ReadAll(Func<T, bool>? filter = null); //Reads all entity objects in the list for which the function returns True(or the entire list)
    void Update(T item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id
}