using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fitradar.Application.Persistence.Dao;

/// <summary>
/// Returns pre-loaded host data for a sport event instance.
/// Used by Azure Functions to hydrate in-process integration events
/// (comment added, like added, follower added, ...)
/// </summary>
public interface ISportEventNotificationDao
{
    Task<SportEventHostNotificationData> GetHostNotificationDataAsync(
        Guid sportEventInstancePublicId,
        CancellationToken cancellationToken = default);
}

public sealed record SportEventHostNotificationData(
    string HostId,
    string HostUserName,
    Guid SportEventPublicId,
    Guid SportEventId,
    string SportEventTitle,
    string[] HostFcmTokens);
