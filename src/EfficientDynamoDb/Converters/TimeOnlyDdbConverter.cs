using System;
using System.Globalization;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Converters;

public class TimeOnlyDdbConverter : TimeDdbConverter<TimeOnly>
{
    public TimeOnlyDdbConverter() : this("O", 16)
    {
    }

    public TimeOnlyDdbConverter(string format) : this(format, MaxDateTimeStringLength)
    {
    }

    internal TimeOnlyDdbConverter(string format, int maxDateTimeStringLength) : base(format, maxDateTimeStringLength)
    {
        DateTimeStyles = DateTimeStyles.None;
    }

    public sealed override TimeOnly Read(in AttributeValue attributeValue) =>
        TimeOnly.ParseExact(attributeValue.AsString(), Format, CultureInfo, DateTimeStyles);

    public override AttributeValue Write(ref TimeOnly value) => new(new StringAttributeValue(value.ToString(Format, CultureInfo)));

    public override string WriteStringValue(ref TimeOnly value) => value.ToString(Format, CultureInfo);

    protected override bool TryWriteToBuffer(TimeOnly value, Span<char> buffer, out int charsWritten) => 
        value.TryFormat(buffer, out charsWritten, Format, CultureInfo);

    protected override bool TryParseFromBuffer(Span<char> buffer, out TimeOnly value) =>
        TimeOnly.TryParseExact(buffer, Format, CultureInfo, DateTimeStyles, out value);
}