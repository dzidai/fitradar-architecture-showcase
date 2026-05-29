namespace Fitradar.Domain.Common.Validation
{
    /// <summary>
    /// Contains the result of a <see cref="IEntityValidator{TEntity}.Validate"/> method call.
    /// </summary>
    [Serializable]
    public class ValidationResult
    {
        private readonly List<ValidationError> _errors = [];

        /// <summary>
        /// Gets whether the validation operation on an entity was valid or not.
        /// </summary>
        public bool IsValid => _errors.Count == 0;

        /// <summary>
        /// Gets whether the validation operation is just a warning and it might be ignored.
        /// </summary>
        public bool IsWarning => _errors.Count > 0 && _errors.All(e => e.IsWarning);

        /// <summary>
        /// Gets an <see cref="IEnumerable{ValidationError}"/> that can be used to enumerate over
        /// the validation errors as a result of a <see cref="IEntityValidator{TEntity}.Validate"/> method
        /// call.
        /// </summary>
        public IReadOnlyList<ValidationError> Errors => _errors;

        /// <summary>
        /// Adds a validation error into the result.
        /// </summary>
        /// <param name="error"></param>
        public void AddError(ValidationError error)
        {
            ArgumentNullException.ThrowIfNull(error);
            _errors.Add(error);
        }

        /// <summary>
        /// Removes a validation error from the result.
        /// </summary>
        /// <param name="error"></param>
        public void RemoveError(ValidationError error) => _errors.Remove(error);
    }
}
