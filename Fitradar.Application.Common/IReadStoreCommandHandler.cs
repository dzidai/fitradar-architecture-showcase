using MediatR;

namespace Fitradar.Application.Common
{
    public interface IReadStoreCommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IReadStoreCommand<TResponse>
    {
    }

    public interface IReadStoreCommandHandler<in TRequest> : IRequestHandler<TRequest>
        where TRequest : IReadStoreCommand
    {
    }
}
