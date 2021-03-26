using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Internal.Converters.Primitives
{
    internal sealed class StringDdbConverter : DdbConverter<string?>, IDictionaryKeyConverter<string>, ISetValueConverter<string>
    {
        public StringDdbConverter() : base(true)
        {
        }

        public override string? Read(in AttributeValue attributeValue) => attributeValue.IsNull ? null : attributeValue.AsString();
        
        public override AttributeValue Write(ref string? value) => value == null ? AttributeValue.Null : new AttributeValue(new StringAttributeValue(value));

        public override void Write(in DdbWriter writer, ref string? value)
        {
            if (value == null)
                writer.WriteDdbNull();
            else
                writer.WriteDdbString(value);
        }

        public void WritePropertyName(in DdbWriter writer, ref string value) => writer.JsonWriter.WritePropertyName(value);

        public string WriteStringValue(ref string value) => value;

        public void WriteStringValue(in DdbWriter writer, ref string value) => writer.JsonWriter.WriteStringValue(value);

        public override string? Read(ref DdbReader reader)
        {
            return reader.AttributeType == AttributeType.Null ? null : reader.JsonReaderValue.GetString()!;
        }
    }
}