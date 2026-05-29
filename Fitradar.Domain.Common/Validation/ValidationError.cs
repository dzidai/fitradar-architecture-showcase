namespace Fitradar.Domain.Common.Validation
{
    /// <summary>
    /// Represents a validation error from a <see cref="IEntityValidator{TEntity}.Validate"/> method
    /// call.
    /// </summary>
    [Serializable]
    public class ValidationError : IEquatable<ValidationError>
    {
        ///<summary>
        /// The message code that can be used as error Id in the other systems.
        ///</summary>
        public string Code { get; init; }

        ///<summary>
        /// The message that describes this validation error.
        ///</summary>
        public string Message { get; init; }

        ///<summary>
        /// The property that this validation error is associated with.
        ///</summary>
        public string Property { get; init; }

        ///<summary>
        /// The error is just a warning that can be ignored and will not break a business logic
        ///</summary>
        public bool IsWarning { get; init; }

        /// <summary>
        /// Default Constructor.
        /// Creates a new instance of the <see cref="ValidationError"/> data structure.
        /// </summary>
        /// <param name="message">string. The validation error message.</param>
        /// <param name="property">string. The property that was validated.</param>
        /// <param name="code">string. The validation error code.</param>
        public ValidationError(string code, string message, string property, bool isWarning)
        {
            ArgumentException.ThrowIfNullOrEmpty(code);
            ArgumentException.ThrowIfNullOrEmpty(property);
            Message = message;
            Property = property;
            Code = code;
            IsWarning = isWarning;
        }

        /// <summary>
        /// Overridden. Gets a string that represents the validation error.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("({0}) - {1}. {2}", Property, Code, Message);
        }

        /// <summary>
        /// Overridden. Compares if an object is equal to the <see cref="ValidationError"/> instance.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((ValidationError)obj);
        }

        /// <summary>
        /// Overridden. Compares if a <see cref="ValidationError"/> instance is equal to this
        /// <see cref="ValidationError"/> instance.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(ValidationError obj)
        {
            return Equals(obj.Message, Message) && Equals(obj.Property, Property);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode() => HashCode.Combine(Message, Property);

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(ValidationError left, ValidationError right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(ValidationError left, ValidationError right)
        {
            return !left.Equals(right);
        }
    }
}
