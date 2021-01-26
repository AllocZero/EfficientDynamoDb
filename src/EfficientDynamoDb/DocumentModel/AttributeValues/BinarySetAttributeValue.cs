using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.DocumentModel.AttributeValues
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct BinarySetAttributeValue
    {
        [FieldOffset(0)] 
        private readonly List<byte[]> _items;

        public List<byte[]> Items => _items;

        public BinarySetAttributeValue(List<byte[]> items)
        {
            _items = items;
        }
        
        public void Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(DdbTypeNames.BinarySet);
            
            writer.WriteStartArray();
            
            foreach (var value in _items)
                writer.WriteBase64StringValue(value);
            
            writer.WriteEndArray();
            
            writer.WriteEndObject();
        }

        public override string ToString() =>  $"[{string.Join(", ", _items.Select(Convert.ToBase64String))}]";
    }
}