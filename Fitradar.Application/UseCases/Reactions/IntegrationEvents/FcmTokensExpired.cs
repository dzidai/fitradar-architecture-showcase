using Fitradar.Application.Common;
using Fitradar.Application.Persistence.Dao;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fitradar.Application.UseCases.Reactions.IntegrationEvents;

/// <summary>
/// Raised whenever Firebase reports one or more FCM tokens as unregistered.
/// A single dedicated handler removes them from the device store regardless
/// of which push notification triggered the discovery.
/// </summary>
public sealed class FcmTokensExpired : IIntegrationEvent
{
    public required IReadOnlyList<string> FcmTokens { get; init; }


    public class RemoveExpiredTokens : IIntegrationEventHandler<FcmTokensExpired>
    {
        private readonly IDeviceNotificationDao _deviceNotificationDao;
        private readonly ILogger<RemoveExpiredTokens> _logger;

        public RemoveExpiredTokens(
            IDeviceNotificationDao deviceNotificationDao,
            ILogger<RemoveExpiredTokens> logger)
        {
            _deviceNotificationDao = deviceNotificationDao;
            _logger = logger;
        }

        public async Task Handle(FcmTokensExpired notification, CancellationToken cancellationToken)
        {
            await _deviceNotificationDao.DeleteExpiredAsync(notification.FcmTokens, cancellationToken);

            _logger.LogInformation(
                "Removed {Count} expired FCM token(s) from device store",
                notification.FcmTokens.Count);
        }
    }
}
