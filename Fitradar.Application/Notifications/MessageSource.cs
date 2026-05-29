namespace Fitradar.Application.Notifications;

public enum MessageSource
{
    None = 0,
    Like = 1,
    Unlike = 2,
    Comment = 3,
    Share = 4,
    FollowEvent = 5,
    UnfollowEvent = 6,
    FollowUser = 7,
    UnfollowUser = 8,
    Book = 9,
    CancelBooking = 10,
    CancelEvent = 11,
    NewFeedback = 12,
    NewRating = 13,
    VerifiedByStripe = 14,
    RejectedByStripe = 15,
    NewEvent = 16,
    UpdatedEvent = 17
}
