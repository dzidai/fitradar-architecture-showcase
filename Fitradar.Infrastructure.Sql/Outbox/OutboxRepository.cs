using Fitradar.Application.Common;
using Fitradar.Infrastructure.Sql.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fitradar.Infrastructure.Sql.Outbox
{
    public sealed class OutboxRepository(FitradarDbContext dbContext) : IOutboxRepository
    {
        /// <summary>
        /// Messages that have failed this many times are excluded from future polls and
        /// treated as dead-lettered. Visible here so callers can surface the threshold in
        /// monitoring / alerting queries.
        /// </summary>
        public const int MaxRetryCount = 5;

        private readonly FitradarDbContext _dbContext = dbContext;

        public void Add(string routeKey, string payload)
        {
            _dbContext.Outbox.Add(new OutboxMessageDbModel
            {
                Id = Guid.NewGuid(),
                RouteKey = routeKey,
                Payload = payload,
                CreatedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Fetches up to <paramref name="batchSize"/> unprocessed messages using
        /// <c>WITH (UPDLOCK, READPAST)</c>:
        /// <list type="bullet">
        ///   <item><c>UPDLOCK</c> — promotes the shared read lock to an update lock so no
        ///   other reader can also lock the same rows.</item>
        ///   <item><c>READPAST</c> — skips rows already locked by another transaction
        ///   instead of blocking, giving each processor instance a disjoint set.</item>
        /// </list>
        /// Rows whose <c>RetryCount</c> has reached <see cref="MaxRetryCount"/> are
        /// excluded and treated as dead-lettered.
        /// </summary>
        public async Task<IReadOnlyList<OutboxMessageEnvelope>> GetPendingAsync(
            int batchSize,
            CancellationToken cancellationToken = default)
        {
            var batchSizeParam = new SqlParameter("@batchSize", batchSize);
            var maxRetryParam = new SqlParameter("@maxRetry", MaxRetryCount);

            return await _dbContext.Outbox
                .FromSqlRaw(
                    """
                    SELECT TOP (@batchSize) *
                    FROM OutboxMessages WITH (UPDLOCK, READPAST)
                    WHERE ProcessedAt IS NULL
                      AND RetryCount < @maxRetry
                    ORDER BY CreatedAt
                    """,
                    batchSizeParam, maxRetryParam)
                .AsNoTracking()
                .Select(m => new OutboxMessageEnvelope(m.Id, m.RouteKey, m.Payload))
                .ToListAsync(cancellationToken);
        }

        public async Task MarkProcessedAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _dbContext.Outbox
                .Where(m => m.Id == id)
                .ExecuteUpdateAsync(
                    s => s.SetProperty(m => m.ProcessedAt, DateTime.UtcNow),
                    cancellationToken);
        }

        public async Task MarkProcessedBatchAsync(IReadOnlyList<Guid> ids, CancellationToken cancellationToken = default)
        {
            await _dbContext.Outbox
                .Where(m => ids.Contains(m.Id))
                .ExecuteUpdateAsync(
                    s => s.SetProperty(m => m.ProcessedAt, DateTime.UtcNow),
                    cancellationToken);
        }

        /// <summary>
        /// Records a dispatch failure, increments <c>RetryCount</c>, and stores the last
        /// error. Once <c>RetryCount</c> reaches <see cref="MaxRetryCount"/> the row is
        /// excluded from future polls (dead-lettered).
        /// </summary>
        public async Task MarkFailedAsync(Guid id, string error, CancellationToken cancellationToken = default)
        {
            var truncated = error.Length > 2000 ? error[..2000] : error;
            await _dbContext.Outbox
                .Where(m => m.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(m => m.Error, truncated)
                    .SetProperty(m => m.RetryCount, m => m.RetryCount + 1),
                    cancellationToken);
        }

        public async Task PurgeProcessedAsync(TimeSpan olderThan, CancellationToken cancellationToken = default)
        {
            var cutoff = DateTime.UtcNow - olderThan;
            await _dbContext.Outbox
                .Where(m => m.ProcessedAt != null && m.ProcessedAt < cutoff)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}
