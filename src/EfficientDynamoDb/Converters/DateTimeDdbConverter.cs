using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Exceptions;

namespace EfficientDynamoDb.Converters
{
    public class DateTimeDdbConverter : DdbConverter<DateTime>, IDictionaryKeyConverter<DateTime>, ISetValueConverter<DateTime>
    {
        private const int MaxDateTimeStringLength = 64;
        
        public string Format { get; }

        public DateTimeStyles DateTimeStyles { get; set; } = DateTimeStyles.RoundtripKind;

        public CultureInfo CultureInfo { get; set; } = CultureInfo.InvariantCulture;

        internal int StackAllocSize { get; } = MaxDateTimeStringLength;

        public DateTimeDdbConverter() : this("O", 28)
        {
        }

        public DateTimeDdbConverter(string format) : base(true)
        {
            Format = format;
        }

        internal DateTimeDdbConverter(string format, int maxDateTimeStringLength) : this(format)
        {
            StackAllocSize = maxDateTimeStringLength;
        }

        public sealed override DateTime Read(in AttributeValue attributeValue) =>
            DateTime.ParseExact(attributeValue.AsString(), Format, CultureInfo, DateTimeStyles);

        public override AttributeValue Write(ref DateTime value) => new AttributeValue(new StringAttributeValue(value.ToString(Format, CultureInfo)));

        public override void Write(in DdbWriter writer, ref DateTime value)
        {
            writer.JsonWriter.WriteStartObject();
            
            Span<char> buffer = stackalloc char[StackAllocSize];

            WriteToBuffer(value, buffer, out var length);
            
            writer.JsonWriter.WriteString(DdbTypeNames.String, buffer[..length]);
            
            writer.JsonWriter.WriteEndObject();
        }

        public virtual void WritePropertyName(in DdbWriter writer, ref DateTime value)
        {
            Span<char> buffer = stackalloc char[StackAllocSize];
            
            WriteToBuffer(value, buffer, out var length);
            
            writer.JsonWriter.WritePropertyName(buffer[..length]);
        }

        public virtual string WriteStringValue(ref DateTime value) => value.ToString(Format, CultureInfo);

        public virtual void WriteStringValue(in DdbWriter writer, ref DateTime value)
        {
            Span<char> buffer = stackalloc char[StackAllocSize];
            
            WriteToBuffer(value, buffer, out var length);

            writer.JsonWriter.WriteStringValue(buffer[..length]);
        }

        public sealed override DateTime Read(ref DdbReader reader)
        {
            Span<char> buffer = stackalloc char[StackAllocSize];

            var length = Encoding.UTF8.GetChars(reader.JsonReaderValue.ValueSpan, buffer);

            if (!DateTime.TryParseExact(buffer[..length], Format, CultureInfo, DateTimeStyles, out var value))
                throw new DdbException($"Couldn't parse DateTime ddb value from '{reader.JsonReaderValue.GetString()}'.");

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteToBuffer(DateTime value, Span<char> buffer, out int charsWritten)
        {
            if (!value.TryFormat(buffer, out charsWritten, Format, CultureInfo))
                throw new DdbException($"Couldn't format DateTime ddb value from '{value}'.");
        }
    }
}