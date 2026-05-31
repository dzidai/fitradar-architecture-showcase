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
    public class EventInstanceRepository : EventSourcedRepository<WorkoutOccurrence>, IEventInstanceRepository
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
            WorkoutOccurrence updatedWorkoutOccurrence,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentNullException.ThrowIfNull(updatedWorkoutOccurrence);

            var storedDbModel = DbContext.Set<SportEventInstanceDbModel>().Find(updatedWorkoutOccurrence.Id);
            storedDbModel.SportEventId = updatedWorkoutOccurrence.SportEventId;

            var entityTracked = DbContext.Entry(storedDbModel);
            var previousState = entityTracked.State;
            DbContext.Update(storedDbModel);
            if (previousState == EntityState.Added)
            {
                DbContext.Entry(storedDbModel).State = EntityState.Added;
            }

            base.UnpublishedEntities.Add(updatedWorkoutOccurrence);

           await base.SaveAndPublishEventsAsync();
        }

        public async Task DeleteAsync(
            WorkoutOccurrence workoutOccurrence,
            bool commitChanges = true,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (workoutOccurrence == null)
            {
                throw new ArgumentNullException(nameof(WorkoutOccurrence));
            }

            workoutOccurrence.AddPendingEvent(new WorkoutOccurrenceDeleted(workoutOccurrence));
            base.UnpublishedEntities.Add(workoutOccurrence);

            var dbModel = await DbContext.SportEventInstances.SingleOrDefaultAsync(si => si.Id == workoutOccurrence.Id);


            await base.PublishEventsAndSaveAsync(() => DbContext.SportEventInstances.Remove(dbModel));
        }

        public Task<bool> ExistsAsync(Guid publicId, CancellationToken cancellationToken = default)
        {
            return DbContext.SportEventInstances.AnyAsync(si => si.PublicId == publicId, cancellationToken);
        }
    }
}
