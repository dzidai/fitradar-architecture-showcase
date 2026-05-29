using Fitradar.Infrastructure.Sql.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fitradar.Infrastructure.Sql.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<CommentDbModel>
    {
        public void Configure(EntityTypeBuilder<CommentDbModel> builder)
        {
            builder.ToTable(b => b.HasTrigger("TR_Comments_UpdateStatistics"));
            builder
                .HasKey(comment => comment.Id)
                .HasName("PK_Comment");
        }
    }
}
