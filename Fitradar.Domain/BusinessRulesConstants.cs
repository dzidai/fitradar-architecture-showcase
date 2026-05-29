using System.Collections.Generic;

namespace Fitradar.Domain
{
    public class BusinessRulesConstants
    {
        // Business Rules
        public const decimal FITRADAR_COMISSION_FEE = 0.15m;                        // the Fee is expressed in precents from the received payments amount
        public const decimal INSTANT_PAYOUT_FEE = 0.15m;                            // the Fee is expressed in precents from the payout amount
        public const decimal PARTIAL_REFUND_FEE = 0.5m;                             // the Fee is expressed in precents from the price of the workout
        public const decimal MAX_FEE_COVERED_BY_FITRADAR = 50.0m;                   // the amount of money in euros that FitRadar is ready to pay for Stipe transaction fees, when host cancels workout
        public const int MINUTES_BEFOR_APPLY_FULL_CHARGE_FOR_EASY_CANCELLATION = 30;
        public const int MINUTES_BEFOR_APPLY_FULL_CHARGE_FOR_PARTIAL_CANCELLATION = 60;
        public const int MINUTES_BEFOR_APPLY_PARTIAL_CHARGE_FOR_PARTIAL_CANCALLETION = 1440;
        public const int SEAT_RESERVATION_AUTO_EXPIRATION_MINUTES = 10;
        public const int TIMELINE_START_TIME_AFTER_NOW_MINUTES = 30;                // time gap in minutes between now and first event shown in timeline
        public const float FREE_EVENT_RATING_CALCULATION_FACTOR = 0.5f;             // the factor applied to free events when user rating in category is calculated
        public const int EMAIL_USERS_ABOUT_START_FIRST_TIME_BEFORE_MINUTES = 2 * 60;
        public const int EMAIL_USERS_ABOUT_START_SECOND_TIME_BEFORE_MINUTES = 12 * 60;
        public const int MIN_NUMBER_OF_HELD_EVENTS_AFTER_RATING_IS_VISIBLE = 5;
        public const int MIN_NUMBER_OF_UNIQUE_FEEDBACK_GIVERS_AFTER_RATING_IS_VISIBLE = 5;
        public const int MIN_NUMBER_OF_FEEDBACK_GIVER_REGISTRATION_DAYS = 1;
        public const int MIN_NUMBER_OF_FEEDBACK_GIVER_VISITED_EVENTS = 3;
        public const int MIN_NUMBER_OF_FEEDBACK_GIVER_HELD_EVENTS = 3;
        public const int MIN_NUMBER_OF_FEEDBACK_GIVER_PURCHASES = 3;
        public const int MIN_NUMBER_OF_MINUTES_BEFORE_EVENT_STARTS = 55;
        public const int HOURS_AFTER_EVENT_ENDS_WHEN_FEEDBACK_IS_ASKED = 1;
        public const int NUMBER_OF_DAYS_WAITING_FOR_FEEDBACK = 7;                   // the number of days after sport event has ended when we allow feedbacks and complaints. Only then transfer payments to event host
        public const int MAX_DISTANCE_TO_USER = 50000;                              // max distance from user to event in meters
        public const int INITIAL_PAGE_SIZE = 10;                                    // max events in single location initial timeline
        public const int MAX_TIME_WINDOW = 3;                                       // time gap between now and last event start time on the map in months
        public const int NUMBER_OF_DAYS_UNTIL_DELETE_UNBOOKED_EVENTS = 3;           // in production it will be 6 months. After 6 month event ended it will be deleted from archive
        public const string PLATFORM_DEFAULT_CURRENCY = "eur";


        // Validation Rules
        public const decimal MIN_EVENT_PRICE = 1.00m;                               // Stripe has min charge limit, therefor we set min price       
        public const float MIN_COMPLAINTS_FOR_REFUNDING = 0.8f;                     // the percentage of complaints against the number of visitors to refund all visitors
        public const int MAX_NUMBER_OF_TICKETS_IN_FREE_EVENTS = 100;                // users without verified Stripe account has this limit on max places in one free event
        public const int MAX_DAYS_UNTIL_EVENT_STARTS = 60;                          // the max start date expressed in days between now and event start date
        public const int MAX_HOURS_OF_EVENT_DURATION = 24;                          // the max duration of the sport event instance expressed in hours


        public static Dictionary<string, decimal> STRIPE_MIN_CHARGE_TABLE = new()
        {
            { "USD", 0.50m }, { "AED", 2.00m }, { "AUD", 0.50m }, { "BGN", 1.00m }, { "BRL", 0.50m },
            { "CAD", 0.50m }, { "CHF", 0.50m }, { "CZK", 15.0m }, { "DKK", 2.50m }, { "EUR", 0.50m },
            { "GBP", 0.30m }, { "HKD", 4.00m }, { "HRK", 0.50m }, { "HUF", 175.0m }, { "INR", 0.50m },
            { "JPY", 50.0m }, { "MXN", 10.0m }, { "MYR", 2.00m }, { "NOK", 3.00m }, { "NZD", 0.50m },
            { "PLN", 2.00m }, { "RON", 2.00m }, { "SEK", 3.00m }, { "SGD", 0.50m }, { "THB", 10.00m }
        };

        public static Dictionary<string, decimal> STRIPE_MIN_PAYOUT_TABLE = new()
        {
            { "USD", 105.0m }, { "AED", 389.0m }, { "AUD", 158.0m }, { "BGN", 195.0m }, { "BRL", 562.0m },
            { "CAD", 145.0m }, { "CHF", 98.0m }, { "CZK", 2424.0m }, { "DKK", 743.0m }, { "EUR", 100.0m },
            { "GBP", 87.0m }, { "HKD", 824.0m }, { "HRK", 754.0m }, { "HUF", 40579.0m }, { "INR", 8764.0m },
            { "JPY", 14483.0m }, { "MXN", 2096.0m }, { "MYR", 468.0m }, { "NOK", 1048.0m }, { "NZD", 166.0m },
            { "PLN", 468.0m }, { "RON", 491.0m }, { "SEK", 1102m }, { "SGD", 143.0m }, { "THB", 3690.0m }
        };
    }
}
