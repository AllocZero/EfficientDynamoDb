using System;
using System.Globalization;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Converters;

public class DateTimeOffsetDdbConverter : TimeDdbConverter<DateTimeOffset>
{
    public DateTimeOffsetDdbConverter() : this("O", 33)
    {
    }

    public DateTimeOffsetDdbConverter(string format) : this(format, MaxDateTimeStringLength)
    {
    }

    internal DateTimeOffsetDdbConverter(string format, int maxDateTimeStringLength) : base(format, maxDateTimeStringLength)
    {
        DateTimeStyles = DateTimeStyles.RoundtripKind;
    }

    public sealed override DateTimeOffset Read(in AttributeValue attributeValue) =>
        DateTimeOffset.ParseExact(attributeValue.AsString(), Format, CultureInfo, DateTimeStyles);

    public override AttributeValue Write(ref DateTimeOffset value) => new(new StringAttributeValue(value.ToString(Format, CultureInfo)));

    public override string WriteStringValue(ref DateTimeOffset value) => value.ToString(Format, CultureInfo);

    protected override bool TryWriteToBuffer(DateTimeOffset value, Span<char> buffer, out int charsWritten) => 
        value.TryFormat(buffer, out charsWritten, Format, CultureInfo);

    protected override bool TryParseFromBuffer(Span<char> buffer, out DateTimeOffset value) =>
        DateTimeOffset.TryParseExact(buffer, Format, CultureInfo, DateTimeStyles, out value);
}