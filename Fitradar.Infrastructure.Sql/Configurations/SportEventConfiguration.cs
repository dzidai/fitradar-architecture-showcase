using Fitradar.Application.Contracts.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fitradar.Infrastructure.Sql.Configurations
{
    public class SportEventConfiguration : IEntityTypeConfiguration<SportEventDbModel>
    {
        public void Configure(EntityTypeBuilder<SportEventDbModel> builder)
        {
            builder.ToTable(b => b.HasTrigger("TR_SportEvents_UpdatePostedTickets"));
            builder.HasKey(app => app.Id).HasName("PK_SportEvent");
        }
    }
}
