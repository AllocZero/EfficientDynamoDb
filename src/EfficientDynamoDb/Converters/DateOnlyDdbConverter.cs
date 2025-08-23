using System;
using System.Globalization;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Converters;

public class DateOnlyDdbConverter : TimeDdbConverter<DateOnly>
{
    public DateOnlyDdbConverter() : this("O", 10)
    {
    }

    public DateOnlyDdbConverter(string format) : this(format, MaxDateTimeStringLength)
    {
    }

    internal DateOnlyDdbConverter(string format, int maxDateTimeStringLength) : base(format, maxDateTimeStringLength)
    {
        DateTimeStyles = DateTimeStyles.None;
    }

    public sealed override DateOnly Read(in AttributeValue attributeValue) =>
        DateOnly.ParseExact(attributeValue.AsString(), Format, CultureInfo, DateTimeStyles);

    public override AttributeValue Write(ref DateOnly value) => new(new StringAttributeValue(value.ToString(Format, CultureInfo)));

    public override string WriteStringValue(ref DateOnly value) => value.ToString(Format, CultureInfo);

    protected override bool TryWriteToBuffer(DateOnly value, Span<char> buffer, out int charsWritten) => 
        value.TryFormat(buffer, out charsWritten, Format, CultureInfo);

    protected override bool TryParseFromBuffer(Span<char> buffer, out DateOnly value) =>
        DateOnly.TryParseExact(buffer, Format, CultureInfo, DateTimeStyles, out value);
}