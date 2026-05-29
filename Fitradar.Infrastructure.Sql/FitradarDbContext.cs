using Fitradar.Application.Contracts.Persistence.Models;
using Fitradar.Infrastructure.Sql.Models;
using Microsoft.EntityFrameworkCore;

namespace Fitradar.Infrastructure.Sql
{
    public partial class FitradarDbContext : DbContext
    {
        public DbSet<SportEventInstanceDbModel> SportEventInstances { get; set; }
        public DbSet<ArchivedSportEventDbModel> ArchivedSportEvents { get; set; }
        public DbSet<CommentDbModel> Comments { get; set; }
        public DbSet<MessageDbModel> Messages { get; set; }
        public DbSet<InboxStatisticsDbModel> InboxStatistics { get; set; }
        public DbSet<OutboxMessageDbModel> Outbox { get; set; }

        public FitradarDbContext(DbContextOptions<FitradarDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(FitradarDbContext).Assembly);
        }
    }
}
