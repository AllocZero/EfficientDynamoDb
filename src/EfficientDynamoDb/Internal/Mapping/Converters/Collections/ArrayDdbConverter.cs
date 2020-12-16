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

        public override T[] Read(AttributeValue attributeValue)
        {
            var items = attributeValue.AsListAttribute().Items;
            var entities = new T[items.Length];

            for (var i = 0; i < items.Length; i++)
                entities[i] = _elementConverter.Read(items[i]);

            return entities;
        }
    }
}