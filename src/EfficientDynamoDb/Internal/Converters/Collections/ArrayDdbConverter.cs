using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal sealed class ArrayDdbConverter<T> : DdbConverter<T[]>
    {
        private readonly DdbConverter<T> _elementConverter;

        public ArrayDdbConverter(DdbConverter<T> elementConverter)
        {
            _elementConverter = elementConverter;
        }

        public override T[] Read(in AttributeValue attributeValue)
        {
            var items = attributeValue.AsListAttribute().Items;
            var entities = new T[items.Length];

            for (var i = 0; i < items.Length; i++)
            {
                ref var item = ref items[i];
                entities[i] = _elementConverter.Read(in item);
            }

            return entities;
        }

        public override AttributeValue Write(ref T[] value)
        {
            var array = new AttributeValue[value.Length];

            for (var i = 0; i < value.Length; i++)
                array[i] = _elementConverter.Write(ref value[i]);

            return new ListAttributeValue(array);
        }

        public override void Write(Utf8JsonWriter writer, string attributeName, ref T[] value)
        {
            writer.WritePropertyName(attributeName);

            WriteInlined(writer, ref value);
        }

        public override void Write(Utf8JsonWriter writer, ref T[] value) => WriteInlined(writer, ref value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteInlined(Utf8JsonWriter writer, ref T[] value)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("L");

            writer.WriteStartArray();

            for (var i = 0; i < value.Length; i++)
                _elementConverter.Write(writer, ref value[i]);

            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}