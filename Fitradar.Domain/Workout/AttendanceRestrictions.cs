using System;

namespace Fitradar.Domain.Workout
{
    /// <summary>
    /// Value Object describing who is eligible to attend a <see cref="WorkoutSeries"/>.
    /// All properties are optional — null means "no restriction on this dimension".
    /// </summary>
    public sealed record AttendanceRestrictions
    {
        /// <summary>Minimum participant age (inclusive). Null means no lower bound.</summary>
        public int? MinAge { get; init; }

        /// <summary>Maximum participant age (inclusive). Null means no upper bound.</summary>
        public int? MaxAge { get; init; }

        /// <summary>Gender eligibility. <see cref="GenderRestriction.None"/> means open to all.</summary>
        public GenderRestriction GenderRestriction { get; init; }

        public AttendanceRestrictions(int? minAge, int? maxAge, GenderRestriction genderRestriction)
        {
            if (minAge.HasValue && minAge.Value < 0)
                throw new ArgumentOutOfRangeException(nameof(minAge), "MinAge cannot be negative.");

            if (maxAge.HasValue && maxAge.Value < 0)
                throw new ArgumentOutOfRangeException(nameof(maxAge), "MaxAge cannot be negative.");

            if (minAge.HasValue && maxAge.HasValue && minAge.Value > maxAge.Value)
                throw new ArgumentException("MinAge cannot be greater than MaxAge.");

            MinAge = minAge;
            MaxAge = maxAge;
            GenderRestriction = genderRestriction;
        }
    }
}
