using System.Globalization;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Collections.NumberSet
{
    internal sealed class LongNumberSetDdbConverter : NumberSetDdbConverter<long>
    {
        protected override long ParseValue(string value) => long.Parse(value, CultureInfo.InvariantCulture);
    }
}