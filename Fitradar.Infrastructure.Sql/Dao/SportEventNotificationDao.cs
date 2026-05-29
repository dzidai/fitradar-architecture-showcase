using Fitradar.Application.Persistence.Dao;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fitradar.Infrastructure.Sql.Dao
{
    public class SportEventNotificationDao : ISportEventNotificationDao
    {
        private readonly FitradarDbContext _dbContext;

        public SportEventNotificationDao(FitradarDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SportEventHostNotificationData> GetHostNotificationDataAsync(
            Guid sportEventInstancePublicId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.SportEventInstances
                .AsNoTracking()
                .Where(e => e.PublicId == sportEventInstancePublicId)
                .Select(e => new SportEventHostNotificationData(
                    e.SportEvent.CreatedById,
                    e.SportEvent.CreatedBy.UserName,
                    e.PublicId,
                    e.SportEventId,
                    e.SportEvent.Title,
                    e.SportEvent.CreatedBy.Devices.Select(d => d.FcmToken).ToArray()))
                .SingleAsync(cancellationToken);
        }
    }
}
