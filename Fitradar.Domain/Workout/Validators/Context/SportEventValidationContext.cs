namespace Fitradar.Domain.Workout.Validators.Context
{
    public class SportEventValidationContext
    {
        public TimePeriod[] TimeSlots { get; set; }

        public Money Price { get; set; }

        public int NumberOfTickets { get; set; }

        public int NumberOfBookedTickets { get; set; }

        public bool IsCancelled { get; set; }

        public bool IsFreeOfCharge => Price?.Amount == null;
    }
}
