using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;

namespace EfficientDynamoDb.Internal.Converters.Primitives
{
    public class BinaryDdbConverter : DdbConverter<byte[]?>
    {
        public override byte[]? Read(in AttributeValue attributeValue) => attributeValue.IsNull ? null : attributeValue.AsBinaryAttribute().Value;

        public override AttributeValue Write(ref byte[]? value) => value == null ? AttributeValue.Null : new AttributeValue(new BinaryAttributeValue(value));

        public override byte[]? Read(ref DdbReader reader) => reader.AttributeType == AttributeType.Null ? null : reader.JsonReaderValue.GetBytesFromBase64();

        public override void Write(in DdbWriter writer, ref byte[]? value)
        {
            if (value == null)
                writer.WriteDdbNull();
            else
                writer.WriteDdbBinary(value);
        }
    }
}