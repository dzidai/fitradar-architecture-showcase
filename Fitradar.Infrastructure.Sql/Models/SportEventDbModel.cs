using Fitradar.Infrastructure.Sql.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitradar.Application.Contracts.Persistence.Models
{
    [Table("SportEvents")]
    public class SportEventDbModel
    {
        [Column("ID")]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        public IList<SportEventInstanceDbModel> SportEventInstances { get; set; }

        public int NumberOfTickets { get; set; }

        public int Version { get; set; }

        public string CreatedFrom { get; set; }


        [Column("CreatedBy")]
        public string CreatedById { get; set; }

        [Required]
        public UserDbModel CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
