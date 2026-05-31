using Fitradar.SharedKernel.Extensions;
using System;
using System.Text.RegularExpressions;

namespace Fitradar.Domain.Workout
{
    /// <summary>
    /// Value Object that describes when a <see cref="WorkoutSeries"/> occurs,
    /// including recurrence rules and exception dates (RFC 5545 / iCalendar semantics).
    /// Immutable: all "mutation" methods return a new instance.
    /// </summary>
    public sealed record RecurrenceSchedule
    {
        /// <summary>RFC 5545 RRULE string (e.g. "FREQ=WEEKLY;COUNT=10"). Null for a one-off event.</summary>
        public string RecurrenceRule { get; init; }

        /// <summary>RFC 5545 RDATE list of additional occurrence dates.</summary>
        public string RecurrenceDates { get; init; }

        /// <summary>RFC 5545 EXRULE string for exception recurrence rules.</summary>
        public string ExceptionRule { get; init; }

        /// <summary>RFC 5545 EXDATE list of dates excluded from the recurrence.</summary>
        public string ExceptionDates { get; init; }

        /// <summary>UTC start date/time of the first (or only) occurrence.</summary>
        public DateTime DtStart { get; init; }

        /// <summary>UTC end date/time of the first (or only) occurrence.</summary>
        public DateTime DtEnd { get; init; }

        /// <summary>
        /// Returns a new <see cref="RecurrenceSchedule"/> with <paramref name="exceptionDate"/>
        /// appended to <see cref="ExceptionDates"/>.
        /// </summary>
        public RecurrenceSchedule WithExceptionDate(DateTime exceptionDate)
        {
            var formatted = exceptionDate.ToRfc2445String();
            var newDates = string.IsNullOrEmpty(ExceptionDates)
                ? formatted
                : ExceptionDates + "," + formatted;

            return this with { ExceptionDates = newDates };
        }

        /// <summary>
        /// Returns a new <see cref="RecurrenceSchedule"/> with the COUNT value in
        /// <see cref="RecurrenceRule"/> decremented by one.
        /// Returns a schedule with a null <see cref="RecurrenceRule"/> when COUNT reaches zero.
        /// </summary>
        public RecurrenceSchedule WithDecreasedCount()
        {
            if (string.IsNullOrEmpty(RecurrenceRule))
                return this;

            var newRule = Regex.Replace(
                RecurrenceRule,
                @"COUNT=(?<count>\d+)",
                DecrementCount,
                RegexOptions.IgnoreCase);

            return this with { RecurrenceRule = newRule.Contains("COUNT=0") ? null : newRule };
        }

        private static string DecrementCount(Match match)
        {
            if (match.Groups["count"].Success &&
                int.TryParse(match.Groups["count"].Value, out var count))
            {
                return $"COUNT={Math.Max(count - 1, 0)}";
            }

            return match.Value;
        }
    }
}
