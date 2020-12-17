using System;
using System.Globalization;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives
{
    internal sealed class DateTimeDdbConverter : DdbConverter<DateTime>
    {
        public override DateTime Read(in AttributeValue attributeValue)
        {
            return DateTime.ParseExact(attributeValue.AsString(), "O", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
        }

        public override AttributeValue Write(ref DateTime value) => new StringAttributeValue(value.ToString("O"));
    }
}