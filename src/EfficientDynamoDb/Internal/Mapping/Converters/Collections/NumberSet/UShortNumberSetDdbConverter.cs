using System.Globalization;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Collections.NumberSet
{
    internal sealed class UShortNumberSetDdbConverter : NumberSetDdbConverter<ushort>
    {
        protected override ushort ParseValue(string value) => ushort.Parse(value, CultureInfo.InvariantCulture);
    }
}