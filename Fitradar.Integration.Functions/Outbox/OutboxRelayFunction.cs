using Fitradar.Application.Common;
using Fitradar.Application.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fitradar.Integration.Functions.Outbox
{
    /// <summary>
    /// Timer-triggered relay that forwards pending outbox messages to Azure Service Bus.
    ///
    /// Runs every 10 seconds. Concurrency across multiple function host instances is safe
    /// because <see cref="IOutboxRepository.GetPendingAsync"/> uses WITH (UPDLOCK, READPAST),
    /// so each instance processes a disjoint batch.
    ///
    /// The raw JSON payload is forwarded verbatim — no deserialization or type registry needed.
    /// </summary>
    public class OutboxRelayFunction
    {
        private const int BatchSize = 20;
        private static readonly TimeSpan ProcessedMessageRetention = TimeSpan.FromDays(7);

        private readonly IOutboxRepository _outboxRepository;
        private readonly IServiceBusQueue _serviceBusQueue;
        private readonly ILogger<OutboxRelayFunction> _logger;

        public OutboxRelayFunction(
            IOutboxRepository outboxRepository,
            IServiceBusQueue serviceBusQueue,
            ILoggerFactory loggerFactory)
        {
            _outboxRepository = outboxRepository;
            _serviceBusQueue = serviceBusQueue;
            _logger = loggerFactory.CreateLogger<OutboxRelayFunction>();
        }

        [Function("OutboxRelay")]
        public async Task Run([TimerTrigger("*/10 * * * * *")] TimerInfo timer, CancellationToken cancellationToken)
        {
            var messages = await _outboxRepository.GetPendingAsync(BatchSize, cancellationToken);

            if (messages.Count == 0)
                return;

            _logger.LogDebug("OutboxRelay dispatching {Count} pending message(s).", messages.Count);

            var processedIds = new List<Guid>(messages.Count);

            foreach (var message in messages)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                try
                {
                    await _serviceBusQueue.EnqueueMessageAsync(message.RouteKey, message.Payload, cancellationToken);
                    processedIds.Add(message.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "OutboxRelay failed to dispatch message {MessageId} with route key '{RouteKey}'.",
                        message.Id, message.RouteKey);

                    await _outboxRepository.MarkFailedAsync(message.Id, ex.Message, cancellationToken);
                }
            }

            if (processedIds.Count > 0)
            {
                await _outboxRepository.MarkProcessedBatchAsync(processedIds, cancellationToken);
            }

            await _outboxRepository.PurgeProcessedAsync(ProcessedMessageRetention, cancellationToken);
        }
    }
}
