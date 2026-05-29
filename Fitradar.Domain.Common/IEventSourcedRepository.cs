namespace Fitradar.Domain
{
    public interface IEventSourcedRepository
    {
        Task SaveAndPublishEventsAsync();
    }
}
