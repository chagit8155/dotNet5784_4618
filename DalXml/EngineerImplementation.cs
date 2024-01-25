using DalApi;
using DO;

namespace Dal;

internal class EngineerImplementation : IEngineer
{
    readonly string s_engineers_xml = "engineers";

    /// <summary>
    /// A function that creates a new object of type Engineer for a database
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Engineer item)
    {
        List<Engineer> Engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        if (Read(item.Id) is not null)    //for entities with normal id (not auto id)
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        Engineers.Add(item);
        XMLTools.SaveListToXMLSerializer<Engineer>(Engineers, s_engineers_xml);
        return item.Id;
    }

    /// <summary>
    /// A function for deleting an existing object with a certain ID, from the list of objects of type Engineer
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id)
    {
        List<Engineer> Engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        if (Engineers.RemoveAll(x => x.Id == id) == 0)
            throw new DalDoesNotExistsException($"Engineer with ID={id} does not exist");
        XMLTools.SaveListToXMLSerializer<Engineer>(Engineers, s_engineers_xml);
    }

    /// <summary>
    /// A method that deleted all the data of the entity
    /// </summary>
    public void DeleteAll()
    {
        List<Engineer> Engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        Engineers.Clear();
        XMLTools.SaveListToXMLSerializer(Engineers, s_engineers_xml);
    }

    /// <summary>
    /// The function returning a reference to an object of type Engineer with a certain ID, if it exists in a database, otherwise null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Engineer? Read(int id)
    {
        List<Engineer> Engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        return Engineers.FirstOrDefault(item => item?.Id == id);
    }

    /// <summary>
    /// The method will run the Boolean function on the members of the list and return the first object in the list on which the function returns True.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns> A pointer to a boolean function, a delegate of type Func </returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer> Engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        return Engineers.FirstOrDefault(item => filter(item));
    }

    /// <summary>
    ///  Return the list of all objects in the list for which the function returns True. If no pointer is sent, the entire list will be returned
    /// </summary>
    /// <param name="filter"> A pointer to a boolean function, a delegate of type Func </param>
    /// <returns></returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer> Engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        if (filter != null)
        {
            return from item in Engineers
                   where filter(item)
                   select item;
        }
        return from item in Engineers
               select item;
    }

    /// <summary>
    /// The update function of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with a new object with the same ID number and updated fields.
    /// </summary>
    /// <param name="item"></param>
    public void Update(Engineer item)
    {
        //delete
        List<Engineer> Engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        if (Engineers.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistsException($"Engineer with ID={item.Id} does not exists");
        //add
        Engineers.Add(item);
        XMLTools.SaveListToXMLSerializer<Engineer>(Engineers, s_engineers_xml);
    }
}
