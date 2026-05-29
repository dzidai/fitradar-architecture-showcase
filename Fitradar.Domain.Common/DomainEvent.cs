using Fitradar.Domain.Events;

namespace Fitradar.Domain.Common
{
    public abstract class DomainEvent : IDomainEvent
    {
        public override bool Equals(object? obj)
        {
            // Check if the objects are the same instance
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            // Check if the object is null or of a different type
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            // Perform other necessary equality checks on properties or fields

            // Return true if all equality conditions are met, otherwise return false
            return true;
        }

        public override int GetHashCode()
        {
            var hash = GetType()?.FullName?.GetHashCode();
            return hash ?? 1;
        }
    }
}
