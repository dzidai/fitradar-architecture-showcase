using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fitradar.Application.Persistence.Dao
{
    /// <summary>
    /// Manages device push notification registrations at the persistence level.
    /// Used by application-layer handlers to keep FCM token records consistent
    /// with Firebase's device registration state.
    /// </summary>
    public interface IDeviceNotificationDao
    {
        /// <summary>
        /// Removes all device registrations whose FCM tokens Firebase has reported
        /// as unregistered (device logged out or app reinstalled).
        /// </summary>
        Task DeleteExpiredAsync(
            IReadOnlyList<string> expiredFcmTokens,
            CancellationToken cancellationToken = default);
    }
}
