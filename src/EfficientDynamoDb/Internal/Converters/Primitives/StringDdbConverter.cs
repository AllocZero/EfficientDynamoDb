using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Reader;

using NotImplementedException = System.NotImplementedException;

namespace EfficientDynamoDb.Internal.Converters.Primitives
{
    internal sealed class StringDdbConverter : DdbConverter<string>, IDictionaryKeyConverter<string>, ISetValueConverter<string>
    {
        public StringDdbConverter() : base(true)
        {
        }

        public override string Read(in AttributeValue attributeValue) => attributeValue.AsString();

        public override AttributeValue Write(ref string value) => new StringAttributeValue(value);

        public override void Write(in DdbWriter writer, string attributeName, ref string value)
        {
            if (value is null)
                return;
            
            writer.JsonWriter.WritePropertyName(attributeName);
            WriteInlined(writer.JsonWriter, ref value);
        }

        public override void Write(in DdbWriter writer, ref string value) => WriteInlined(writer.JsonWriter, ref value);

        public void WritePropertyName(in DdbWriter writer, ref string value) => writer.JsonWriter.WritePropertyName(value);

        public void WriteStringValue(in DdbWriter writer, ref string value) => writer.JsonWriter.WriteStringValue(value);

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