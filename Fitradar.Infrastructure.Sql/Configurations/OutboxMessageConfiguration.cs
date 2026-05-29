using Fitradar.Infrastructure.Sql.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fitradar.Infrastructure.Sql.Configurations
{
    public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessageDbModel>
    {
        public void Configure(EntityTypeBuilder<OutboxMessageDbModel> builder)
        {
            builder.HasKey(m => m.Id).HasName("PK_OutboxMessage");

            builder.Property(m => m.Id).HasColumnName("ID");
            builder.Property(m => m.RouteKey).IsRequired().HasMaxLength(256);
            builder.Property(m => m.Payload).IsRequired().HasColumnType("nvarchar(max)");
            builder.Property(m => m.Error).HasMaxLength(2000);

            // The processor fetches WHERE ProcessedAt IS NULL — keep this selective.
            builder.HasIndex(m => m.ProcessedAt).HasDatabaseName("IX_OutboxMessages_ProcessedAt");
        }
    }
}
