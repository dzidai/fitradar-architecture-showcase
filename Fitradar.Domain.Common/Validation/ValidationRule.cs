using Fitradar.Domain.Common.Specifications;
using Fitradar.Domain.Specifications;

namespace Fitradar.Domain.Common.Validation
{
    /// <summary>
    /// Implements the <see cref="IValidationRule{TEntity}"/> interface and inherits from the
    /// <see cref="SpecificationRuleBase{TEntity}"/> to provide a very basic implementation of an
    /// entity validation rule that uses specifications as underlying rule logic.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class ValidationRule<TEntity> : SpecificationRuleBase<TEntity>, IValidationRule<TEntity> where TEntity : notnull
    {
        /// <summary>
        /// Default Constructor.
        /// Creates a new instance of the <see cref="ValidationRule{TEntity}"/> class.
        /// </summary>
        /// <param name="code">string. The validation message code associated with the rule.</param>
        /// <param name="message">string. The validation message associated with the rule.</param>
        /// <param name="property">string. The generic or specific name of the property that was validated.</param>
        /// <param name="rule"></param>
        public ValidationRule(ISpecification<TEntity> rule, string code, string message, string property, bool isWarning)
            : base(rule)
        {
            ArgumentException.ThrowIfNullOrEmpty(code);
            ArgumentException.ThrowIfNullOrEmpty(message);
            ArgumentException.ThrowIfNullOrEmpty(property);
            ValidationMessage = message;
            ValidationProperty = property;
            ValidationMessageCode = code;
            IsValidationResultWarning = isWarning;
        }

        /// <summary>
        /// Default Constructor.
        /// Creates a new instance of the <see cref="ValidationRule{TEntity}"/> class.
        /// </summary>
        /// <param name="code">string. The validation message code associated with the rule.</param>
        /// <param name="property">string. The generic or specific name of the property that was validated.</param>
        /// <param name="rule"></param>
        public ValidationRule(ISpecification<TEntity> rule, string code, string property, bool isWarning, bool stopValidation = false)
            : base(rule)
        {
            ArgumentException.ThrowIfNullOrEmpty(code);
            ArgumentException.ThrowIfNullOrEmpty(property);
            ValidationProperty = property;
            ValidationMessageCode = code;
            IsValidationResultWarning = isWarning;
            StopValidation = stopValidation;
        }

        /// <summary>
        /// Gets the message of the validation rule.
        /// </summary>
        public string ValidationMessage { get; } = string.Empty;

        /// <summary>
        /// Gets the message code of the validation rule.
        /// </summary>
        public string ValidationMessageCode { get; }

        /// <summary>
        /// Gets a generic or specific name of a property that was validated.
        /// </summary>
        public string ValidationProperty { get; }

        /// <summary>
        /// Gets the severity of validation error
        /// </summary>
        public bool IsValidationResultWarning { get; }

        ///<summary>
        /// If the rule fails stop checking other validation rules
        ///</summary>
        public bool StopValidation { get; init; }

        /// <summary>
        /// Validates whether the entity violates the validation rule or not.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/> entity instance to validate.</param>
        /// <returns>Should return true if the entity instance is valid, else false.</returns>
        public bool Validate(TEntity entity) => IsSatisfied(entity);
    }
}
