using Fitradar.Domain.Common.Specifications;
using Fitradar.Domain.Specifications;

namespace Fitradar.Domain.Common.BusinessRules
{
    /// <summary>
    /// Implements the <see cref="IBusinessRule{TEntity}"/> interface and inherits from the
    /// <see cref="SpecificationRuleBase{TEntity}"/> to provide a implementation of a business rule that
    /// uses specifications as rule logic.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BusinessRule<TEntity> : SpecificationRuleBase<TEntity>, IBusinessRule<TEntity> where TEntity : notnull
    {
        private readonly Action<TEntity> _action; //The business action to undertake.

        /// <summary>
        /// Default Constructor.
        /// Creates a new instance of the <see cref="BusinessRule{TEntity}"/> instance.
        /// </summary>
        /// <param name="rule">A <see cref="ISpecification{T}"/> instance that acts as the underlying
        /// specification that this business rule is evaluated against.</param>
        /// <param name="action">A <see cref="Action{TEntity}"/> instance that is invoked when the business rule
        /// is satisfied.</param>
        public BusinessRule(ISpecification<TEntity> rule, Action<TEntity> action) : base(rule)
        {
            ArgumentNullException.ThrowIfNull(action);
            _action = action;
        }

        /// <summary>
        /// Evaluates the business rule against an entity instance.
        /// </summary>
        /// <param name="entity"><typeparamref name="TEntity"/>. The entity instance against which
        /// the business rule is evaluated.</param>
        public void Evaluate(TEntity entity)
        {
            if (IsSatisfied(entity))
                _action(entity);
        }
    }
}
