using Fitradar.Domain.Events;

namespace Fitradar.Domain.Workout.Events;

public class WorkoutOccurrenceUpdated : IDomainEvent
{
    public WorkoutOccurrence UpdatedWorkoutOccurrence { get; set; }

    public WorkoutOccurrenceUpdated(WorkoutOccurrence updatedWorkoutOccurrence)
    {
        UpdatedWorkoutOccurrence = updatedWorkoutOccurrence;
    }
}
