using Fitradar.Domain.Events;

namespace Fitradar.Domain.Common
{
    public interface IEventSourcedEntity
    {
        IEnumerable<IDomainEvent> Events { get; }
    }
}
