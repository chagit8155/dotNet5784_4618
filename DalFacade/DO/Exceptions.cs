namespace DO;

/// <summary>
/// Exception for an entity with an ID number that does not exist in the list
/// </summary>
[Serializable]
public class DalDoesNotExistsException : Exception 
{
    public DalDoesNotExistsException(string? message) : base(message) { }
}

/// <summary>
/// Exception for an entity with an ID number that already exists in the list
/// </summary>
[Serializable]
public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}


/// <summary>
/// Exception for deleting an entity that is not allowed to be deleted
/// </summary>
[Serializable]
public class DalDeletionImpossibleException : Exception
{
    public DalDeletionImpossibleException(string? message) : base(message) { }
}




