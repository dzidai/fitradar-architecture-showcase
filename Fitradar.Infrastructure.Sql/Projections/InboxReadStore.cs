using Fitradar.Application.Persistence.ReadStores;
using Fitradar.Application.Persistence.ReadStores.Models;
using Fitradar.Application.Contracts.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fitradar.Infrastructure.Sql.Projections
{
    public sealed class InboxReadStore : IInboxReadStore
    {
        private readonly FitradarDbContext _dbContext;

        public InboxReadStore(FitradarDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpsertAsync(InboxMessageReadModel message, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(message);

            var existingMessage = await _dbContext.Messages
                .FirstOrDefaultAsync(
                    msg => msg.NavigationLink == message.NavigationLink
                           && msg.ReceiverId == message.ReceiverId
                           && msg.TriggeredById == message.TriggeredById
                           && msg.Source == (int)message.Source,
                    cancellationToken);

            if (existingMessage != null)
            {
                existingMessage.CreatedAt = message.CreatedAt;
                _dbContext.Update(existingMessage);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return;
            }

            var sportEventInstance = message.SportEventPublicId.HasValue
                ? await _dbContext.SportEventInstances
                    .FirstOrDefaultAsync(e => e.PublicId == message.SportEventPublicId.Value, cancellationToken)
                : null;

            var archivedSportEvent = message.SportEventPublicId.HasValue
                ? await _dbContext.ArchivedSportEvents
                    .FirstOrDefaultAsync(e => e.Id == message.SportEventPublicId.Value, cancellationToken)
                : null;

            var newMessage = new MessageDbModel
            {
                Id = Guid.NewGuid(),
                ReceiverId = message.ReceiverId,
                NavigationLink = message.NavigationLink,
                Source = (int)message.Source,
                TriggeredById = message.TriggeredById,
                SportEventPublicId = message.SportEventPublicId,
                ArchivedSportEventId = archivedSportEvent?.Id,
                SportEventInstanceId = sportEventInstance?.Id,
                AvatarId = message.AvatarId,
                CreatedAt = message.CreatedAt
            };

            _dbContext.Messages.Add(newMessage);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
