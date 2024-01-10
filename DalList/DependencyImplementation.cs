namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class DependencyImplementation : IDependency
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
            throw new NotImplementedException($"An object of type Dependency with such an ID={id} does not exist");
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

        foreach (var Dependency in DataSource.Dependences)
        {
            if (id == Dependency.Id)
                return Dependency;
        }
        return null;
        // throw new NotImplementedException();     
    }
    /// <summary>
    /// The function returned a copy of the list of references to all objects of type Task
    /// </summary>
    /// <returns></returns>
    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependences);
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
            throw new NotImplementedException($"An object of type Dependency with such an ID={item.Id} does not exist");
        }
        else
        {
            DataSource.Dependences.Remove(itemToUpdate);
            DataSource.Dependences.Add(item);
        }
    }
}
