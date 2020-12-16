using System.Globalization;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Collections.NumberSet
{
    internal sealed class FloatNumberSetDdbConverter : NumberSetDdbConverter<float>
    {
        protected override float ParseValue(string value) => float.Parse(value, CultureInfo.InvariantCulture);
    }
}