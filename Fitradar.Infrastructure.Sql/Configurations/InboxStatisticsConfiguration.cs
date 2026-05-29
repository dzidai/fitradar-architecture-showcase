using Fitradar.Application.Contracts.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fitradar.Infrastructure.Sql.Configurations
{
    public class InboxStatisticsConfiguration : IEntityTypeConfiguration<InboxStatisticsDbModel>
    {
        public void Configure(EntityTypeBuilder<InboxStatisticsDbModel> builder)
        {
            
        }
    }
}
