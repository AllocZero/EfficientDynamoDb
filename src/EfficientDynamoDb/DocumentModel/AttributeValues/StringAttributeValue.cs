using System.Runtime.InteropServices;
using System.Text.Json;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.DocumentModel.AttributeValues
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct StringAttributeValue : IAttributeValue
    {
        [FieldOffset(0)]
        private readonly string _value;

        public string Value => _value;

        public StringAttributeValue(string value)
        {
            _value = value;
        }

        public void Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.String, _value);
            writer.WriteEndObject();
        }
        
        public override string ToString() => _value;
    }
}