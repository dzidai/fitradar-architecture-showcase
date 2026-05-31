using Fitradar.Domain.Events;

namespace Fitradar.Domain.Common;

public abstract class EventSourcedEntity : IEventSourcedEntity
{
    public long Id { get; set; }
    public Guid PublicId { get; set; }

    private readonly HashSet<IDomainEvent> _pendingEvents = [];

    protected bool _isNewInstance;

    public IEnumerable<IDomainEvent> DomainEvents
    {
        get { return _pendingEvents; }
    }

    public void AddPendingEvent(IDomainEvent e)
    {
        _pendingEvents.Add(e);
    }

    public void ClearPendingEvents()
    {
        _pendingEvents.Clear();
    }
}
