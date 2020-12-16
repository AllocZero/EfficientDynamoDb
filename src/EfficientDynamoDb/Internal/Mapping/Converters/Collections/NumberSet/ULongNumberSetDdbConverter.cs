using System.Globalization;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Collections.NumberSet
{
    internal sealed class ULongNumberSetDdbConverter : NumberSetDdbConverter<ulong>
    {
        protected override ulong ParseValue(string value) => ulong.Parse(value, CultureInfo.InvariantCulture);
    }
}