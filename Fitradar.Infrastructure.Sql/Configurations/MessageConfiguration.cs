using Fitradar.Application.Contracts.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fitradar.Infrastructure.Sql.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<MessageDbModel>
    {
        public void Configure(EntityTypeBuilder<MessageDbModel> builder)
        {
            builder.HasKey(app => app.Id).HasName("PK_Message");
            builder
                .HasOne(msg => msg.TriggeredBy)
                .WithMany(user => user.TriggeredMessages)
                .HasForeignKey(msg => msg.TriggeredById)
                .HasConstraintName("FK_Messages_User_ID")
                .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasOne(msg => msg.Receiver)
                .WithMany(user => user.ReceivedMessages)
                .HasForeignKey(msg => msg.ReceiverId)
                .HasConstraintName("FK_Messages_Receiver_ID")
                .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasOne(msg => msg.ArchivedSportEvent)
                .WithMany(se => se.InboxMessages)
                .HasForeignKey(msg => msg.ArchivedSportEventId)
                .HasConstraintName("FK_Messages_ArchivedSportEvent_ID")
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasOne(msg => msg.SportEventInstance)
                .WithMany(se => se.InboxMessages)
                .HasForeignKey(msg => msg.SportEventInstanceId)
                .HasConstraintName("FK_Messages_SportEventInstance_ID")
                .OnDelete(DeleteBehavior.SetNull);  // this is the workaround in order to make FitradarStore update function update SportEventInstanceId = null. In db it should be Cascade
        }
    }
}
