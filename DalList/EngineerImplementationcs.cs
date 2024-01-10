namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class EngineerImplementationcs : IEngineer
{
    /// <summary>
    /// A function that creates a new object of type Engineer for a database
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public int Create(Engineer item)
    {
        //for entities with normal id (not auto id)
        if (Read(item.Id) is not null)
                throw new NotImplementedException($"An object of type Engineer with ID={item.Id} already exists");
        DataSource.Engineers.Add(item);
        return item.Id;
    }
    /// <summary>
    /// A function for deleting an existing object with a certain ID, from the list of objects of type Engineer
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Delete(int id)
    {
        //Entities that must not be deleted from the list ?!
        Engineer? engineerToDelete = DataSource.Engineers.Find(x => x.Id == id);
        if (engineerToDelete == null)
            throw new NotImplementedException($"An object of type Engineer with such an ID={id} does not exist");
        else
            DataSource.Engineers.Remove(engineerToDelete);
    }
    /// <summary>
    /// The function returning a reference to an object of type Engineer with a certain ID, if it exists in a database, otherwise null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Engineer? Read(int id)
    {
        foreach (var engineer in DataSource.Engineers)
        {
            if (id == engineer.Id)
                return engineer;
        }
        return null;
        // throw new NotImplementedException();
    }
    /// <summary>
    /// The function returned a copy of the list of references to all objects of type Engineer
    /// </summary>
    /// <returns></returns>
    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }
    /// <summary>
    /// The update function of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with a new object with the same ID number and updated fields.
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Update(Engineer item)
    {
        Engineer? itemToUpdate = DataSource.Engineers.Find(x => x.Id == item.Id);
        if (itemToUpdate == null)
        {
            throw new NotImplementedException($"An object of type Engineer with such an ID={item.Id} does not exist");
        }
        else
        {
            DataSource.Engineers.Remove(itemToUpdate);
            DataSource.Engineers.Add(item);
        }
    }
}
