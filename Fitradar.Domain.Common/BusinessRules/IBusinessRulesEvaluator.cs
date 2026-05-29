namespace Fitradar.Domain.Common.BusinessRules
{
    /// <summary>
    /// Defines an interface implemented by a business rule evaluator for an entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that the business rules are applicable for.</typeparam>
    public interface IBusinessRulesEvaluator<in TEntity> where TEntity : notnull
    {
        /// <summary>
        /// Evaluates a business rules against an entity.
        /// </summary>
        /// <param name="entity">A <typeparamref name="TEntity"/> instance against which the business
        /// rules are evaluated.</param>
        void Evaluate(TEntity entity);
    }
}
