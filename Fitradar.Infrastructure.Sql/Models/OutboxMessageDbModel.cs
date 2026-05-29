using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitradar.Infrastructure.Sql.Models
{
    [Table("OutboxMessages")]
    public class OutboxMessageDbModel
    {
        [Column("ID")]
        public Guid Id { get; set; }

        /// <summary>
        /// Service Bus route key — matches <c>ServiceBusRouteKeys.For&lt;TEvent&gt;()</c>
        /// (i.e. the event CLR type name). Used by <c>OutboxProcessor</c> to resolve the
        /// target queue without deserializing the payload.
        /// </summary>
        [Required]
        [MaxLength(256)]
        [Column("RouteKey")]
        public string RouteKey { get; set; }

        /// <summary>JSON-serialized event payload, forwarded verbatim to the Service Bus queue.</summary>
        [Required]
        [Column("Payload", TypeName = "nvarchar(max)")]
        public string Payload { get; set; }

        public DateTime CreatedAt { get; set; }

        /// <summary>Set by the processor once the message has been successfully enqueued.</summary>
        public DateTime? ProcessedAt { get; set; }

        /// <summary>Populated on dispatch failure. Cleared on each retry attempt.</summary>
        [MaxLength(2000)]
        public string Error { get; set; }

        /// <summary>
        /// Number of times dispatch was attempted and failed. Rows with
        /// <see cref="RetryCount"/> &gt;= <c>MaxRetryCount</c> are excluded from
        /// <c>GetPendingAsync</c> and treated as dead-lettered.
        /// </summary>
        public int RetryCount { get; set; }
    }
}
