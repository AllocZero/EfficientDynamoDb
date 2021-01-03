using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;
using EfficientDynamoDb.Internal.Reader.DocumentDdbReader;

namespace EfficientDynamoDb.Internal.Converters.Documents
{
    internal sealed class AttributeValueDdbConverter : DdbResumableConverter<AttributeValue>
    {
        internal override DdbClassType ClassType => DdbClassType.None;
        
        public override AttributeValue Read(in AttributeValue attributeValue) => attributeValue.AsDocument();

        internal override bool TryRead(ref DdbReader reader, out AttributeValue value) => DocumentDdbReader.TryReadValue(ref reader, reader.AttributeType, out value);

        public override AttributeValue Write(ref AttributeValue value) => value;

        public override void Write(Utf8JsonWriter writer, string attributeName, ref AttributeValue value)
        {
            writer.WritePropertyName(attributeName);

            value.Write(writer);
        }

        public override void Write(Utf8JsonWriter writer, ref AttributeValue value) => value.Write(writer);
    }

    internal sealed class AttributeValueDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(AttributeValue);

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata) => new AttributeValueDdbConverter();
    }
}