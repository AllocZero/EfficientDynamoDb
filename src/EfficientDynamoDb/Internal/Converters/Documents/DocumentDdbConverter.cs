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
    internal sealed class DocumentDdbConverter : DdbResumableConverter<Document>
    {
        internal override DdbClassType ClassType => DdbClassType.None;
        
        public override Document Read(in AttributeValue attributeValue) => attributeValue.AsDocument();

        internal override bool TryRead(ref DdbReader reader, out Document value) => DocumentDdbReader.TryReadMap(ref reader, out value);

        public override AttributeValue Write(ref Document value) => new MapAttributeValue(value);

        public override void Write(in DdbWriter writer, string attributeName, ref Document value)
        {
            writer.JsonWriter.WritePropertyName(attributeName);

            WriteInlined(writer.JsonWriter, ref value);
        }

        public override void Write(in DdbWriter writer, ref Document value) => WriteInlined(writer.JsonWriter, ref value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInlined(Utf8JsonWriter writer, ref Document value) => writer.WriteAttributesDictionary(value);
    }

    internal sealed class DocumentDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(Document);

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata) => new DocumentDdbConverter();
    }
}