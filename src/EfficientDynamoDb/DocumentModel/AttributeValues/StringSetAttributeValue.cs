using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace EfficientDynamoDb.DocumentModel.AttributeValues
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct StringSetAttributeValue : IAttributeValue
    {
        [FieldOffset(0)] 
        private readonly HashSet<string> _items;

        public HashSet<string> Items => _items;

        public StringSetAttributeValue(HashSet<string> items)
        {
            _items = items;
        }

        public void Write(Utf8JsonWriter writer)
        {
            writer.WritePropertyName("SS");
            
            writer.WriteStartArray();

            // TODO: Call WriteAsync
            foreach (var item in _items)
                writer.WriteStringValue(item);
            
            writer.WriteEndArray();
        }
    }
}