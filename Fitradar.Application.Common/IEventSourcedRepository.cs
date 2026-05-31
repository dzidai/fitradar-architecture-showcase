namespace Fitradar.Application.Common;

public interface IEventSourcedRepository
{
    Task SaveAndPublishEventsAsync();
}
