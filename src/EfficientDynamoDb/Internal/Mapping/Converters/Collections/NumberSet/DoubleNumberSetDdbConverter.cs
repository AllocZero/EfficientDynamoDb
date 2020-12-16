using System.Globalization;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Collections.NumberSet
{
    internal sealed class DoubleNumberSetDdbConverter : NumberSetDdbConverter<double>
    {
        protected override double ParseValue(string value) => double.Parse(value, CultureInfo.InvariantCulture);
    }
}