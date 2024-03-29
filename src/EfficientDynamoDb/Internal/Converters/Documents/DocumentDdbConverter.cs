using System;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader.DocumentDdbReader;

namespace EfficientDynamoDb.Internal.Converters.Documents
{
    internal sealed class DocumentDdbConverter : DdbResumableConverter<Document?>
    {
        internal override DdbClassType ClassType => DdbClassType.None;
        
        public override Document Read(in AttributeValue attributeValue) => attributeValue.AsDocument();

        internal override bool TryRead(ref DdbReader reader, out Document value) => DocumentDdbReader.TryReadMap(ref reader, out value);

        public override AttributeValue Write(ref Document? value) => value == null ? AttributeValue.Null : new AttributeValue(new MapAttributeValue(value));

        public override void Write(in DdbWriter writer, ref Document? value) => WriteInlined(in writer, ref value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInlined(in DdbWriter writer, ref Document? value)
        {
            if (value == null)
            {
                writer.WriteDdbNull();
                return;
            }
            
            writer.JsonWriter.WriteAttributesDictionary(value);
        }
    }

    internal sealed class DocumentDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(Document);

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata) => new DocumentDdbConverter();
    }
}