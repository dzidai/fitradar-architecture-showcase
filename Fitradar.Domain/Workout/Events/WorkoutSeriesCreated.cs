using Fitradar.Domain.Events;

namespace Fitradar.Domain.Workout.Events;

public sealed record WorkoutSeriesCreated(
    System.Guid WorkoutSeriesId,
    System.DateTime CreatedAtUtc,
    System.DateTime OccurredAtUtc) : IDomainEvent;
