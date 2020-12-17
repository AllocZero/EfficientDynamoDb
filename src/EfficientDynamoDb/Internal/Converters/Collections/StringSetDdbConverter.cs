using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal sealed class StringSetDdbConverter : DdbConverter<HashSet<string>>
    {
        public override HashSet<string> Read(in AttributeValue attributeValue) => attributeValue.AsStringSetAttribute().Items;

        public override AttributeValue Write(ref HashSet<string> value) => new StringSetAttributeValue(value);
    }
}