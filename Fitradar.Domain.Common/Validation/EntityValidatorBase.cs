namespace Fitradar.Domain.Common.Validation
{
    ///<summary>
    /// Base class that implementors of <see cref="IEntityValidator{TEntity}"/> can use to
    /// provide validation logic for their entities.
    ///</summary>
    ///<typeparam name="TEntity"></typeparam>
    public abstract class EntityValidatorBase<TEntity> : IEntityValidator<TEntity> where TEntity : notnull
    {
        //The internal dictionary used to store rule sets.
        private readonly Dictionary<string, IValidationRule<TEntity>> _validations = [];

        /// <summary>
        /// Adds a <see cref="IValidationRule{TEntity}"/> instance to the entity validator.
        /// </summary>
        /// <param name="rule">The <see cref="IValidationRule{TEntity}"/> instance to add.</param>
        /// <param name="ruleName">string. The unique name assigned to the validation rule.</param>
        protected virtual void AddValidation(string ruleName, IValidationRule<TEntity> rule)
        {
            ArgumentNullException.ThrowIfNull(rule);
            ArgumentException.ThrowIfNullOrEmpty(ruleName);
            if (_validations.ContainsKey(ruleName))
                throw new ArgumentException("Another rule with the same name already exists. Cannot add duplicate rules.");

            _validations.Add(ruleName, rule);
        }

        /// <summary>
        /// Removes a previously added rule, specified with the <paramref name="ruleName"/>, from the evaluator.
        /// </summary>
        /// <param name="ruleName">string. The name of the rule to remove.</param>
        protected virtual void RemoveValidation(string ruleName)
        {
            ArgumentException.ThrowIfNullOrEmpty(ruleName);
            _validations.Remove(ruleName);
        }

        /// <summary>
        /// Validates an entity against all validations defined for the entity.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/> to validate.</param>
        /// <returns>A <see cref="ValidationResult"/> that contains the results of the validation.</returns>
        public virtual ValidationResult Validate(TEntity entity)
        {
            var result = new ValidationResult();
            foreach (var rule in _validations.Values)
            {
                if (!rule.Validate(entity))
                {
                    result.AddError(new ValidationError(
                        rule.ValidationMessageCode,
                        rule.ValidationMessage,
                        rule.ValidationProperty,
                        rule.IsValidationResultWarning));

                    if (rule.StopValidation)
                    {
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Gets a <see cref="IValidationRule{TEntity}"/> that was added to the validator with the specified
        /// rule name.
        /// </summary>
        /// <param name="ruleName">The name of the validation rule to retrieve.</param>
        /// <returns>A <see cref="IValidationRule{TEntity}"/> instance, or null if no rule stored with the specified
        /// rule name was found.</returns>
        protected IValidationRule<TEntity>? GetValidationRule(string ruleName)
        {
            _validations.TryGetValue(ruleName, out IValidationRule<TEntity>? rule);
            return rule;
        }
    }
}
