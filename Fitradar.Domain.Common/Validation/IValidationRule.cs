namespace Fitradar.Domain.Common.Validation
{
    /// <summary>
    /// Provides a contract that defines a validation rule that provides validation logic  for an entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity this validation rule is applicable for.</typeparam>
    public interface IValidationRule<in TEntity> where TEntity : notnull
    {
        /// <summary>
        /// Gets the message of the validation rule.
        /// </summary>
        string ValidationMessage { get; }

        /// <summary>
        /// Gets the message code of the validation rule. It can be used instead of message itself
        /// </summary>
        string ValidationMessageCode { get; }

        /// <summary>
        /// Gets a generic or specific name of a property that was validated.
        /// </summary>
        string ValidationProperty { get; }

        /// <summary>
        /// Gets whether the validation result should be treated as a warning.
        /// </summary>
        bool IsValidationResultWarning { get; }

        ///<summary>
        /// If the rule fails stop checking other validation rules
        ///</summary>
        bool StopValidation { get; }

        /// <summary>
        /// Validates whether the entity violates the validation rule or not.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/> entity instance to validate.</param>
        /// <returns>Should return true if the entity instance is valid, else false.</returns>
        bool Validate(TEntity entity);
    }
}
