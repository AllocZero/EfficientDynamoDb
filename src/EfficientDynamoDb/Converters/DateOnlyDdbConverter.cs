using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Exceptions;

namespace EfficientDynamoDb.Converters
{
    public class DateOnlyDdbConverter : DdbConverter<DateOnly>, IDictionaryKeyConverter<DateOnly>, ISetValueConverter<DateOnly>
    {
        private const int MaxDateTimeStringLength = 64;
        
        public string Format { get; }

        public CultureInfo CultureInfo { get; set; } = CultureInfo.InvariantCulture;

        internal int StackAllocSize { get; } = MaxDateTimeStringLength;

        public DateOnlyDdbConverter() : this("O", 10)
        {
        }

        public DateOnlyDdbConverter(string format) : base(true)
        {
            Format = format;
        }

        internal DateOnlyDdbConverter(string format, int maxDateTimeStringLength) : this(format)
        {
            StackAllocSize = maxDateTimeStringLength;
        }

        public sealed override DateOnly Read(in AttributeValue attributeValue) =>
            DateOnly.ParseExact(attributeValue.AsString(), Format, CultureInfo);

        public override AttributeValue Write(ref DateOnly value) => new AttributeValue(new StringAttributeValue(value.ToString(Format, CultureInfo)));

        public override void Write(in DdbWriter writer, ref DateOnly value)
        {
            writer.JsonWriter.WriteStartObject();
            
            Span<char> buffer = stackalloc char[StackAllocSize];

            WriteToBuffer(value, buffer, out var length);
            
            writer.JsonWriter.WriteString(DdbTypeNames.String, buffer[..length]);
            
            writer.JsonWriter.WriteEndObject();
        }

        public virtual void WritePropertyName(in DdbWriter writer, ref DateOnly value)
        {
            Span<char> buffer = stackalloc char[StackAllocSize];
            
            WriteToBuffer(value, buffer, out var length);
            
            writer.JsonWriter.WritePropertyName(buffer[..length]);
        }

        public virtual string WriteStringValue(ref DateOnly value) => value.ToString(Format, CultureInfo);

        public virtual void WriteStringValue(in DdbWriter writer, ref DateOnly value)
        {
            Span<char> buffer = stackalloc char[StackAllocSize];
            
            WriteToBuffer(value, buffer, out var length);

            writer.JsonWriter.WriteStringValue(buffer[..length]);
        }

        public sealed override DateOnly Read(ref DdbReader reader)
        {
            Span<char> buffer = stackalloc char[StackAllocSize];

            var length = Encoding.UTF8.GetChars(reader.JsonReaderValue.ValueSpan, buffer);

            if (!DateOnly.TryParseExact(buffer[..length], Format, CultureInfo, DateTimeStyles.None, out var value))
                throw new DdbException($"Couldn't parse DateOnly ddb value from '{reader.JsonReaderValue.GetString()}'.");

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteToBuffer(DateOnly value, Span<char> buffer, out int charsWritten)
        {
            if (!value.TryFormat(buffer, out charsWritten, Format, CultureInfo))
                throw new DdbException($"Couldn't format DateOnly ddb value from '{value}'.");
        }
    }
}