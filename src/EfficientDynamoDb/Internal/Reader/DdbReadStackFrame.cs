using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Internal.Reader
{
    [StructLayout(LayoutKind.Auto)]
    public struct DdbReadStackFrame
    {
        public Document? Document;
        public ReusableBuffer<KeyValuePair<string, AttributeValue>> DocumentBuffer;
        
        public AttributeValue[]? Items;
        public int ItemsIndex;
        
        public string? KeyName;

        public AttributeType AttributeType;

        public int ItemsLength;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsProcessingValue() => KeyName != null;

        public void Reset()
        {
            Document = null;
            KeyName = null;
            Items = null;
            ItemsIndex = default;
            ItemsLength = 0;
            DocumentBuffer = default;
            AttributeType = default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Document? CreateDocumentFromBuffer()
        {
            if (DocumentBuffer.RentedBuffer == null)
                return null;
            
            var document = new Document(DocumentBuffer.Index);
            
            for (var i = 0; i < DocumentBuffer.Index; i++)
            {
                ref var item = ref DocumentBuffer.RentedBuffer![i];
            
                document.Add(item.Key, item.Value);
            }
            
            if(DocumentBuffer.RentedBuffer != null)
                DocumentBuffer.Dispose();
            
            return document;
        }
    }
}