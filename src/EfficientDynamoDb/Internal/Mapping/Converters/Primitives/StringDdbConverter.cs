using EfficientDynamoDb.DocumentModel.AttributeValues;
using NotImplementedException = System.NotImplementedException;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives
{
    internal sealed class StringDdbConverter : DdbConverter<string>
    {
        public override string Read(in AttributeValue attributeValue) => attributeValue.AsString();

        public override AttributeValue Write(ref string value) => new StringAttributeValue(value);
    }
}