#nullable disable
using System;
using System.Globalization;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace Benchmarks.Converters
{
    public class DateTimeUtcConverter : IPropertyConverter
    {
        public DynamoDBEntry ToEntry(object value) => (DateTime) value;

        public object FromEntry(DynamoDBEntry entry)
        {
            var dateTime = entry.AsString();

            return DateTime.ParseExact(dateTime, "O", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
        }
    }
}