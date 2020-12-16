using System.Globalization;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Collections.NumberSet
{
    internal sealed class IntNumberSetDdbConverter : NumberSetDdbConverter<int>
    {
        protected override int ParseValue(string value) => int.Parse(value, CultureInfo.InvariantCulture);
    }
}