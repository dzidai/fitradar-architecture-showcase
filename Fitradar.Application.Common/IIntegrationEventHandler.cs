using MediatR;

namespace Fitradar.Application.Common
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : INotificationHandler<TIntegrationEvent>
        where TIntegrationEvent : INotification
    {
    }
}
