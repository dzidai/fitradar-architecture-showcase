using Fitradar.Infrastructure.Sql.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitradar.Application.Contracts.Persistence.Models
{
    [Table("InboxMessages")]
    public class MessageDbModel
    {
        [Column("ID")]
        [Key]
        public Guid Id { get; set; }

        [Column("ReceiverID")]
        [Required]
        [MaxLength(450)]
        public string ReceiverId { get; set; }

        public UserDbModel Receiver { get; set; }

        [Column("SportEventPublicID")]
        public Guid? SportEventPublicId { get; set; }

        [Column("ArchivedSportEventID")]
        public Guid? ArchivedSportEventId { get; set; }
        public ArchivedSportEventDbModel ArchivedSportEvent { get; set; }

        [Column("SportEventInstanceID")]
        public long? SportEventInstanceId { get; set; }
        public SportEventInstanceDbModel SportEventInstance { get; set; }

        public string NavigationLink { get; set; }

        public int Source { get; set; }

        [MaxLength(450)]
        [Column("TriggeredBy")]
        public string TriggeredById { get; set; }

        public UserDbModel TriggeredBy { get; set; }

        [Column("AvatarID")]
        public string AvatarId { get; set; }    // it is SportEvents.ID or Users.ID

        public DateTime CreatedAt { get; set; }
    }
}
