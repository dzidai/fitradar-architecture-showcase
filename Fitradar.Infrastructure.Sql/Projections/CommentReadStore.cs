using Fitradar.Application.Common;
using Fitradar.Application.Common.Exceptions;
using Fitradar.Application.Persistence.ReadStores;
using Fitradar.Application.Persistence.ReadStores.Models;
using Fitradar.Application.UseCases.Reactions.OutboundEvents;
using Fitradar.Infrastructure.Sql.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Fitradar.Infrastructure.Sql.Projections
{
    public sealed class CommentReadStore(FitradarDbContext dbContext, IOutboxRepository outboxRepository) : ICommentReadStore
    {
        private readonly FitradarDbContext _dbContext = dbContext;
        private readonly IOutboxRepository _outboxRepository = outboxRepository;

        public async Task<Guid> CreateAsync(CommentReadModel comment, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(comment);

            var sportEventInstance = await _dbContext.SportEventInstances
                .FirstOrDefaultAsync(instance => instance.PublicId == comment.SportEventInstancePublicId, cancellationToken);

            Guid? archivedSportEventId = null;
            long? sportEventInstanceId = null;

            if (sportEventInstance != null)
            {
                sportEventInstanceId = sportEventInstance.Id;
            }
            else
            {
                var archivedSportEvent = await _dbContext.ArchivedSportEvents
                    .FirstOrDefaultAsync(instance => instance.Id == comment.SportEventInstancePublicId, cancellationToken);

                if (archivedSportEvent == null)
                {
                    throw new NotFoundException(nameof(Domain.Workout.SportEventInstance), comment.SportEventInstancePublicId);
                }

                archivedSportEventId = comment.SportEventInstancePublicId;
            }

            var commentDbModel = new CommentDbModel
            {
                Id = Guid.NewGuid(),
                CommentText = comment.CommentText,
                PostedById = comment.PostedById,
                PostedAt = comment.PostedAt,
                SportEventInstanceId = sportEventInstanceId,
                ArchivedSportEventId = archivedSportEventId
            };

            _dbContext.Comments.Add(commentDbModel);

            var outboundEvent = new CommentAdded(commentDbModel.Id, comment.PostedById, comment.SportEventInstancePublicId);
            _outboxRepository.Add(nameof(CommentAdded), JsonSerializer.Serialize(outboundEvent));

            await _dbContext.SaveChangesAsync(cancellationToken);

            return commentDbModel.Id;
        }
    }
}
