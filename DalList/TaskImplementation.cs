namespace Dal;
using DalApi;
using DO;
public class TaskImplementation : ITask
{
    /// <summary>
    /// A function that creates a new object of type Task for a database
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Task item)
    {
        //for entities with auto id
        int autoId = DataSource.Config.NextTaskId;
        Task itemCopy = item with { Id = autoId };
        DataSource.Tasks.Add(itemCopy);
        return autoId;
    }
    /// <summary>
    /// A function for deleting an existing object with a certain ID, from the list of objects of type Task
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Delete(int id)
    {
        //Entities that must not be deleted from the list ?!
        Task? TaskToDelete = DataSource.Tasks.Find(x => x.Id == id);
        if (TaskToDelete == null)
            throw new NotImplementedException($"An object of type Task with such an ID={id} does not exist");
        else
            DataSource.Tasks.Remove(TaskToDelete);
    }
    /// <summary>
    ///  The function returning a reference to an object of type Task with a certain ID, if it exists in a database, otherwise null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task? Read(int id)
    {
        foreach (var Task in DataSource.Tasks)
        {
            if (id == Task.Id)
                return Task;
        }
        return null;
        // throw new NotImplementedException();
    }
    /// <summary>
    /// The function returned a copy of the list of references to all objects of type Task
    /// </summary>
    /// <returns></returns>
    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);

    }
    /// <summary>
    /// The update function of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with a new object with the same ID number and updated fields.
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Update(Task item)
    {
        Task? itemToUpdate = DataSource.Tasks.Find(x => x.Id == item.Id);
        if (itemToUpdate == null)
        {
            throw new NotImplementedException($"An object of type Task with such an ID={item.Id} does not exist");
        }
        else
        {
            DataSource.Tasks.Remove(itemToUpdate);
            DataSource.Tasks.Add(item);
        }
    }
}

