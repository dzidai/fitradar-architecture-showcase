namespace Fitradar.Domain.Workout.Validators.Context;

public sealed record WorkoutSeriesValidationContext
{
    public TimePeriod[] TimeSlots { get; init; }

    public Money Price { get; init; }

    public int NumberOfTickets { get; init; }

    public int NumberOfBookedTickets { get; init; }

    public bool IsCancelled { get; init; }

    public bool IsFreeOfCharge => Price?.Amount == null;
}
