using System.Runtime.InteropServices;

namespace EfficientDynamoDb.DocumentModel
{
    [StructLayout(LayoutKind.Explicit)]
    internal readonly struct DocumentListAttributeValue
    {
        [FieldOffset(0)]
        private readonly Document[] _items;
        
        public Document[] Items => _items;

        public DocumentListAttributeValue(Document[] items)
        {
            _items = items;
        }
    }
}