namespace DO;
/// <summary>
/// Dependency Entity
/// </summary>
/// <param name="Id"> Unique ID number (automatic runner number) </param>
/// <param name="DependentTask"> ID number of a dependency task </param>
/// <param name="DependsOnTask"> Previous assignment ID number </param>
public record Dependency
(
    int Id,
    int? DependentTask,
    int? DependsOnTask = null
)
{
    public Dependency() : this(0, 0) { } //empty ctor 
}
