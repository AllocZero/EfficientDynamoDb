using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Collections
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
    }
}