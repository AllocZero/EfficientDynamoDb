using System.Globalization;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Collections.NumberSet
{
    internal sealed class DecimalNumberSetDdbConverter : NumberSetDdbConverter<decimal>
    {
        protected override decimal ParseValue(string value) => decimal.Parse(value, CultureInfo.InvariantCulture);
    }
}