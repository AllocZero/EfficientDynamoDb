using System;
using System.Globalization;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Converters;

public class DateTimeDdbConverter : TimeDdbConverter<DateTime>
{
    public DateTimeDdbConverter() : this("O", 33)
    {
    }

    public DateTimeDdbConverter(string format) : this(format, MaxDateTimeStringLength)
    {
    }

    internal DateTimeDdbConverter(string format, int maxDateTimeStringLength) : base(format, maxDateTimeStringLength)
    {
        DateTimeStyles = DateTimeStyles.RoundtripKind;
    }

    public sealed override DateTime Read(in AttributeValue attributeValue) =>
        DateTime.ParseExact(attributeValue.AsString(), Format, CultureInfo, DateTimeStyles);

    public override AttributeValue Write(ref DateTime value) => new(new StringAttributeValue(value.ToString(Format, CultureInfo)));

    public override string WriteStringValue(ref DateTime value) => value.ToString(Format, CultureInfo);

    protected override bool TryWriteToBuffer(DateTime value, Span<char> buffer, out int charsWritten) => 
        value.TryFormat(buffer, out charsWritten, Format, CultureInfo);

    protected override bool TryParseFromBuffer(Span<char> buffer, out DateTime value) =>
        DateTime.TryParseExact(buffer, Format, CultureInfo, DateTimeStyles, out value);
}