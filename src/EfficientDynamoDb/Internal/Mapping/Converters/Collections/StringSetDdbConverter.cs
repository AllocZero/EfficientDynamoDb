using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Collections
{
    internal sealed class StringSetDdbConverter : DdbConverter<HashSet<string>>
    {
        public override HashSet<string> Read(in AttributeValue attributeValue) => attributeValue.AsStringSetAttribute().Items;
    }
}