namespace Fitradar.Domain
{
    public class ValidationErrorCodes
    {
        // Domain Model validation error codes
        public const string RESERVATION_EXPIRED = "1001";
        public const string RESERVATION_IS_ALREADY_CONFIRMED = "1002";
        public const string BOOKING_CANCELLATION_TOO_LATE = "1003";
        public const string NO_FREE_SEATS = "1004";
        public const string BOOKING_TOO_LATE = "1005";
        public const string USER_BOOKED_IN_CALENDAR = "1006";
        public const string ORGANIZER_HAS_NO_PAYOUT = "1007";
        public const string FEEDBACK_TOO_EARLY = "1008";
        public const string FEEDBACK_ONLY_BY_VISITORS = "1009";
        public const string UPDATE_ONLY_BEFORE_BOOKED_SEATS = "1010";
        public const string UPDATE_ONLY_NOT_CANCELLED = "1011";
        public const string CANCEL_BOOKING_ONLY_NOT_CANCELLED = "1012";
        public const string USERNAME_IS_TAKEN = "1013";
        public const string EVENT_START_TIME_TOO_EARLY = "1014";
        public const string COMPLAINT_TOO_EARLY = "1015";
        public const string COMPLAINT_ONLY_BY_VISITORS = "1016";
        public const string FEEDBACK_UPDATE_TOO_LATE = "1017";
        public const string COMPLAINT_TOO_LATE = "1018";
        public const string NEW_FEEDBACK_TOO_LATE = "1019";
        public const string USERNAME_CONTAINS_RESERVED_WORD = "1020";
        public const string USERNAME_CONTAINS_BANNED_WORD = "1021";
        public const string REPORT_ON_EVENT_TO_LATE = "1022";
        public const string MORE_THAN_ONE_FEEDBACK_FROM_VISITOR = "1023";
        public const string MAX_NUMBER_OF_TICKETS_EXCEEDED = "1024";
        public const string EVENT_START_DATE_TOO_BIG = "1025";
        public const string EVENT_PRICE_TOO_SMALL = "1026";
        public const string EVENT_DURATION_TOO_BIG = "1027";
        public const string USERNAME_HAS_NON_ENGLISH_CHARACTERS = "1028";
        public const string EVENT_CANCELLATION_TOO_LATE = "1029";
        public const string NO_PAYMENT_IS_RECEIVED = "1030";
        public const string USER_BUSY_DURING_WORKOUT = "1031";
        public const string CANCEL_ONLY_CONFIRMED_BOOKING = "1032";
        public const string HOST_DELETED = "1033";
    }
}
