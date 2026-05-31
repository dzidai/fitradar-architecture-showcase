using Fitradar.Domain.Common;
using Fitradar.Domain.Common.Validation;
using Fitradar.Domain.Workout.Events;
using Fitradar.Domain.Workout.Validators;
using Fitradar.Domain.Workout.Validators.Context;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fitradar.Domain.Workout
{
    public enum CancellationType
    {
        NO_PENALTY, PARTLY_CHARGE, FULL_CHARGE
    }

    public sealed class WorkoutSeries : EventSourcedEntity, IAggregateRoot
    {
        private static readonly UpdatedWorkoutSeriesValidator _eventUpdateValidator = new();

        private string _title;
        private string _description;
        private Money _price;
        private PaymentType _allowedPaymentType;
        private CancellationType _cancellationTerms;
        private AttendanceRestrictions _restrictions;
        private RecurrenceSchedule _recurrenceSchedule;
        private List<WorkoutOccurrence> _instances = [];
        private int _numberOfTickets;

        internal WorkoutSeries()
        {
        }

        public WorkoutSeries(Guid id) : this()
        {
            _isNewInstance = true;
            PublicId = id;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            AddPendingEvent(new WorkoutSeriesCreated(PublicId, CreatedAt, DateTime.UtcNow));
        }

        public WorkoutSeries(
            Guid id,
            string title,
            string description,
            Money price,
            PaymentType allowedPaymentType,
            CancellationType cancellationTerms,
            AttendanceRestrictions restrictions,
            RecurrenceSchedule recurrenceSchedule,
            int numberOfTickets,
            string createdFrom,
            DateTime createdAt,
            DateTime updatedAt) : this()
        {
            _isNewInstance = false;
            PublicId = id;
            _title = title;
            _description = description;
            _price = price;
            _allowedPaymentType = allowedPaymentType;
            _cancellationTerms = cancellationTerms;
            _restrictions = restrictions;
            _numberOfTickets = numberOfTickets;
            _recurrenceSchedule = recurrenceSchedule;
            CreatedFrom = createdFrom;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            _instances = [];
        }

        public string Title => _title;

        public string Description => _description;

        public Money Price => _price;

        public CancellationType CancellationTerms => _cancellationTerms;

        public PaymentType AllowedPaymentType => _allowedPaymentType;

        public AttendanceRestrictions Restrictions => _restrictions;

        public int NumberOfTickets => _numberOfTickets;

        public RecurrenceSchedule RecurrenceSchedule => _recurrenceSchedule;

        public IReadOnlyList<WorkoutOccurrence> Occurrences
        {
            get => _instances.AsReadOnly();
        }

        public void SetInstances(IEnumerable<WorkoutOccurrence> instances) => _instances = [.. instances];

        public string CreatedFrom { get; init; }

        public DateTime CreatedAt { get; init; }

        public DateTime? UpdatedAt { get; private set; }

        public bool IsRecurring => RecurrenceSchedule?.RecurrenceRule != null;

        private ValidationResult CanUpdate(Money price, int numberOfTickets)
        {
            var validationContext = new WorkoutSeriesValidationContext
            {
                TimeSlots = Occurrences
                    .Select(i => new TimePeriod(i.StartTime, i.EndTime))
                    .ToArray(),
                Price = price.Amount == null || price.Amount == 0 ? null : price,
                NumberOfTickets = numberOfTickets,
                NumberOfBookedTickets = Occurrences.Max(i => i.NumberOfBookedTickets),
                IsCancelled = Occurrences.Any(i => i.IsCancelled)
            };
            var validationResult = _eventUpdateValidator.Validate(validationContext);

            return validationResult;
        }

        public Result UpdateDetails(
            string title,
            string description,
            Money price,
            PaymentType allowedPaymentType,
            CancellationType cancellationTerms,
            AttendanceRestrictions restrictions,
            RecurrenceSchedule recurrenceSchedule,
            int numberOfTickets)
        {
            var normalizedPrice = NormalizePrice(price);
            var validationResult = CanUpdate(normalizedPrice, numberOfTickets);

            if (!validationResult.IsValid)
            {
                return Result.Failure(ToFailureMessage(validationResult));
            }

            var hasChanged =
                _title != title ||
                _description != description ||
                _price != normalizedPrice ||
                _allowedPaymentType != allowedPaymentType ||
                _cancellationTerms != cancellationTerms ||
                _restrictions != restrictions ||
                _recurrenceSchedule != recurrenceSchedule ||
                _numberOfTickets != numberOfTickets;

            _title = title;
            _description = description;
            _price = normalizedPrice;
            _allowedPaymentType = allowedPaymentType;
            _cancellationTerms = cancellationTerms;
            _restrictions = restrictions;
            _recurrenceSchedule = recurrenceSchedule;
            _numberOfTickets = numberOfTickets;

            if (!_isNewInstance && hasChanged)
            {
                QueueUpdatedEventIfNeeded();
            }

            return Result.Success();
        }

        public WorkoutSeries CreateExceptionInRecurrence(
            DateTime exceptionStartDateTime,
            DateTime exceptionEndDateTime)
        {
            var exceptionSchedule = new RecurrenceSchedule
            {
                DtStart = exceptionStartDateTime,
                DtEnd = exceptionEndDateTime
            };
            var clonedSportEvent = CloneWithoutInstances(exceptionSchedule);

            // Exclude the exception date from this series (non-mutating — replaces the field)
            _recurrenceSchedule = _recurrenceSchedule.WithExceptionDate(exceptionStartDateTime);
            QueueUpdatedEventIfNeeded();

            return clonedSportEvent;
        }

        private WorkoutSeries CloneWithoutInstances(RecurrenceSchedule recurrenceSchedule)
        {
            var clonedSportEvent = new WorkoutSeries
            (
                id: Guid.NewGuid(),
                title: _title,
                description: _description,
                price: _price,
                allowedPaymentType: _allowedPaymentType,
                cancellationTerms: _cancellationTerms,
                restrictions: _restrictions,
                recurrenceSchedule: recurrenceSchedule,
                numberOfTickets: _numberOfTickets,
                createdFrom: CreatedFrom,
                createdAt: DateTime.UtcNow,
                updatedAt: DateTime.UtcNow
            );
            return clonedSportEvent;
        }

        private void QueueUpdatedEventIfNeeded()
        {
            if (DomainEvents.Any(e => e is WorkoutSeriesUpdated))
            {
                return;
            }

            UpdatedAt = DateTime.UtcNow;

            AddPendingEvent(
                new WorkoutSeriesUpdated(
                    WorkoutSeriesId: PublicId,
                    Title: _title,
                    Description: _description,
                    Price: ClonePrice(_price),
                    AllowedPaymentType: _allowedPaymentType,
                    CancellationTerms: _cancellationTerms,
                    Restrictions: _restrictions,
                    RecurrenceSchedule: _recurrenceSchedule,
                    NumberOfTickets: _numberOfTickets,
                    UpdatedAtUtc: UpdatedAt,
                    OccurredAtUtc: DateTime.UtcNow));
        }

        private static Money ClonePrice(Money price)
        {
            if (price == null)
            {
                return null;
            }

            return new Money(price);
        }

        private static Money NormalizePrice(Money price)
        {
            if (price == null || price.Amount == null || price.Amount == 0)
            {
                return null;
            }

            return price;
        }

        private static string ToFailureMessage(ValidationResult validationResult)
        {
            return string.Join("; ", validationResult.Errors.Select(e => $"{e.Property}:{e.Code}"));
        }
    }
}
