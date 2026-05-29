using Fitradar.Domain.Specifications;

namespace Fitradar.Domain.Common.Specifications
{
    /// <summary>
    /// Base implementation that uses <see cref="ISpecification{T}"/> instances that provide the logic to check if the
    /// rule is satisfied.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class SpecificationRuleBase<TEntity> where TEntity : notnull
    {
        private readonly ISpecification<TEntity> _rule;

        /// <summary>
        /// Default Constructor.
        /// Protected. Must be called by implementors.
        /// </summary>
        /// <param name="rule">A <see cref="ISpecification{TEntity}"/> instance that specifies the rule.</param>
        protected SpecificationRuleBase(ISpecification<TEntity> rule)
        {
            ArgumentNullException.ThrowIfNull(rule);
            _rule = rule;
        }

        /// <summary>
        /// Checks if the entity instance satisfies this rule.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/> instance.</param>
        /// <returns>bool. True if the rule is satisfied, else false.</returns>
        public bool IsSatisfied(TEntity entity) => _rule.IsSatisfiedBy(entity);
    }
}
