using System.Globalization;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Collections.NumberSet
{
    internal sealed class ShortNumberSetDdbConverter : NumberSetDdbConverter<short>
    {
        protected override short ParseValue(string value) => short.Parse(value, CultureInfo.InvariantCulture);
    }
}