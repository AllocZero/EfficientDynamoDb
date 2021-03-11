using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.Constants;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.DocumentModel.AttributeValues
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct NumberSetAttributeValue
    {
        [FieldOffset(0)]
        private readonly HashSet<string> _items;

        public HashSet<string> Items => _items;
        
        public NumberSetAttributeValue(HashSet<string> items)
        {
            _items = items;
        }

        public float[] ToFloatArray()
        {
            var result = new float[_items.Count];

            var i = 0;
            foreach (var item in _items)
            {
                result[i++] = float.Parse(item);
            }

            return result;
        }

        public void Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(DdbTypeNames.NumberSet);
            
            writer.WriteStartArray();

            foreach (var item in _items)
                writer.WriteStringValue(item);
            
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
        
        public override string ToString() => $"[{string.Join(", ", _items)}]";
    }
}