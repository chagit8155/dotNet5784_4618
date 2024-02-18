using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BO;

/// <summary>
/// Exception for an entity with an ID number that does not exist in the list
/// </summary>
[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}
/// <summary>
/// Exception for property with null value
/// </summary>
[Serializable]
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
}

/// <summary>
/// Exception for an entity with an ID number that already exists in the list
/// </summary>
[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException)
                : base(message, innerException) { }

}
/// <summary>
/// Exception for deleting an entity that is not allowed to be deleted
/// </summary>
[Serializable]
public class BlDeletionImpossibleException : Exception
{
    public BlDeletionImpossibleException(string? message) : base(message) { }
}


/// <summary>
/// Exception for fail to load xml file
/// </summary>

[Serializable]
public class DalXMLFileLoadCreateException : Exception
{
    public DalXMLFileLoadCreateException(string? message) : base(message) { }
}
/// <summary>
/// Exception for Incorrect format given value
/// </summary>
[Serializable]
public class BlInvalidInputFormatException : Exception
{
    public BlInvalidInputFormatException(string? message) : base(message) { }
    public BlInvalidInputFormatException(string message, Exception innerException)
    : base(message, innerException) { }
}



[Serializable]
public class BlincorrectDateOrderException : Exception
{
    public BlincorrectDateOrderException(string? message) : base(message) { }
}

/// <summary>
/// Exception for Impossible update with given value
/// </summary>
[Serializable]
public class BlCannotBeUpdatedException : Exception
{
    public BlCannotBeUpdatedException(string? message) : base(message) { }
}

/// <summary>
/// Exception for execution impossible at this stage of the project
/// </summary>
[Serializable]
public class BlTheProjectTimeDoesNotAllowException : Exception
{
    public BlTheProjectTimeDoesNotAllowException(string? message) : base(message) { }
}