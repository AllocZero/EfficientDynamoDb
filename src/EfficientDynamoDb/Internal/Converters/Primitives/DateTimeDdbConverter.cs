using System;
using System.Buffers.Text;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Reader;


namespace EfficientDynamoDb.Internal.Converters.Primitives
{
    internal sealed class DateTimeDdbConverter : DdbConverter<DateTime>, IDictionaryKeyConverter<DateTime>, ISetValueConverter<DateTime>
    {
        public DateTimeDdbConverter() : base(true)
        {
        }

        public override DateTime Read(in AttributeValue attributeValue)
        {
            return DateTime.ParseExact(attributeValue.AsString(), "O", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
        }

        public override bool TryWrite(ref DateTime value, out AttributeValue attributeValue)
        {
            attributeValue = new AttributeValue(new StringAttributeValue(value.ToString("O")));
            return true;
        }

        public override AttributeValue Write(ref DateTime value) => new AttributeValue(new StringAttributeValue(value.ToString("O")));

        public override void Write(in DdbWriter writer, string attributeName, ref DateTime value)
        {
            writer.JsonWriter.WritePropertyName(attributeName);

            WriteInlined(writer.JsonWriter, ref value);
        }

        public override void Write(in DdbWriter writer, ref DateTime value) => WriteInlined(writer.JsonWriter, ref value);

        public void WritePropertyName(in DdbWriter writer, ref DateTime value) => writer.JsonWriter.WritePropertyName(value);

        public void WriteStringValue(in DdbWriter writer, ref DateTime value) => writer.JsonWriter.WriteIso8601DateTimeValue(value);

        public override DateTime Read(ref DdbReader reader)
        {
            if(!Utf8Parser.TryParse(reader.JsonReaderValue.ValueSpan, out DateTime value, out _, 'O'))
                throw new DdbException($"Couldn't parse DateTime ddb value from '{reader.JsonReaderValue.GetString()}'.");

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInlined(Utf8JsonWriter writer, ref DateTime value)
        {
            writer.WriteStartObject();
            writer.WriteIso8601DateTime(DdbTypeNames.String, value);
            writer.WriteEndObject();
        }
    }
}