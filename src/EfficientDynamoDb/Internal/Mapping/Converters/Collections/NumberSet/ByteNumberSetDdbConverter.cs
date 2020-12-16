using System.Globalization;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Collections.NumberSet
{
    internal sealed class ByteNumberSetDdbConverter : NumberSetDdbConverter<byte>
    {
        protected override byte ParseValue(string value) => byte.Parse(value, CultureInfo.InvariantCulture);
    }
}