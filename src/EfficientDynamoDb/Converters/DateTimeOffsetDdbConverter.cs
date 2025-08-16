using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Exceptions;

namespace EfficientDynamoDb.Converters
{
    public class DateTimeOffsetDdbConverter : DdbConverter<DateTimeOffset>, IDictionaryKeyConverter<DateTimeOffset>, ISetValueConverter<DateTimeOffset>
    {
        private const int MaxDateTimeStringLength = 64;
        
        public string Format { get; }

        public DateTimeStyles DateTimeStyles { get; set; } = DateTimeStyles.RoundtripKind;

        public CultureInfo CultureInfo { get; set; } = CultureInfo.InvariantCulture;

        internal int StackAllocSize { get; } = MaxDateTimeStringLength;

        public DateTimeOffsetDdbConverter() : this("O", 33)
        {
        }

        public DateTimeOffsetDdbConverter(string format) : base(true)
        {
            Format = format;
        }

        internal DateTimeOffsetDdbConverter(string format, int maxDateTimeStringLength) : this(format)
        {
            StackAllocSize = maxDateTimeStringLength;
        }

        public sealed override DateTimeOffset Read(in AttributeValue attributeValue) =>
            DateTimeOffset.ParseExact(attributeValue.AsString(), Format, CultureInfo, DateTimeStyles);

        public override AttributeValue Write(ref DateTimeOffset value) => new AttributeValue(new StringAttributeValue(value.ToString(Format, CultureInfo)));

        public override void Write(in DdbWriter writer, ref DateTimeOffset value)
        {
            writer.JsonWriter.WriteStartObject();
            
            Span<char> buffer = stackalloc char[StackAllocSize];

            WriteToBuffer(value, buffer, out var length);
            
            writer.JsonWriter.WriteString(DdbTypeNames.String, buffer[..length]);
            
            writer.JsonWriter.WriteEndObject();
        }

        public virtual void WritePropertyName(in DdbWriter writer, ref DateTimeOffset value)
        {
            Span<char> buffer = stackalloc char[StackAllocSize];
            
            WriteToBuffer(value, buffer, out var length);
            
            writer.JsonWriter.WritePropertyName(buffer[..length]);
        }

        public virtual string WriteStringValue(ref DateTimeOffset value) => value.ToString(Format, CultureInfo);

        public virtual void WriteStringValue(in DdbWriter writer, ref DateTimeOffset value)
        {
            Span<char> buffer = stackalloc char[StackAllocSize];
            
            WriteToBuffer(value, buffer, out var length);

            writer.JsonWriter.WriteStringValue(buffer[..length]);
        }

        public sealed override DateTimeOffset Read(ref DdbReader reader)
        {
            Span<char> buffer = stackalloc char[StackAllocSize];

            var length = Encoding.UTF8.GetChars(reader.JsonReaderValue.ValueSpan, buffer);

            if (!DateTimeOffset.TryParseExact(buffer[..length], Format, CultureInfo, DateTimeStyles, out var value))
                throw new DdbException($"Couldn't parse DateTimeOffset ddb value from '{reader.JsonReaderValue.GetString()}'.");

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteToBuffer(DateTimeOffset value, Span<char> buffer, out int charsWritten)
        {
            if (!value.TryFormat(buffer, out charsWritten, Format, CultureInfo))
                throw new DdbException($"Couldn't format DateTimeOffset ddb value from '{value}'.");
        }
    }
}