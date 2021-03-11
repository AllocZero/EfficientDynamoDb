using System.Runtime.InteropServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.Constants;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.DocumentModel.AttributeValues
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct NullAttributeValue
    {
        private static readonly object NullValue = new object();
        
        [FieldOffset(0)]
        private readonly object? _value;

        public bool IsNull => _value == NullValue;

        public NullAttributeValue(bool _)
        {
            _value = NullValue;
        }

        public void Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WriteBoolean(DdbTypeNames.Null, true);
            
            writer.WriteEndObject();
        }
        
        public override string ToString() => "null";
    }
}