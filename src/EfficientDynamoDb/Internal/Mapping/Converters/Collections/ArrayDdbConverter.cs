using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Collections
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
    }
}