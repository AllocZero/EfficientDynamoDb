using System.Globalization;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Collections.NumberSet
{
    internal sealed class UIntNumberSetDdbConverter : NumberSetDdbConverter<uint>
    {
        protected override uint ParseValue(string value) => uint.Parse(value, CultureInfo.InvariantCulture);
    }
}