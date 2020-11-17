using System;
using System.Globalization;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.Internal.Extensions
{
    internal static class DateTimeFormattingExtensions
    {
        public static string ToIso8601BasicDateTime(this DateTime dateTime) =>
            dateTime.ToString(DateTimeFormats.Iso8601BasicDateTimeFormat, CultureInfo.InvariantCulture);
        
        public static string ToIso8601BasicDate(this DateTime dateTime) =>
            dateTime.ToString(DateTimeFormats.Iso8601BasicDateFormat, CultureInfo.InvariantCulture);
    }
}