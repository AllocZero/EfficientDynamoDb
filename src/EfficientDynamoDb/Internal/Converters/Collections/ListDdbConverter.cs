using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal sealed class ListDdbConverter<T> : DdbConverter<List<T>>
    {
        private readonly DdbConverter<T> _elementConverter;

        public ListDdbConverter(DdbConverter<T> elementConverter)
        {
            _elementConverter = elementConverter;
        }

        public override List<T> Read(in AttributeValue attributeValue)
        {
            var items = attributeValue.AsListAttribute().Items;
            var entities = new List<T>(items.Length);

            for (var i = 0; i < items.Length; i++)
            {
                ref var item = ref items[i];
                entities.Add(_elementConverter.Read(in item));
            }

            return entities;
        }

        public override AttributeValue Write(ref List<T> value)
        {
            var array = new AttributeValue[value.Count];

            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                array[i] = _elementConverter.Write(ref item);
            }
            
            return new ListAttributeValue(array);
        }

        public override void Write(Utf8JsonWriter writer, string attributeName, ref List<T> value)
        {
            writer.WritePropertyName(attributeName);
            
            writer.WriteStartObject();
            writer.WritePropertyName("L");
            
            writer.WriteStartArray();

            foreach (var item in value)
            {
                var itemCopy = item;
                _elementConverter.Write(writer, attributeName, ref itemCopy);
            }
            
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}