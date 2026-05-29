using Fitradar.Infrastructure.Sql.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitradar.Application.Contracts.Persistence.Models
{
    public class UserDbModel
    {
        [Required]
        [MaxLength(450)]
        [Column("ID")]
        public string Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Email { get; set; }

        [MaxLength(256)]
        public string UserName { get; set; }

        [MaxLength(512)]
        public string FullName { get; set; }

        public string AboutUser { get; set; }


        public IList<CommentDbModel> PostedComments { get; set; } = [];

        public IList<MessageDbModel> TriggeredMessages { get; set; } = [];

        public IList<MessageDbModel> ReceivedMessages { get; set; } = [];

        public IList<SignedInDeviceDbModel> Devices { get; set; } = [];

        [Required]
        public DateTime RegisteredAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

    }
}
