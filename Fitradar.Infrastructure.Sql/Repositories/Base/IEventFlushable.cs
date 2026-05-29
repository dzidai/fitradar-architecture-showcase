using System.Threading.Tasks;

namespace Fitradar.Infrastructure.Sql.Repositories.Base
{
    /// <summary>
    /// Infrastructure-internal contract for flushing pending domain events and persisting changes.
    /// Only <see cref="UnitOfWork"/> and <see cref="EventSourcedRepository{TEntity}"/> should depend on this.
    /// Application and Domain layers must not reference this interface.
    /// </summary>
    internal interface IEventFlushable
    {
        Task SaveAndPublishEventsAsync();
    }
}
