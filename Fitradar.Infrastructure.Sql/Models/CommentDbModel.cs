using Fitradar.Application.Contracts.Persistence.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitradar.Infrastructure.Sql.Models
{
    [Table("Comments")]
    public class CommentDbModel
    {
        [Column("ID")]
        public Guid Id { get; set; }

        [Column("SportEventInstanceID")]
        public long? SportEventInstanceId { get; set; }
        public SportEventInstanceDbModel SportEventInstance { get; set; }

        [Column("ArchivedSportEventID")]
        public Guid? ArchivedSportEventId { get; set; }
        public ArchivedSportEventDbModel ArchivedSportEvent { get; set; }

        [Required]
        public string CommentText { get; set; }

        public DateTime PostedAt { get; set; }

        [Required]
        [Column("PostedBy")]
        public string PostedById { get; set; }

        public UserDbModel PostedBy { get; set; }
    }
}
