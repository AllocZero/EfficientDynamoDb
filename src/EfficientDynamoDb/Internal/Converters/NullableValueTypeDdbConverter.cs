using System.Text.Json;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;

namespace EfficientDynamoDb.Internal.Converters
{
    internal sealed class NullableValueTypeDdbConverter<T> : DdbConverter<T?> where T : struct
    {
        private readonly DdbConverter<T> _converter;

        public NullableValueTypeDdbConverter(DdbConverter<T> converter)
        {
            _converter = converter;
        }

        public override T? Read(in AttributeValue attributeValue) => _converter.Read(in attributeValue);

        public override AttributeValue Write(ref T? value)
        {
            var notNullableValue = value!.Value;
            return _converter.Write(ref notNullableValue);
        }

        public override void Write(Utf8JsonWriter writer, string attributeName, ref T? value)
        {
            if (!value.HasValue)
                return;
            
            var attributeValue = Write(ref value);
            if (attributeValue.IsNull)
                return;
            
            writer.WritePropertyName(attributeName);
            attributeValue.Write(writer);
        }
    }
}