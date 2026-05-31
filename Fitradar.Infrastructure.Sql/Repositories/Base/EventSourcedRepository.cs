using Fitradar.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fitradar.Infrastructure.Sql.Repositories.Base
{
    public abstract class EventSourcedRepository<TEntity> : IEventFlushable where TEntity : class, IEventSourcedEntity
    {
        private readonly IMediator _mediator;

        protected HashSet<TEntity> UnpublishedEntities { get; }
        protected FitradarDbContext DbContext { get; }

        public EventSourcedRepository(FitradarDbContext context, IMediator mediator)
        {
            UnpublishedEntities = new HashSet<TEntity>();
            DbContext = context ?? throw new ArgumentNullException(nameof(context));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        // Persists domain state, dispatches domain events so their side-effects
        // (calendar entries, stat updates) are written in the same transaction,
        // then commits everything atomically.
        //
        // NOTE: domain event handlers must only write to the DbContext. Handlers that
        // call external services (Service Bus, email, push) must be migrated to
        // integration event handlers
        public async Task SaveAndPublishEventsAsync()
        {
            if (DbContext.Database.CurrentTransaction == null)
            {
                using var transaction = await DbContext.Database.BeginTransactionAsync();
                try
                {
                    // Save domain state first so DB-generated IDs are available to event handlers.
                    await DbContext.SaveChangesAsync();

                    await DispatchPendingEventsAsync();

                    // Flush side-effects written by domain event handlers.
                    await DbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            else
            {
                // Nested call — already inside a transaction started by an outer repository
                // (e.g. a domain event handler that saves additional aggregates). Participate
                // in the existing transaction without wrapping a new one.
                await DbContext.SaveChangesAsync();
                await DispatchPendingEventsAsync();
                await DbContext.SaveChangesAsync();
            }
        }

        public async Task PublishEventsAndSaveAsync(Action postPublishSaveAction)
        {
            if (DbContext.Database.CurrentTransaction == null)
            {
                using var transaction = await DbContext.Database.BeginTransactionAsync();
                try
                {
                    await DispatchPendingEventsAsync();

                    postPublishSaveAction.Invoke();

                    await DbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            else
            {
                await DispatchPendingEventsAsync();
                postPublishSaveAction.Invoke();
                await DbContext.SaveChangesAsync();
            }
        }

        // Snapshots all pending events, clears the tracking set, then publishes.
        // Snapshotting before clearing prevents InvalidOperationException from
        // modifying UnpublishedEntities while it is being enumerated, and ensures
        // any new entities enqueued by event handlers are not lost.
        private async Task DispatchPendingEventsAsync()
        {
            var entitiesToFlush = UnpublishedEntities.ToList();

            var eventsToPublish = entitiesToFlush
                .SelectMany(e => e.DomainEvents)
                .ToList();

            foreach (var entity in entitiesToFlush)
            {
                entity.ClearPendingEvents();
            }

            UnpublishedEntities.Clear();

            foreach (var domainEvent in eventsToPublish)
            {
                await _mediator.Publish(domainEvent);
            }
        }
    }
}
