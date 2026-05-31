using Fitradar.Domain.Events;

namespace Fitradar.Domain.Workout.Events;

public class WorkoutOccurrenceDeleted : IDomainEvent
{
    public WorkoutOccurrence DeletedWorkoutOccurrence { get; set; }

    public WorkoutOccurrenceDeleted(WorkoutOccurrence deletedWorkoutOccurrence)
    {
        DeletedWorkoutOccurrence = deletedWorkoutOccurrence;
    }
}
