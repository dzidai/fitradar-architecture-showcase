using Azure.Messaging.ServiceBus;
using Fitradar.Application.Services;
using Microsoft.Extensions.Options;

namespace Fitradar.Infrastructure.Azure
{
    public class ServiceBusQueue : IServiceBusQueue
    {
        public const string CommentsQueueName = "Comments";

        private readonly ServiceBusClient _client;
        private readonly ServiceBusOptions _options;

        public ServiceBusQueue(ServiceBusClient client, IOptions<ServiceBusOptions> options)
        {
            _client = client;
            _options = options.Value;
        }

        public Task EnqueueMessageAsync(string routeKey, string payload,
            CancellationToken cancellationToken = default)
            => SendMessageAsync(payload, ResolveRoute(routeKey), cancellationToken);

        public Task EnqueueMessageAsync(string routeKey, Guid payload,
            CancellationToken cancellationToken = default)
            => EnqueueMessageAsync(routeKey, payload.ToString(), cancellationToken);

        public Task EnqueueMessageAsync(string routeKey, long payload,
            CancellationToken cancellationToken = default)
            => EnqueueMessageAsync(routeKey, payload.ToString(), cancellationToken);

        public Task<long> ScheduleMessageAsync(string routeKey, string payload, TimeSpan delay,
            CancellationToken cancellationToken = default)
            => ScheduleMessageAsync(payload, DateTimeOffset.UtcNow.Add(delay), ResolveRoute(routeKey), cancellationToken);

        public Task<long> ScheduleMessageAsync(string routeKey, string payload, DateTime scheduledEnqueueTimeUtc,
            CancellationToken cancellationToken = default)
            => ScheduleMessageAsync(payload, new DateTimeOffset(scheduledEnqueueTimeUtc, TimeSpan.Zero), ResolveRoute(routeKey), cancellationToken);

        public Task<long> ScheduleMessageAsync(string routeKey, Guid payload, DateTime scheduledEnqueueTimeUtc,
            CancellationToken cancellationToken = default)
            => ScheduleMessageAsync(routeKey, payload.ToString(), scheduledEnqueueTimeUtc, cancellationToken);

        public Task<long> ScheduleMessageAsync(string routeKey, long payload, DateTime scheduledEnqueueTimeUtc,
            CancellationToken cancellationToken = default)
            => ScheduleMessageAsync(routeKey, payload.ToString(), scheduledEnqueueTimeUtc, cancellationToken);

        public Task CancelScheduledMessageAsync(string routeKey, long sequenceNumber,
            CancellationToken cancellationToken = default)
            => CancelMessageAsync(sequenceNumber, ResolveRoute(routeKey), cancellationToken);

        public Task EnqueueBatchMessagesAsync(string routeKey, string[] payloads,
            CancellationToken cancellationToken = default)
            => SendBatchMessageAsync(payloads, ResolveRoute(routeKey), cancellationToken);

        private string ResolveRoute(string routeKey)
        {
            if (string.IsNullOrWhiteSpace(routeKey))
                throw new ArgumentException("Route key must be provided.", nameof(routeKey));

            if (_options.RouteMap.TryGetValue(routeKey, out var configured) &&
                !string.IsNullOrWhiteSpace(configured))
                return configured;

            return routeKey;
        }

        private async Task SendMessageAsync(string msg, string queueOrTopicName,
            CancellationToken cancellationToken)
        {
            await using var sender = _client.CreateSender(queueOrTopicName);
            await sender.SendMessageAsync(new ServiceBusMessage(msg), cancellationToken);
        }

        private async Task<long> ScheduleMessageAsync(string msg, DateTimeOffset scheduledEnqueueTime,
            string queueName, CancellationToken cancellationToken)
        {
            await using var sender = _client.CreateSender(queueName);
            return await sender.ScheduleMessageAsync(new ServiceBusMessage(msg), scheduledEnqueueTime, cancellationToken);
        }

        private async Task SendBatchMessageAsync(string[] msgs, string queueName,
            CancellationToken cancellationToken)
        {
            var messages = new Queue<ServiceBusMessage>(msgs.Select(m => new ServiceBusMessage(m)));
            int totalCount = messages.Count;

            await using var sender = _client.CreateSender(queueName);
            while (messages.Count > 0)
            {
                using var batch = await sender.CreateMessageBatchAsync(cancellationToken);

                if (!batch.TryAddMessage(messages.Peek()))
                    throw new InvalidOperationException(
                        $"Message {totalCount - messages.Count + 1} is too large to fit in a batch.");

                messages.Dequeue();

                while (messages.Count > 0 && batch.TryAddMessage(messages.Peek()))
                    messages.Dequeue();

                await sender.SendMessagesAsync(batch, cancellationToken);
            }
        }

        private async Task CancelMessageAsync(long sequenceNumber, string queueName,
            CancellationToken cancellationToken)
        {
            await using var sender = _client.CreateSender(queueName);
            await sender.CancelScheduledMessageAsync(sequenceNumber, cancellationToken);
        }
    }
}

