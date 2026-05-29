namespace Fitradar.Application.Common
{
    /// <summary>
    /// Persistence contract for the transactional outbox.
    ///
    /// <para>
    /// Callers must invoke <see cref="Add"/> before calling <c>SaveChangesAsync</c> on the
    /// same <see cref="Microsoft.EntityFrameworkCore.DbContext"/> unit-of-work so that the
    /// business row and the outbox row are committed in a single atomic write.
    /// </para>
    /// </summary>
    public interface IOutboxRepository
    {
        /// <summary>
        /// Stages a pending outbox message in the current <c>DbContext</c> change tracker.
        /// The row is persisted only when the caller subsequently calls <c>SaveChangesAsync</c>.
        /// </summary>
        /// <param name="routeKey">
        /// The Service Bus route key used to resolve the target queue (matches
        /// <c>ServiceBusRouteKeys.For&lt;TEvent&gt;()</c>, i.e. the event type name).
        /// </param>
        /// <param name="payload">JSON-serialized event payload, forwarded verbatim to the queue.</param>
        void Add(string routeKey, string payload);

        /// <summary>
        /// Fetches the next batch of unprocessed messages using <c>UPDLOCK, READPAST</c>
        /// so that concurrent processor instances never pick up the same row.
        /// </summary>
        Task<IReadOnlyList<OutboxMessageEnvelope>> GetPendingAsync(
            int batchSize,
            CancellationToken cancellationToken = default);

        /// <summary>Marks a message as successfully dispatched to the Service Bus queue.</summary>
        Task MarkProcessedAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Marks a batch of messages as successfully dispatched in a single round trip.
        /// Prefer this over calling <see cref="MarkProcessedAsync"/> per message.
        /// </summary>
        Task MarkProcessedBatchAsync(IReadOnlyList<Guid> ids, CancellationToken cancellationToken = default);

        /// <summary>
        /// Records a dispatch failure and increments the retry counter. Rows whose
        /// <c>RetryCount</c> reaches <c>MaxRetryCount</c> are excluded from future
        /// <see cref="GetPendingAsync"/> polls and are considered dead-lettered.
        /// </summary>
        Task MarkFailedAsync(Guid id, string error, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes rows that were successfully processed before <paramref name="olderThan"/> ago,
        /// preventing unbounded table growth.
        /// </summary>
        Task PurgeProcessedAsync(TimeSpan olderThan, CancellationToken cancellationToken = default);
    }

    /// <summary>A lightweight projection of a pending outbox row for processor consumption.</summary>
    public sealed record OutboxMessageEnvelope(Guid Id, string RouteKey, string Payload);
}
