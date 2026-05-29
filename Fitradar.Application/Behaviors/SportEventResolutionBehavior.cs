using Fitradar.Application.Common.Exceptions;
using Fitradar.Application.Persistence.Repositories;
using Fitradar.Domain.Workout;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Fitradar.Application.Behaviors;

public class SportEventResolutionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, ISportEventInteractionCommand
{
        private readonly IEventInstanceRepository _repository;

        public SportEventResolutionBehavior(IEventInstanceRepository repository)
        {
            _repository = repository;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var exists = await _repository.ExistsAsync(request.SportEventInstancePublicId, cancellationToken);

            if (!exists)
            {
                throw new NotFoundException(nameof(SportEventInstance), request.SportEventInstancePublicId);
            }

            return await next();
        }
}
