using Fitradar.Application.Contracts.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fitradar.Infrastructure.Sql.Configurations
{
    public class ArchivedSportEventConfiguration : IEntityTypeConfiguration<ArchivedSportEventDbModel>
    {
        public void Configure(EntityTypeBuilder<ArchivedSportEventDbModel> builder)
        {
            builder
                .HasKey(i => i.Id)
                .HasName("PK_ArchivedSportEvent_ID");
        }
    }
}
