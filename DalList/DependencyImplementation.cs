namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class DependencyImplementation : IDependency
{
    /// <summary>
    /// A function that creates a new object of type Task for a database
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Dependency item)
    {
        //for entities with auto id
        int autoId = DataSource.Config.NextDependencyId;
        Dependency itemCopy = item with { Id = autoId };
        DataSource.Dependences.Add(itemCopy);
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
        Dependency? DependencyToDelete = DataSource.Dependences.Find(x => x.Id == id);
        if (DependencyToDelete == null)
            throw new DalDoesNotExistsException($"Dependency with such an ID={id} does not exist");
        else
            DataSource.Dependences.Remove(DependencyToDelete);

    }

    /// <summary>
    ///  The function returning a reference to an object of type Task with a certain ID, if it exists in a database, otherwise null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Dependency? Read(int id)
    {
        return DataSource.Dependences.FirstOrDefault(item => item?.Id == id);

    }
    /// <summary>
    /// The method will run the Boolean function on the members of the list  and return the first object in the list on which the function returns True.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns> A pointer to a boolean function, a delegate of type Func </returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return DataSource.Dependences.FirstOrDefault(item => filter(item));
    }

    /// <summary>
    ///  Return the list of all objects in the list for which the function returns True. If no pointer is sent, the entire list will be returned
    /// </summary>
    /// <param name="filter"> A pointer to a boolean function, a delegate of type Func </param>
    /// <returns></returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Dependences
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Dependences
               select item;
    }

    /// <summary>
    /// The update function of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with a new object with the same ID number and updated fields.
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Update(Dependency item)
    {
        Dependency? itemToUpdate = DataSource.Dependences.Find(x => x.Id == item.Id);
        if (itemToUpdate == null)
        {
            throw new DalDoesNotExistsException($"Dependency with such an ID={item.Id} does not exist");
        }
        else
        {
            DataSource.Dependences.Remove(itemToUpdate);
            DataSource.Dependences.Add(item);
        }
    }
}
