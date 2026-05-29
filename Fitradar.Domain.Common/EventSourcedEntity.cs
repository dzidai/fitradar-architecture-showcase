using Fitradar.Domain.Common;
using Fitradar.Domain.Events;

namespace Fitradar.Domain
{
    public abstract class EventSourcedEntity : IEventSourcedEntity
    {
        private readonly HashSet<IDomainEvent> _pendingEvents = [];

        protected bool _isNewInstance;

        public IEnumerable<IDomainEvent> Events
        {
            get { return _pendingEvents; }
        }

        public void AddPendingEvent(IDomainEvent e)
        {
            _pendingEvents.Add(e);
        }
    }
}
