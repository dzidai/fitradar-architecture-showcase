using MediatR;

namespace Fitradar.Application.Common
{
    /// <summary>
    /// Marker interface for integration events that are dispatched out-of-process
    /// via Azure Service Bus for eventual processing by Azure Functions.
    ///
    /// Use this when:
    /// - The operation is long-running or resource-intensive (typically &gt;2s)
    /// - Integration with external systems is required (e.g., Jira, Stripe, external APIs)
    /// - Decoupling from the main request/response cycle is needed
    /// - Resilience and retry logic are important
    /// - Examples: Creating Jira tickets, complex data synchronization, external service calls
    ///
    /// These events are sent to Service Bus queues.
    /// Azure Functions consume these events, load full entities, and publish in-process events
    /// to trigger the actual business logic handlers.
    /// </summary>
    public interface IOutboundIntegrationEvent : INotification
    {
    }
}
