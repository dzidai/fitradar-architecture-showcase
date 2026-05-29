namespace Fitradar.Domain.Common.BusinessRules
{
    ///<summary>
    /// A base class that implementors of <see cref="IBusinessRulesEvaluator{TEntity}"/> can use to provide
    /// business rule evaluation logic for their entities.
    ///</summary>
    ///<typeparam name="TEntity"></typeparam>
    public abstract class BusinessRulesEvaluatorBase<TEntity> : IBusinessRulesEvaluator<TEntity> where TEntity : notnull
    {
        //The internal dictionary used to store rule sets.
        private readonly Dictionary<string, IBusinessRule<TEntity>> _ruleSets = [];

        /// <summary>
        /// Adds a <see cref="IBusinessRule{TEntity}"/> instance to the rules evaluator.
        /// </summary>
        /// <param name="rule">The <see cref="IBusinessRule{TEntity}"/> instance to add.</param>
        /// <param name="ruleName">string. The unique name assigned to the business rule.</param>
        protected virtual void AddRule(string ruleName, IBusinessRule<TEntity> rule)
        {
            ArgumentNullException.ThrowIfNull(rule);
            ArgumentException.ThrowIfNullOrEmpty(ruleName);
            if (_ruleSets.ContainsKey(ruleName))
                throw new ArgumentException("Another rule with the same name already exists. Cannot add duplicate rules.");

            _ruleSets.Add(ruleName, rule);
        }

        /// <summary>
        /// Removes a previously added rule, specified with the <paramref name="ruleName"/>, from the evaluator.
        /// </summary>
        /// <param name="ruleName">string. The name of the rule to remove.</param>
        protected virtual void RemoveRule(string ruleName)
        {
            ArgumentException.ThrowIfNullOrEmpty(ruleName);
            _ruleSets.Remove(ruleName);
        }

        /// <summary>
        /// Evaluates all business rules registered with the evaluator against a entity instance.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/> instance against which all
        /// registered business rules are evaluated.</param>
        public virtual void Evaluate(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            foreach (var rule in _ruleSets.Values)
                rule.Evaluate(entity);
        }
    }
}
