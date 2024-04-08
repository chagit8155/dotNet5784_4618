namespace BlImplementation;
using BlApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

internal class EngineerImplementation : IEngineer
{
    private readonly IBl bl;
    internal EngineerImplementation(IBl bl) => this.bl = bl;
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private BlApi.IBl _bl = BlApi.Factory.Get();
   
    /// <summary>
    /// Creates a new engineer with the specified details.
    /// </summary>
    /// <param name="boEngineer">The engineer object containing the details of the new engineer.</param>
    /// <returns>The ID of the newly created engineer.</returns>
    /// <exception cref="BO.BlInvalidInputFormatException">Thrown when the provided data is invalid.</exception>
    /// <exception cref="BO.BlAlreadyExistsException">Thrown when an engineer with the same ID already exists.</exception>
    public int Create(BO.Engineer boEngineer)
    {
        if (!isValidID(boEngineer.Id) || !isValidString(boEngineer.Name) || !isValidCost(boEngineer.Cost) || !isValidEmail(boEngineer.Email))
            throw new BO.BlInvalidInputFormatException("Wrong data");

        DO.Engineer doEngineer = new DO.Engineer(boEngineer.Id, boEngineer.Name, boEngineer.Cost, boEngineer.Email, (DO.EngineerExperience)boEngineer.Level);

        try
        {
            int idEngineer = _dal.Engineer.Create(doEngineer);
            return idEngineer;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={boEngineer.Id} already exists", ex);
        }
    }

    /// <summary>
    /// Deletes the engineer with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the engineer to delete.</param>
    /// <exception cref="BO.BlDeletionImpossibleException">Thrown when the engineer cannot be deleted because they are performing a task.</exception>
    /// <exception cref="BO.BlDoesNotExistException">Thrown when the engineer with the specified ID does not exist.</exception>
    public void Delete(int id)
    {
        if (getTaskInEngineer(id) != null)
            throw new BO.BlDeletionImpossibleException($"Engineer with ID={id} cannot be deleted because they are performing a task");

        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does not exist", ex);
        }
    }

    /// <summary>
    /// Retrieves an engineer based on the specified filter condition.
    /// </summary>
    /// <param name="filter">An optional filter condition to apply to the engineers.</param>
    /// <returns>The engineer matching the specified filter condition.</returns>
    /// <exception cref="BO.BlDoesNotExistException">Thrown when no engineer matches the specified filter condition.</exception>
    public BO.Engineer Read(Func<BO.Engineer, bool>? filter = null)
    {
        if (filter == null)
            return ReadAll().FirstOrDefault() ?? throw new BO.BlDoesNotExistException("There are no engineers in the data");
        else
            return ReadAll().FirstOrDefault(filter) ?? throw new BO.BlDoesNotExistException($"Engineer matching the specified filter was not found");
    }

    /// <summary>
    /// Retrieves the details of an engineer by ID.
    /// </summary>
    /// <param name="id">The ID of the engineer to retrieve.</param>
    /// <returns>The engineer object with the specified ID.</returns>
    /// <exception cref="BO.BlDoesNotExistException">Thrown when the engineer with the specified ID does not exist.</exception>
    public BO.Engineer Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does not exist");
        return convertDOtoBO(doEngineer);
    }

    /// <summary>
    /// Retrieves a list of engineers, optionally filtered by a specified condition.
    /// </summary>
    /// <param name="filter">An optional filter condition to apply to the engineers.</param>
    /// <returns>The list of engineers matching the specified filter condition.</returns>
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        var engineers = from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                        select new BO.Engineer
                        {
                            Id = doEngineer.Id,
                            Name = doEngineer.Name,
                            Email = doEngineer.Email,
                            Level = (BO.EngineerExperience)doEngineer.Level,
                            Cost = doEngineer.Cost,
                            Task = getTaskInEngineer(doEngineer.Id)
                        };
        if (filter != null)
            return engineers.Where(filter);
        return engineers;
    }


    /// <summary>
    /// Updates the details of an engineer.
    /// </summary>
    /// <param name="boEngineer">The engineer object containing the updated details.</param>
    /// <exception cref="BO.BlDoesNotExistException">Thrown when the engineer to be updated does not exist.</exception>
    /// <exception cref="BO.BlInvalidInputFormatException">Thrown when the provided data is invalid.</exception>
    public void Update(BO.Engineer boEngineer)
    {
        DO.Engineer doEngineer = _dal.Engineer.Read(boEngineer.Id) ?? throw new BO.BlDoesNotExistException($"Engineer with ID={boEngineer.Id} does not exist");

        // Validate input data
        //if (boEngineer.Name == null || !isValidCost(boEngineer.Cost) || !isValidEmail(boEngineer.Email) /*|| (DO.EngineerExperience)boEngineer.Level > doEngineer.Level*/)
        //    throw new BO.BlInvalidInputFormatException("Wrong input");

        BO.Engineer engineerBeforeUpdate = Read(boEngineer.Id);

        // Perform task-related updates
        if (engineerBeforeUpdate.Task is not null && boEngineer.Task is null)
        {
            finishTask(engineerBeforeUpdate.Task.Id);
        }
        if (engineerBeforeUpdate.Task is null && boEngineer.Task is not null)
        {
            if (isValidTask(boEngineer.Task.Id))
                throw new BO.BlInvalidInputFormatException("Wrong data");
            assignEngineer(boEngineer.Task.Id, boEngineer.Id);
        }
        if (engineerBeforeUpdate.Task is not null && boEngineer.Task is not null)
        {
            finishTask(engineerBeforeUpdate.Task.Id);
            assignEngineer(boEngineer.Task.Id, boEngineer.Id);
        }

        // Update the engineer
        try
        {
            DO.Engineer updateEngineer = new DO.Engineer(boEngineer.Id, boEngineer.Name, boEngineer.Cost, boEngineer.Email, (DO.EngineerExperience)boEngineer.Level);
            _dal.Engineer.Update(updateEngineer);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={boEngineer.Id} does not exist", ex);
        }
    }


    /// <summary>
    /// Assigns an engineer to a task.
    /// </summary>
    /// <param name="taskID">The ID of the task to assign the engineer to.</param>
    /// <param name="idEngineer">The ID of the engineer to assign.</param>
    /// <exception cref="BO.BlTheProjectTimeDoesNotAllowException">Thrown when trying to assign an engineer at the execution stage of the project.</exception>
    /// <exception cref="BO.BlDoesNotExistException">Thrown when either the task or the engineer does not exist.</exception>
    /// <exception cref="BO.BlCannotBeUpdatedException">Thrown when the assignment cannot be performed due to various reasons (e.g., engineer already working on a task, task's complexity exceeds engineer's level).</exception>
    private void assignEngineer(int taskID, int idEngineer)
    {
        if (_bl.GetProjectStatus() == BO.ProjectStatus.Execution)
            throw new BO.BlTheProjectTimeDoesNotAllowException("Impossible action at this stage of the project - The project start date has already been set");

        DO.Task task = _dal.Task.Read(taskID) ?? throw new BO.BlDoesNotExistException($"Task with ID={taskID} does not exist");

        if (isEngineerOnTrak(idEngineer))
            throw new BO.BlCannotBeUpdatedException("The engineer is already assigned to a task");

        if ((int)task.Copmlexity > (int)_dal.Engineer.Read(idEngineer)!.Level)
            throw new BO.BlCannotBeUpdatedException("The task's complexity exceeds the engineer's level");

        if (task.EngineerId == null)
            _dal.Task.Update(task with { EngineerId = idEngineer, StartDate = DateTime.Now });
        else
            throw new BO.BlCannotBeUpdatedException("Another engineer is already assigned to the task");
    }

    /// <summary>
    /// Marks a task as finished.
    /// </summary>
    /// <param name="id">The ID of the task to mark as finished.</param>
    private void finishTask(int id)
    {
        _dal.Task.Update(_dal.Task.Read(id)! with { CompleteDate = bl.Clock, EngineerId = null });
        var t = bl.Task.Read(id);
        t.Status = BO.Status.Done;

    }

    /// <summary>
    /// Retrieves a task assigned to the engineer with the specified ID.
    /// </summary>
    /// <param name="_id">The ID of the engineer.</param>
    /// <returns>A task assigned to the engineer, or null if no task is assigned.</returns>
    public BO.TaskInEngineer? getTaskInEngineer(int _id)
    {
        var taskInEngineer = _dal.Task
            .ReadAll()
            .FirstOrDefault(item => item != null && item.EngineerId == _id);

        return taskInEngineer != null ? new BO.TaskInEngineer { Id = taskInEngineer.Id, Alias = taskInEngineer.Alias } : null;
    }

    /// <summary>
    /// Checks whether the provided email address is valid.
    /// </summary>
    /// <param name="_email">The email address to validate.</param>
    /// <returns>True if the email address is valid; otherwise, false.</returns>
    private bool isValidEmail(string? _email)
    {
        return new EmailAddressAttribute().IsValid(_email);
    }

    /// <summary>
    /// Checks whether the provided ID is valid (non-negative).
    /// </summary>
    /// <param name="_id">The ID to validate.</param>
    /// <returns>True if the ID is valid; otherwise, false.</returns>
    private bool isValidID(int _id)
    {
        return _id >= 0;
    }

    /// <summary>
    /// Checks whether the provided cost is valid (positive).
    /// </summary>
    /// <param name="_cost">The cost to validate.</param>
    /// <returns>True if the cost is valid; otherwise, false.</returns>
    private bool isValidCost(double? _cost)
    {
        return _cost > 0;
    }

    /// <summary>
    /// Checks whether the provided string is valid (not empty).
    /// </summary>
    /// <param name="_string">The string to validate.</param>
    /// <returns>True if the string is valid; otherwise, false.</returns>
    private bool isValidString(string? _string)
    {
        return _string != "";
    }

    /// <summary>
    /// Checks whether a task with the provided ID already exists.
    /// </summary>
    /// <param name="_id">The ID of the task to check.</param>
    /// <returns>True if no task with the provided ID exists; otherwise, false.</returns>
    private bool isValidTask(int _id)
    {
        return _dal.Task.Read(_id) == null;
    }

    /// <summary>
    /// Checks whether the engineer with the provided ID is working on another task at the same time.
    /// </summary>
    /// <param name="engineerID">The ID of the engineer to check.</param>
    /// <returns>True if the engineer is not working on another task at the same time; otherwise, false.</returns>
    private bool isEngineerOnTrak(int engineerID)
    {
        return _dal.Task.ReadAll(item => item.EngineerId == engineerID) == null;
    }


    /// <summary>
    /// Converts a data object representing an engineer to a business object representing an engineer.
    /// </summary>
    /// <param name="item">The data object representing the engineer to convert.</param>
    /// <returns>A business object representing the engineer.</returns>
    public BO.Engineer convertDOtoBO(DO.Engineer item)
    {
        return new BO.Engineer
        {
            Id = item.Id,
            Name = item?.Name,
            Email = item?.Email,
            Level = (BO.EngineerExperience)item!.Level,
            Cost = item.Cost,
            Task = getTaskInEngineer(item.Id)
        };
    }

    /// <summary>
    /// Retrieves engineers from a specified experience level or higher.
    /// </summary>
    /// <param name="level">The minimum experience level of the engineers to retrieve.</param>
    /// <returns>An enumerable collection of engineers with the specified experience level or higher.</returns>
    public IEnumerable<BO.Engineer> EngineersFromLevel(BO.EngineerExperience level)
    {
        var engineerGroup = from BO.Engineer boEngineer in ReadAll()
                       group boEngineer by boEngineer.Level into g
                       select g;

        List<BO.Engineer> engineers = new List<BO.Engineer>();
        foreach (var gro in engineerGroup)
        {
            if (gro.Key >= level)
                foreach (var g in gro)
                    engineers.Add(g);
        }
        return engineers;
    }

    /// <summary>
    /// Retrieves engineers sorted by their cost per hour.
    /// </summary>
    /// <returns>An enumerable collection of engineers sorted by their cost per hour.</returns>
    public IEnumerable<BO.Engineer> sortByCostForHour()
    {
        return from BO.Engineer boEngineer in ReadAll()
               orderby boEngineer.Name, boEngineer.Cost
               select boEngineer;
    }

    /// <summary>
    /// Retrieves engineers sorted by their name.
    /// </summary>
    /// <returns>An enumerable collection of engineers sorted by their name.</returns>
    public IEnumerable<BO.Engineer> sortByName()
    {
        return from BO.Engineer boEngineer in ReadAll()
               orderby boEngineer.Name
               select boEngineer;
    }
}
