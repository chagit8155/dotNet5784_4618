namespace DO;
/// <summary>
/// Task Entity represents a task with all its props
/// </summary>
/// <param name="Id"> Unique ID number (automatic runner number) </param>
/// <param name="EngineerId"> The engineer ID assigned to the task </param>
/// <param name="Alias"></param>
/// <param name="Description"></param>
/// <param name="IsMilestone"></param>
/// <param name="CreatedAtDate"> Task's creation date </param>
/// <param name="ScheduledDate"> Planned date for work to begin </param>
/// <param name="StartDate"></param>
/// <param name="RequiredEffortTime"> The amount of time required to perform the task</param>
/// <param name="Copmlexity"> Task's difficulty level </param>
/// <param name="Deadline"> Possible final end date </param>
/// <param name="CompleteDate"> Actual end date </param>
/// <param name="Deliverables"> products </param>
/// <param name="Remarks"></param>

public record Task
(
     int Id, 
     int EngineerId,
     bool IsMilestone = false,
     string? Alias = null,
     string? Description = null,
     DateTime? CreatedAtDate = null,
     DateTime? ScheduledDate = null,
     DateTime? StartDate = null,
     TimeSpan? RequiredEffortTime = null,
     DO.EngineerExperience Copmlexity = EngineerExperience.Beginner,
     DateTime? Deadline = null,
     DateTime? CompleteDate = null,
     string? Deliverables = null,
     string? Remarks = null    
    )
{
    public Task() : this(0,0) { }//empty ctor 
//    public Task() : this(Id: 0, Alias: "", Description: "", CreatedAtDate: DateTime.Now) { }
    public bool ShouldSerializeScheduledDate() { return ScheduledDate.HasValue; }
    public bool ShouldSerializeStartDate() { return StartDate.HasValue; }
    public bool ShouldSerializeRequiredEffortTime() { return RequiredEffortTime.HasValue; }
    public bool ShouldSerializeDeadlineDate() { return Deadline.HasValue; }
    public bool ShouldSerializeCompleteDate() { return CompleteDate.HasValue; }
    public bool ShouldSerializeDeliverables() { return !string.IsNullOrEmpty(Deliverables); }
    public bool ShouldSerializeRemarks() { return !string.IsNullOrEmpty(Remarks); }
//    public bool ShouldSerializeEngineerId() { return EngineerId.HasValue; }
  //  public bool ShouldSerializeComplexity() { return Copmlexity.HasValue; }

}
