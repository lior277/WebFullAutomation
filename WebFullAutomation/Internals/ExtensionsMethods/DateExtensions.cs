using System;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Internals.ExtensionsMethods
{
    public static class DateExtensions
    {
        public static int ToUnixTimestamps(this DateTime value)
        {
            return (int)Math.Truncate((value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        }

        public static bool CheckIfInTimeRange(this TimeSpan timeSpan, TimeSpan from, TimeSpan to)
        {
            return timeSpan >= from && timeSpan < to;
        }
    }
}
