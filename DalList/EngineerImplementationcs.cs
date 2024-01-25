namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class EngineerImplementationcs : IEngineer
{
    /// <summary>
    /// A function that creates a new object of type Engineer for a database
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(DO.Engineer item)
    {
        //for entities with normal id (not auto id)
        if (Read(item.Id) is not null)
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        DataSource.Engineers.Add(item);
        return item.Id;
    }
    /// <summary>
    /// A function for deleting an existing object with a certain ID, from the list of objects of type Engineer
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id)
    {
        //Entities that must not be deleted from the list ?!
        DO.Engineer? engineerToDelete = DataSource.Engineers.Find(x => x.Id == id);
        if (engineerToDelete == null)
            throw new DalDoesNotExistsException($"Engineer with such an ID={id} does not exist");
        else
            DataSource.Engineers.Remove(engineerToDelete);
    }
    /// <summary>
    /// A method that deleted all the data of the entity
    /// </summary>
    public void DeleteAll()
    {
      //  throw new NotImplementedException();
    }

    /// <summary>
    /// The function returning a reference to an object of type Engineer with a certain ID, if it exists in a database, otherwise null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public DO.Engineer? Read(int id)
    {
        return DataSource.Engineers.FirstOrDefault(item => item?.Id == id);
    }

    /// <summary>
    /// The method will run the Boolean function on the members of the list and return the first object in the list on which the function returns True.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns> A pointer to a boolean function, a delegate of type Func </returns>
    public DO.Engineer? Read(Func<Engineer, bool> filter)
    {
        return DataSource.Engineers.FirstOrDefault(item => filter(item));
    }

    /// <summary>
    ///  Return the list of all objects in the list for which the function returns True. If no pointer is sent, the entire list will be returned
    /// </summary>
    /// <param name="filter"> A pointer to a boolean function, a delegate of type Func </param>
    /// <returns></returns>
    public IEnumerable<DO.Engineer?> ReadAll(Func<DO.Engineer, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Engineers
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Engineers
               select item;
    }

    /// <summary>
    /// The update function of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with a new object with the same ID number and updated fields.
    /// </summary>
    /// <param name="item"></param>
    public void Update(DO.Engineer item)
    {
        DO.Engineer? itemToUpdate = DataSource.Engineers.Find(x => x.Id == item.Id);
        if (itemToUpdate == null)
        {
            throw new DalDoesNotExistsException($"Engineer with such an ID={item.Id} does not exist");
        }
        else
        {
            DataSource.Engineers.Remove(itemToUpdate);
            DataSource.Engineers.Add(item);
        }
    }
}
