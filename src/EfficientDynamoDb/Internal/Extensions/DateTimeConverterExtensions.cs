using System;

namespace EfficientDynamoDb.Internal.Extensions
{
    internal static class DateTimeConverterExtensions
    {
        public static double ToUnixSeconds(this DateTime dateTime) => (dateTime - UnixEpochStart).TotalSeconds;
        
        public static DateTime FromUnixSeconds(this double seconds) => UnixEpochStart.AddSeconds(seconds);

        private static readonly DateTime UnixEpochStart = new DateTime(1970, 01, 01, 00, 00, 00, DateTimeKind.Utc);
    }
}