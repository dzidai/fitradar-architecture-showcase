using Fitradar.Domain.Events;

namespace Fitradar.Domain.Workout.Events;

public sealed record WorkoutSeriesUpdated(
    System.Guid WorkoutSeriesId,
    string Title,
    string Description,
    Money Price,
    PaymentType AllowedPaymentType,
    CancellationType CancellationTerms,
    AttendanceRestrictions Restrictions,
    RecurrenceSchedule RecurrenceSchedule,
    int NumberOfTickets,
    System.DateTime? UpdatedAtUtc,
    System.DateTime OccurredAtUtc) : IDomainEvent;
