using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace EfficientDynamoDb.DocumentModel.AttributeValues
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct ListAttributeValue : IAttributeValue
    {
        [FieldOffset(0)]
        private readonly AttributeValue[] _items;
        
        public AttributeValue[] Items => _items;

        public ListAttributeValue(AttributeValue[] items)
        {
            _items = items;
        }

        public void Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("L");
            
            writer.WriteStartArray();

            foreach (var item in _items)
                item.Write(writer);
            
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
        
        public override string ToString() => $"[{string.Join(", ", _items.Select(x => x.ToString()))}]";
    }
}