using CSharpFunctionalExtensions;
using Fitradar.Application.Services.Dto;
using System.Threading;
using System.Threading.Tasks;

namespace Fitradar.Application.Services;

public interface IPushMessagingService
{
    Task<Result<PushMessageResult>> SendPushMessage(
        PushMessage message,
        CancellationToken cancellationToken = default);
}
