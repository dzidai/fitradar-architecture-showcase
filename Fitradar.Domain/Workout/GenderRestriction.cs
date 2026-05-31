namespace Fitradar.Domain.Workout
{
    /// <summary>
    /// Describes the gender eligibility restriction for attending a <see cref="WorkoutSeries"/>.
    /// This is a Workout-context concept distinct from a user's personal gender identity
    /// (see <c>Fitradar.Domain.Account.Gender</c>).
    /// </summary>
    public enum GenderRestriction
    {
        /// <summary>No gender restriction — open to all.</summary>
        None = 0,

        /// <summary>Restricted to male participants only.</summary>
        MaleOnly = 1,

        /// <summary>Restricted to female participants only.</summary>
        FemaleOnly = 2
    }
}
