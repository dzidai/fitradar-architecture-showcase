using System.Globalization;

namespace Fitradar.SharedKernel.Extensions
{
    public static class DateTimeExtensions
    {
        public const int SECOND = 1;
        public const int MINUTE = 2;
        public const int HOUR = 3;
        public const int MONTH_DAY = 4;
        public const int MONTH = 5;
        public const int YEAR = 6;
        public const int WEEK_DAY = 7;
        public const int YEAR_DAY = 8;
        public const int WEEK_NUM = 9;

        public const int SUNDAY = 0;
        public const int MONDAY = 1;
        public const int TUESDAY = 2;
        public const int WEDNESDAY = 3;
        public const int THURSDAY = 4;
        public const int FRIDAY = 5;
        public const int SATURDAY = 6;

        public static int ToTimeDay(this DayOfWeek day) => day switch
        {
            DayOfWeek.Sunday    => SUNDAY,
            DayOfWeek.Monday    => MONDAY,
            DayOfWeek.Tuesday   => TUESDAY,
            DayOfWeek.Wednesday => WEDNESDAY,
            DayOfWeek.Thursday  => THURSDAY,
            DayOfWeek.Friday    => FRIDAY,
            DayOfWeek.Saturday  => SATURDAY,
            _                   => throw new ArgumentOutOfRangeException(nameof(day), day, "Unsupported day of week.")
        };

        public static int GetActualMaximum(this DateTime dateTime, int field)
        {
            return field switch
            {
                SECOND    => 59,
                MINUTE    => 59,
                HOUR      => 23,
                MONTH_DAY => DateTime.DaysInMonth(dateTime.Year, dateTime.Month),
                MONTH     => 11,
                YEAR      => 2037,
                WEEK_DAY  => 6,
                YEAR_DAY  => DateTime.IsLeapYear(dateTime.Year) ? 365 : 364,
                WEEK_NUM  => throw new NotSupportedException("WEEK_NUM is not supported."),
                _         => throw new ArgumentOutOfRangeException(nameof(field), field, "Unknown calendar field.")
            };
        }

        /// <summary>
        /// Returns the ISO 8601 week number for the given date.
        /// Delegates to <see cref="ISOWeek.GetWeekOfYear"/>.
        /// </summary>
        public static int GetWeekNumber(this DateTime dateTime) =>
            ISOWeek.GetWeekOfYear(dateTime);

        public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero) return dateTime;
            if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue) return dateTime;
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }
    }
}
