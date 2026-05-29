using MediatR;

namespace Fitradar.Application.Common
{
    public interface IReadStoreCommand : IRequest;

    public interface IReadStoreCommand<out TResponse> : IRequest<TResponse>;
}
