using System.Globalization;

namespace Fitradar.SharedKernel.Extensions
{
    /// <summary>
    /// Formats and parses <see cref="DateTime"/> values using the RFC 2445 (iCalendar) date/time format.
    /// Supported patterns:
    ///   <c>yyyyMMdd</c>           — date only
    ///   <c>yyyyMMddTHHmmss</c>    — local date-time
    ///   <c>yyyyMMddTHHmmssZ</c>   — UTC date-time
    /// </summary>
    public static class Rfc2445Formatter
    {
        public static string ToRfc2445String(this DateTime dateTime) =>
            dateTime.ToString("yyyyMMddTHHmmss");

        public static DateTime ParseFromRfc2445(string value)
        {
            if (value.Length < 8)
                throw new FormatException($"String is too short: \"{value}\". Expected at least 8 characters.");

            if (value.Length > 8)
            {
                if (value.Length < 15)
                    throw new FormatException(
                        $"String is too short: \"{value}\". If there are more than 8 characters there must be at least 15.");

                if (!value.Contains('T'))
                    throw new FormatException($"Unexpected character '{value[8]}' at pos=8. Expected 'T'.");

                if (value.Length > 15 && !value.Contains('Z'))
                    throw new FormatException($"Unexpected character '{value[15]}' at pos=15. Expected 'Z'.");
            }

            return value.Length switch
            {
                8  => DateTime.ParseExact(value, "yyyyMMdd",        CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal),
                15 => DateTime.ParseExact(value, "yyyyMMddTHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal),
                _  => DateTime.ParseExact(value, "yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal)
            };
        }
    }
}
