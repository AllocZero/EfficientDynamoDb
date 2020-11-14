using System.Runtime.InteropServices;
using System.Text.Json;

namespace EfficientDynamoDb.DocumentModel.AttributeValues
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct BoolAttributeValue : IAttributeValue
    {
        private static readonly object TrueValue = new object();
        
        [FieldOffset(0)]
        private readonly object? _value;

        public bool Value => _value == TrueValue;

        public BoolAttributeValue(bool value)
        {
            _value = value ? TrueValue : null;
        }

        public void Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteBoolean("BOOL", Value);
            writer.WriteEndObject();
        }
    }
}