using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace EfficientDynamoDb.DocumentModel
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct ListAttributeValue
    {
        [FieldOffset(0)]
        private readonly List<AttributeValue> _items;
        
        public List<AttributeValue> Items => _items;

        public ListAttributeValue(List<AttributeValue> items)
        {
            _items = items;
        }

        public void Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(DdbTypeNames.List);
            
            writer.WriteStartArray();

            foreach (var item in _items)
                item.Write(writer);
            
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
        
        public override string ToString() => $"[{string.Join(", ", _items.Select(x => x.ToString()))}]";
    }
}