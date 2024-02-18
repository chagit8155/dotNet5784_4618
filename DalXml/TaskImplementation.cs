namespace Dal;
using DalApi;
using System;
using System.Collections.Generic;
using System.Data.Common;

internal class TaskImplementation : ITask
{
    readonly string s_tasks_xml = "tasks";

    /// <summary>
    /// A function that creates a new object of type Task for a database
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(DO.Task item)
    {
        //for entities with auto id
        List<DO.Task> Tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        int autoId = Config.NextTaskId;
        DO.Task itemCopy = item with { Id = autoId };
        Tasks.Add(itemCopy);
        XMLTools.SaveListToXMLSerializer<DO.Task>(Tasks,s_tasks_xml);
        return autoId;
    }

    /// <summary>
    /// A function for deleting an existing object with a certain ID, from the list of objects of type Task
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id)
    {
        List<DO.Task> Tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        if (Tasks.RemoveAll(x => x.Id == id) == 0)
            throw new DO.DalDoesNotExistsException($"Task with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer<DO.Task>(Tasks, s_tasks_xml);
    }

    /// <summary>
    /// A method that deleted all the data of the entity
    /// </summary>
    public void DeleteAll()
    {
        List<DO.Task> Tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        Tasks.Clear();
        XMLTools.SaveListToXMLSerializer(Tasks, s_tasks_xml);
    }

    /// <summary>
    ///  The function returning a reference to an object of type Task with a certain ID, if it exists in a database, otherwise null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public DO.Task? Read(int id)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        return tasks.FirstOrDefault(x => x?.Id == id);
    }

    /// <summary>
    /// The method will run the Boolean function on the members of the list  and return the first object in the list on which the function returns True.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns> A pointer to a boolean function, a delegate of type Func </returns>
    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        List<DO.Task> Tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        return Tasks.FirstOrDefault(item => filter!(item));

    }

    /// <summary>
    ///  Return the list of all objects in the list for which the function returns True. If no pointer is sent, the entire list will be returned
    /// </summary>
    /// <param name="filter"> A pointer to a boolean function, a delegate of type Func </param>
    /// <returns></returns>
    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        List<DO.Task> Tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        if (filter == null)
            return Tasks.Select(item => item);
        else
            return Tasks.Where(filter);
    }

    /// <summary>
    /// The update function of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with a new object with the same ID number and updated fields.
    /// </summary>
    /// <param name="item"></param>
    public void Update(DO.Task item)
    {

        List<DO.Task> Tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        if (Tasks.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DO.DalDoesNotExistsException($"Task with ID={item.Id} does not exists");
        //add
        Tasks.Add(item);
        XMLTools.SaveListToXMLSerializer(Tasks, s_tasks_xml);
    }


  
}