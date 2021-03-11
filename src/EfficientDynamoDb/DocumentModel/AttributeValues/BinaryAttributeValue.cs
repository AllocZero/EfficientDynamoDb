using System;
using System.Runtime.InteropServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.Constants;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.DocumentModel.AttributeValues
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct BinaryAttributeValue
    {
        [FieldOffset(0)] 
        private readonly byte[] _value;

        public byte[] Value => _value;

        public BinaryAttributeValue(byte[] value)
        {
            _value = value;
        }
        
        public void Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteBase64String(DdbTypeNames.Binary, _value);
            writer.WriteEndObject();
        }

        public override string ToString() => Convert.ToBase64String(_value);
    }
}