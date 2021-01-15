using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.Internal.Converters.Primitives
{
    internal sealed class StringDdbConverter : DdbConverter<string?>, IDictionaryKeyConverter<string>, ISetValueConverter<string>
    {
        public StringDdbConverter() : base(true)
        {
        }

        public override string? Read(in AttributeValue attributeValue) => attributeValue.IsNull ? null : attributeValue.AsString();

        public override bool TryWrite(ref string? value, out AttributeValue attributeValue)
        {
            attributeValue = new AttributeValue(new StringAttributeValue(value!));
            return true;
        }

        public override AttributeValue Write(ref string? value) => value == null ? AttributeValue.Null : new AttributeValue(new StringAttributeValue(value));

        public override void Write(in DdbWriter writer, string attributeName, ref string? value)
        {
            writer.JsonWriter.WritePropertyName(attributeName);
            writer.WriteDdbString(value!);
        }

        public override void Write(in DdbWriter writer, ref string? value)
        {
            if (value == null)
                writer.WriteDdbNull();
            else
                writer.WriteDdbString(value);
        }

        public void WritePropertyName(in DdbWriter writer, ref string value) => writer.JsonWriter.WritePropertyName(value);

        public void WriteStringValue(in DdbWriter writer, ref string value) => writer.JsonWriter.WriteStringValue(value);

        public override string? Read(ref DdbReader reader)
        {
            return reader.AttributeType == AttributeType.Null ? null : reader.JsonReaderValue.GetString()!;
        }
    }
}