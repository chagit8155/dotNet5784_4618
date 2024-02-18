namespace Dal;
using DalApi;
using DO;
internal class TaskImplementation : ITask
{
    /// <summary>
    /// A function that creates a new object of type Task for a database
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(DO.Task item)
    {
        //for entities with auto id
        int autoId = DataSource.Config.NextTaskId;
        DO.Task itemCopy = item with { Id = autoId };
        DataSource.Tasks.Add(itemCopy);
        return autoId;
    }

    /// <summary>
    /// A function for deleting an existing object with a certain ID, from the list of objects of type Task
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id)
    {
        //Entities that must not be deleted from the list ?!
        DO.Task? TaskToDelete = DataSource.Tasks.Find(x => x.Id == id);
        if (TaskToDelete == null)
            throw new DalDoesNotExistsException($"Task with such an ID={id} does not exist");
        else
            DataSource.Tasks.Remove(TaskToDelete);
    }
    /// <summary>
    /// A method that deleted all the data of the entity
    /// </summary>
    public void DeleteAll() 
    {
       // throw new NotImplementedException();
    }

    /// <summary>
    ///  The function returning a reference to an object of type Task with a certain ID, if it exists in a database, otherwise null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(item => item?.Id == id);
    }

    /// <summary>
    /// The method will run the Boolean function on the members of the list  and return the first object in the list on which the function returns True.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns> A pointer to a boolean function, a delegate of type Func </returns>
    public Task? Read(Func<Task, bool> filter)
    {
        return DataSource.Tasks.FirstOrDefault(item => filter(item));
    }
    /// <summary>
    ///  Return the list of all objects in the list for which the function returns True. If no pointer is sent, the entire list will be returned
    /// </summary>
    /// <param name="filter"> A pointer to a boolean function, a delegate of type Func </param>
    /// <returns></returns>
    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Tasks.Select(item => item);
        else
            return DataSource.Tasks.Where(filter);
    }

    /// <summary>
    /// The update function of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with a new object with the same ID number and updated fields.
    /// </summary>
    /// <param name="item"></param>
    public void Update(DO.Task item)
    {
        DO.Task? itemToUpdate = DataSource.Tasks.Find(x => x.Id == item.Id);
        if (itemToUpdate == null)
        {
            throw new DalDoesNotExistsException($"Task with such an ID={item.Id} does not exist");
        }
        else
        {
            DataSource.Tasks.Remove(itemToUpdate);
            DataSource.Tasks.Add(item);
        }
    }
}

