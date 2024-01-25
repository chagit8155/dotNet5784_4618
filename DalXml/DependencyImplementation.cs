using DalApi;
using DO;
using System.Xml.Linq;

namespace Dal;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependences_xml = "dependencies";

    /// <summary>
    /// A function that creates a new object of type Task for a database
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Dependency item)
    {
        XElement? Delement = XMLTools.LoadListFromXMLElement(s_dependences_xml);
        int autoId = Config.NextDependencyId;
        XElement? ElementItem = new XElement("Dependency",
           new XElement("Id", autoId),
           new XElement("DependentTask", item.DependentTask),
           new XElement("DependsOnTask", item.DependsOnTask));
        Delement.Add(ElementItem);
        XMLTools.SaveListToXMLElement(Delement, s_dependences_xml);
        return item.Id;
    }

    /// <summary>
    /// A function for deleting an existing object with a certain ID, from the list of objects of type Task
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id)
    {
        XElement? Delement = XMLTools.LoadListFromXMLElement(s_dependences_xml);
        XElement? DependencyElementToDelete = Delement.Elements().FirstOrDefault(d => (int?)d.Element("Id") == id);
        if (DependencyElementToDelete == null)
            throw new DalDoesNotExistsException($"Dependency with such an ID={id} does not exist");
        else
            DependencyElementToDelete.Remove();
    }

    /// <summary>
    /// A method that deleted all the data of the entity
    /// </summary>
    public void DeleteAll()
    {
        XElement? Delement = XMLTools.LoadListFromXMLElement(s_dependences_xml);
        Delement.RemoveAll();
        XMLTools.SaveListToXMLElement(Delement, s_dependences_xml);
    }

    /// <summary>
    ///  The function returning a reference to an object of type Task with a certain ID, if it exists in a database, otherwise null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Dependency? Read(int id)
    {
        XElement? Delemement = XMLTools.LoadListFromXMLElement(s_dependences_xml).Elements().FirstOrDefault(d => (int?)d.Element("Id") == id);
        return Delemement is null ? null : getDependency(Delemement);
    }

    /// <summary>
    /// The method will run the Boolean function on the members of the list  and return the first object in the list on which the function returns True.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns> A pointer to a boolean function, a delegate of type Func </returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(s_dependences_xml).Elements().Select(d => getDependency(d)).FirstOrDefault(filter);
    }

    /// <summary>
    ///  Return the list of all objects in the list for which the function returns True. If no pointer is sent, the entire list will be returned
    /// </summary>
    /// <param name="filter"> A pointer to a boolean function, a delegate of type Func </param>
    /// <returns></returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        if (filter == null)
            return XMLTools.LoadListFromXMLElement(s_dependences_xml).Elements().Select(d => getDependency(d));
        else
            return XMLTools.LoadListFromXMLElement(s_dependences_xml).Elements().Select(d => getDependency(d)).Where(filter);
    }
    /// <summary>
    /// The update function of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with a new object with the same ID number and updated fields.
    /// </summary>
    /// <param name="item"></param>
    public void Update(Dependency item)
    {
        //delete


        XElement? Delement = XMLTools.LoadListFromXMLElement(s_dependences_xml);
        XElement? DependencyElementToDelete = Delement.Elements().FirstOrDefault(d => (int?)d.Element("Id") == item.Id);
        if (DependencyElementToDelete == null)
            throw new DalDoesNotExistsException($"Dependency with such an ID={item.Id} does not exist");
        else
        {
            //add
            DependencyElementToDelete.Remove();
            Delement.Add(item);
        }
        XMLTools.SaveListToXMLElement(Delement, s_dependences_xml);
    }

    private static Dependency getDependency(XElement e)
    {
        return new Dependency()
        {
            Id = e.ToIntNullable("Id") ?? throw new FormatException("can't  convert dependent task"),
            // Id = XMLTools.GetAndIncreaseNextId("data-config","Dependency"), /*?? throw new FormatException("can't  convert id"),*/
            DependentTask = e.ToIntNullable("DependentTask") ?? throw new FormatException("can't  convert dependent task"),
            DependsOnTask = e.ToIntNullable("DependsOnTask") ?? null
        };
    }
}
