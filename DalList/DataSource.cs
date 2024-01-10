namespace Dal;

internal static class DataSource
{
    internal static class Config
    {
        //( public DateTime )Start date of work on the project
        //   Planned date for project completion

        //For a running ID number for the Dependency entity
        internal const int startDependencyId =0;
        private static int nextDependencyId = startDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }
        //For a running ID number for the Task entity
        internal const int startTaskId = 0;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }
    }
    // DependencyImplementation

    internal static List<DO.Engineer> Engineers { get; } = new();
    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Dependency> Dependences { get; } = new();
}
