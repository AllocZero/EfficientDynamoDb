using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Primitives
{
    internal sealed class StringDdbConverter : DdbConverter<string>
    {
        public StringDdbConverter() : base(true)
        {
        }

        public override string Read(in AttributeValue attributeValue) => attributeValue.AsString();

        public override AttributeValue Write(ref string value) => new StringAttributeValue(value);

        public override void Write(Utf8JsonWriter writer, string attributeName, ref string value)
        {
            if (value is null)
                return;
            
            writer.WritePropertyName(attributeName);
            WriteInlined(writer, ref value);
        }

        public override void Write(Utf8JsonWriter writer, ref string value) => WriteInlined(writer, ref value);

        public override void WriteStringValue(Utf8JsonWriter writer, ref string value) => writer.WriteStringValue(value);

        public override string Read(ref DdbReader reader)
        {
            return reader.JsonReaderValue.GetString()!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteInlined(Utf8JsonWriter writer, ref string value)
        {
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.String, value);
            writer.WriteEndObject();
        }
    }
}