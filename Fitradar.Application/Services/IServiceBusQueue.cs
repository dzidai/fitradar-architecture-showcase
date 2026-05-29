using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fitradar.Application.Services
{
    public interface IServiceBusQueue
    {
        Task EnqueueMessageAsync(string routeKey, string payload,
            CancellationToken cancellationToken = default);

        Task EnqueueMessageAsync(string routeKey, Guid payload,
            CancellationToken cancellationToken = default);

        Task EnqueueMessageAsync(string routeKey, long payload,
            CancellationToken cancellationToken = default);

        Task<long> ScheduleMessageAsync(string routeKey, string payload, TimeSpan delay,
            CancellationToken cancellationToken = default);

        Task<long> ScheduleMessageAsync(string routeKey, string payload, DateTime scheduledEnqueueTimeUtc,
            CancellationToken cancellationToken = default);

        Task<long> ScheduleMessageAsync(string routeKey, Guid payload, DateTime scheduledEnqueueTimeUtc,
            CancellationToken cancellationToken = default);

        Task<long> ScheduleMessageAsync(string routeKey, long payload, DateTime scheduledEnqueueTimeUtc,
            CancellationToken cancellationToken = default);

        Task CancelScheduledMessageAsync(string routeKey, long sequenceNumber,
            CancellationToken cancellationToken = default);

        Task EnqueueBatchMessagesAsync(string routeKey, string[] payloads,
            CancellationToken cancellationToken = default);
    }
}
