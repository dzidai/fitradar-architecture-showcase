using Fitradar.Application.Contracts.Persistence.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitradar.Infrastructure.Sql.Models
{
    [Table("SportEventInstances")]
    public class SportEventInstanceDbModel
    {
        public SportEventInstanceDbModel()
        {
            PublicId = Guid.NewGuid();
        }

        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("PublicID")]
        public Guid PublicId { get; set; }

        [Column("SportEventID")]
        public Guid SportEventId { get; set; }

        public SportEventDbModel SportEvent { get; set; }

        public IList<CommentDbModel> Comments { get; set; } = [];

        public IList<MessageDbModel> InboxMessages { get; } = [];
    }
}
