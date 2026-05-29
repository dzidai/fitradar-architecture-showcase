namespace Fitradar.SharedKernel.Extensions
{
    public static class UnixTimeConverter
    {
        /// <summary>
        /// Converts a <see cref="DateTime"/> to the number of milliseconds since the Unix epoch (1970-01-01T00:00:00Z).
        /// The <paramref name="dateTime"/> is treated as UTC.
        /// </summary>
        public static long ToUnixTimeMilliseconds(this DateTime dateTime) =>
            new DateTimeOffset(dateTime, TimeSpan.Zero).ToUnixTimeMilliseconds();

        /// <summary>
        /// Creates a UTC <see cref="DateTime"/> from the number of milliseconds since the Unix epoch.
        /// </summary>
        public static DateTime FromUnixTimeMilliseconds(long milliseconds) =>
            DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).UtcDateTime;
    }
}
