namespace DO;
/// <summary>
/// Engineer Entity
/// </summary>
/// <param name="Id"> Unique ID number </param>
/// <param name="Email"></param>
/// <param name="Cost"> cost per hour </param>
/// <param name="Name"> Engineer's name (full name) </param>
/// <param name="Level"> Engineer's level </param>
public record Engineer
(
    int Id,
    string? Name=null,
    double? Cost = null,
    string? Email = null,
    EngineerExperience Level = EngineerExperience.Beginner
)
{
    public Engineer() : this(0) { }
}
