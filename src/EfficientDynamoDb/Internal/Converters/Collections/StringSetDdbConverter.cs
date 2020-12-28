using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal sealed class StringSetDdbConverter : SetDdbConverter<string>
    {
        protected override string ReadValue(ref DdbReader reader) => reader.JsonReaderValue.GetString()!;

        public override HashSet<string> Read(in AttributeValue attributeValue) => attributeValue.AsStringSetAttribute().Items;

        public override AttributeValue Write(ref HashSet<string> value) => new StringSetAttributeValue(value);
        
    }
}