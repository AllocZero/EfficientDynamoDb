using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json;
using EfficientDynamoDb.Internal.Constants;

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

        public float[] ToFloatArray()
        {
            var result = new float[_items.Length];
            for (var i = 0; i < _items.Length; i++)
            {
                result[i] = float.Parse(_items[i]);
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