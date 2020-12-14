using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters
{
    public sealed class ListDdbConverter<T> : DdbConverter<List<T>>
    {
        private readonly DdbConverter<T> _elementConverter;

        public ListDdbConverter(DdbConverter<T> elementConverter)
        {
            _elementConverter = elementConverter;
        }

        public override List<T> Read(AttributeValue attributeValue)
        {
            var items = attributeValue.AsListAttribute().Items;
            var entities = new List<T>(items.Length);

            foreach (var item in items)
                entities.Add(_elementConverter.Read(item));

            return entities;
        }
    }
}