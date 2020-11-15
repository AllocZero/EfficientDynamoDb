using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace EfficientDynamoDb.DocumentModel.AttributeValues
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct ListAttributeValue : IAttributeValue
    {
        [FieldOffset(0)]
        private readonly List<Document>? _items;
        
        public List<Document>? Items => _items;

        public ListAttributeValue(List<Document>? items)
        {
            _items = items;
        }

        public void Write(Utf8JsonWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}