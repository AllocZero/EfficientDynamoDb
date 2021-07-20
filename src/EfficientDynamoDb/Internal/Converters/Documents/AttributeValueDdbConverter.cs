using System;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader.DocumentDdbReader;

namespace EfficientDynamoDb.Internal.Converters.Documents
{
    internal sealed class AttributeValueDdbConverter : DdbResumableConverter<AttributeValue>
    {
        internal override DdbClassType ClassType => DdbClassType.None;
        
        public override AttributeValue Read(in AttributeValue attributeValue) => attributeValue.AsDocument();

        internal override bool TryRead(ref DdbReader reader, out AttributeValue value) => DocumentDdbReader.TryReadValue(ref reader, out value);

        public override AttributeValue Write(ref AttributeValue value) => value;
        
        public override void Write(in DdbWriter writer, ref AttributeValue value) => value.Write(writer.JsonWriter);
    }

    internal sealed class AttributeValueDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(AttributeValue);

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata) => new AttributeValueDdbConverter();
    }
}