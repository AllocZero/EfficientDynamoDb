using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using EfficientDynamoDb.Internal.Constants;

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
            writer.WriteStartObject();
            writer.WritePropertyName(DdbTypeNames.StringSet);
            
            writer.WriteStartArray();

            foreach (var item in _items)
                writer.WriteStringValue(item);
            
            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        public override string ToString() => $"[{string.Join(", ", _items.Select(x => x.ToString()))}]";
    }
}