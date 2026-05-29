namespace Fitradar.Web.Api.Authorization
{
    public class FitradarPolicies
    {
        public const string OnlyRegisteredUsers = "OnlyRegisteredUsers";
        public const string OnlyVerifiedByFitRadar = "OnlyVerifiedByFitRadar";
        public const string OnlyVerifiedByStripe = "OnlyVerifiedByStripe";
        public const string OnlyBookingOwner = "OnlyBookingOwner";
        public const string OnlyEventOwner = "OnlyEventOwner";
        public const string OnlyFeedbackOwner = "OnlyFeedbackOwner";
        public const string OnlyEnabledUsers = "OnlyEnabledUsers";
    }
}
