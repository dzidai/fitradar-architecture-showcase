using AutoMapper;
using Fitradar.Application.Persistence.Repositories;
using Fitradar.Domain.Workout;
using Fitradar.Domain.Workout.Events;
using Fitradar.Infrastructure.Sql.Models;
using Fitradar.Infrastructure.Sql.Repositories.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fitradar.Infrastructure.Sql.Repositories
{
    public class EventInstanceRepository : EventSourcedRepository<SportEventInstance>, IEventInstanceRepository
    {
        private readonly IMapper _mapper;

        public EventInstanceRepository(
            FitradarDbContext context,
            IMapper mapper,
            IMediator mediator)
            : base(context, mediator)
        {
            _mapper = mapper;
        }

        public async Task UpdateAsync(
            SportEventInstance updatedSportEventInstance,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentNullException.ThrowIfNull(updatedSportEventInstance);

            var storedDbModel = DbContext.Set<SportEventInstanceDbModel>().Find(updatedSportEventInstance.Id);
            storedDbModel.SportEventId = updatedSportEventInstance.SportEventId;

            var entityTracked = DbContext.Entry(storedDbModel);
            var previousState = entityTracked.State;
            DbContext.Update(storedDbModel);
            if (previousState == EntityState.Added)
            {
                DbContext.Entry(storedDbModel).State = EntityState.Added;
            }

            base.UnpublishedEntities.Add(updatedSportEventInstance);

           await base.SaveAndPublishEventsAsync();
        }

        public async Task DeleteAsync(
            SportEventInstance sportEventInstance,
            bool commitChanges = true,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (sportEventInstance == null)
            {
                throw new ArgumentNullException(nameof(SportEventInstance));
            }

            sportEventInstance.AddPendingEvent(new SportEventInstanceDeleted(sportEventInstance));
            base.UnpublishedEntities.Add(sportEventInstance);

            var dbModel = await DbContext.SportEventInstances.SingleOrDefaultAsync(si => si.Id == sportEventInstance.Id);


            await base.PublishEventsAndSaveAsync(() => DbContext.SportEventInstances.Remove(dbModel));
        }

        public Task<bool> ExistsAsync(Guid publicId, CancellationToken cancellationToken = default)
        {
            return DbContext.SportEventInstances.AnyAsync(si => si.PublicId == publicId, cancellationToken);
        }
    }
}
