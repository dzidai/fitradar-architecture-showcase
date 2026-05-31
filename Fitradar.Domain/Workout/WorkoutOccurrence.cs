using Fitradar.Domain.Common;
using Fitradar.Domain.Common.Validation;
using Fitradar.Domain.Workout.Events;
using Fitradar.Domain.Workout.Validators;
using Fitradar.Domain.Workout.Validators.Context;
using System;

namespace Fitradar.Domain.Workout
{
    public enum EventStatus { Upcoming, Ongoing, Finished }

    public sealed class WorkoutOccurrence : EventSourcedEntity
    {
        private static readonly UpdatedWorkoutSeriesValidator _updateValidator = new();

        private readonly TimeProvider _clock;
        private readonly DateTime _initStartTime;
        private readonly DateTime _initEndTime;

        internal WorkoutOccurrence(
            Guid publicId,
            Guid sportEventId,
            DateTime startTime,
            DateTime endTime,
            int numberOfBookedTickets,
            bool isCancelled,
            TimeProvider clock)
        {
            _clock = clock ?? TimeProvider.System;
            PublicId = publicId;
            SportEventId = sportEventId;
            StartTime = startTime;
            _initStartTime = startTime;
            EndTime = endTime;
            _initEndTime = endTime;
            IsCancelled = isCancelled;
            NumberOfBookedTickets = numberOfBookedTickets;
        }

        public Guid SportEventId { get; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public int NumberOfBookedTickets { get; private set; }
        public bool IsCancelled { get; private set; }

        public EventStatus Status
        {
            get
            {
                var now = _clock.GetUtcNow().UtcDateTime;
                if (StartTime > now) return EventStatus.Upcoming;
                if (EndTime < now) return EventStatus.Finished;
                return EventStatus.Ongoing;
            }
        }

        public bool IsUpcoming => Status == EventStatus.Upcoming;
        public bool IsOngoing => Status == EventStatus.Ongoing;
        public bool IsFinished => Status == EventStatus.Finished;

        /// <summary>Reschedule the instance. Raises domain event only if the time actually changed.</summary>
        public void Reschedule(DateTime newStart, DateTime newEnd)
        {
            // Guard: cannot reschedule a cancelled instance
            if (IsCancelled)
                throw new InvalidOperationException("Cannot reschedule a cancelled event instance.");

            StartTime = newStart;
            EndTime = newEnd;

            if (newStart != _initStartTime || newEnd != _initEndTime)
                AddPendingEvent(new WorkoutOccurrenceUpdated(this));
        }

        public ValidationResult CanUpdate(TimePeriod timeSlot, Money price, int numberOfTickets)
        {
            var context = new WorkoutSeriesValidationContext
            {
                TimeSlots = [timeSlot],
                Price = price.Amount is null or 0 ? null : price,
                NumberOfTickets = numberOfTickets,
                NumberOfBookedTickets = NumberOfBookedTickets,
                IsCancelled = IsCancelled
            };
            var result = _updateValidator.Validate(context);

            return result;
        }
    }
}