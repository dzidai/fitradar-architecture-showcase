using Fitradar.Infrastructure.Sql.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitradar.Application.Contracts.Persistence.Models
{
    [Table("ArchivedSportEvents")]
    public class ArchivedSportEventDbModel
    {
        [Column("ID")]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool HasImage { get; set; }

        public int NumberOfTickets { get; set; }

        public int NumberOfComments { get; set; }

        public long StartTimeMillis { get; set; }

        public long EndTimeMillis { get; set; }

        public string CreatedFrom { get; set; }

        [Column("CreatedBy")]
        public string CreatedById { get; set; }

        // In case user is deleted we change user's Name and Username, we need to follow those changes
        [Required]
        public UserDbModel CreatedBy { get; set; }

        public DateTime PublishedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime ArchivedAt { get; set; }

        public IList<CommentDbModel> Comments { get; } = [];

        public IList<MessageDbModel> InboxMessages { get; } = [];
    }
}
