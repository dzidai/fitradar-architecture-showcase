using Fitradar.Infrastructure.Sql.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fitradar.Infrastructure.Sql.Configurations
{
    public class SportEventInstanceConfiguration : IEntityTypeConfiguration<SportEventInstanceDbModel>
    {
        public void Configure(EntityTypeBuilder<SportEventInstanceDbModel> builder)
        {
            builder
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();
            builder
                .HasKey(i => i.Id)
                .HasName("PK_SportEventInstance_ID");
            builder
                .HasAlternateKey(i => i.PublicId)
                .HasName("AK_SportEventInstance_PublicID");
            builder
                .HasOne(i => i.SportEvent)
                .WithMany(e => e.SportEventInstances)
                .HasForeignKey(i => i.SportEventId)
                .HasConstraintName("FK_SportEventInstances_SportEvent_ID");
        }
    }
}
