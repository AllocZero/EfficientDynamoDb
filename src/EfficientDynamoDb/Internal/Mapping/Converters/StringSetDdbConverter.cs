using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters
{
    public sealed class StringSetDdbConverter : DdbConverter<HashSet<string>>
    {
        public override HashSet<string> Read(AttributeValue attributeValue) => attributeValue.AsStringSetAttribute().Items;
    }
}