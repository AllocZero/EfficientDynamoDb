using System.Runtime.InteropServices;
using System.Text.Json;
using EfficientDynamoDb.Internal.Constants;

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
            writer.WriteBoolean(DdbTypeNames.Bool, Value);
            writer.WriteEndObject();
        }

        public override string ToString() => Value.ToString();
    }
}