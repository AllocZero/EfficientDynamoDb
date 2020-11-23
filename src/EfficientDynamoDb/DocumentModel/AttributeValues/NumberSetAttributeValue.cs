using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace EfficientDynamoDb.DocumentModel.AttributeValues
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct NumberSetAttributeValue : IAttributeValue
    {
        [FieldOffset(0)]
        private readonly string[] _items;

        public string[] Items => _items;
        
        public NumberSetAttributeValue(string[] items)
        {
            _items = items;
        }

        public void Write(Utf8JsonWriter writer)
        {
            writer.WritePropertyName("NS");
            
            writer.WriteStartArray();

            // TODO: Call WriteAsync
            foreach (var item in _items)
                writer.WriteStringValue(item);
            
            writer.WriteEndArray();
        }
        
        public override string ToString() => $"[{string.Join(", ", _items)}]";
    }
}