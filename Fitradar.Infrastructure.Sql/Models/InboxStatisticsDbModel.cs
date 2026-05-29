using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitradar.Application.Contracts.Persistence.Models
{
    [Table("InboxStatistics")]
    public class InboxStatisticsDbModel
    {
        [Column("UserID")]
        [Key]
        public string UserId { get; set; }

        public UserDbModel User { get; set; }

        [NotMapped]
        public int TotalMessages => NumberOfLikeMessages + NumberOfDislikeMessages +
                                    NumberOfEventFollowerMessages + NumberOfEventUnFollowerMessages +
                                    NumberOfUserFollowerMessages + NumberOfUserUnFollowerMessages +
                                    NumberOfCommentMessages + NumberOfShareMessages +
                                    NumberOfBookingMessages + NumberOfCancelMessages +
                                    NumberOfSystemMessages;

        [Column("LikeMessages")]
        public int NumberOfLikeMessages { get; set; }

        [Column("DislikeMessages")]
        public int NumberOfDislikeMessages { get; set; }

        [Column("CommentMessages")]
        public int NumberOfCommentMessages { get; set; }

        [Column("ShareMessages")]
        public int NumberOfShareMessages { get; set; }

        [Column("FollowEventMessages")]
        public int NumberOfEventFollowerMessages { get; set; }

        [Column("UnFollowEventMessages")]
        public int NumberOfEventUnFollowerMessages { get; set; }

        [Column("FollowUserMessages")]
        public int NumberOfUserFollowerMessages { get; set; }

        [Column("UnFollowUserMessages")]
        public int NumberOfUserUnFollowerMessages { get; set; }

        [Column("BookingMessages")]
        public int NumberOfBookingMessages { get; set; }

        [Column("CancelMessages")]
        public int NumberOfCancelMessages { get; set; }

        [Column("SystemMessages")]
        public int NumberOfSystemMessages { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int UpdatedBy { get; set; }
    }
}
