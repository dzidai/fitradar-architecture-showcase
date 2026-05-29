using Fitradar.SharedKernel.Extensions;
using System;

namespace Fitradar.Domain.Workout
{
    public sealed record TimePeriod
    {
        public DateTime StartTime { get; init; }

        public DateTime EndTime { get; init; }


        public TimePeriod(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public TimePeriod(long startTimeMillis, long endTimeMillis)
        {
            StartTime = UnixTimeConverter.FromUnixTimeMilliseconds(startTimeMillis);
            EndTime = UnixTimeConverter.FromUnixTimeMilliseconds(endTimeMillis);
        }
    }
}
