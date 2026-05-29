using Fitradar.Application.Contracts.Persistence.Models;
using Fitradar.Application.Persistence.Dao;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fitradar.Infrastructure.Sql.Dao
{
    public sealed class DeviceNotificationDao : IDeviceNotificationDao
    {
        private readonly FitradarDbContext _dbContext;

        public DeviceNotificationDao(FitradarDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteExpiredAsync(
            IReadOnlyList<string> expiredFcmTokens,
            CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<SignedInDeviceDbModel>()
                .Where(d => expiredFcmTokens.Contains(d.FcmToken))
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}
