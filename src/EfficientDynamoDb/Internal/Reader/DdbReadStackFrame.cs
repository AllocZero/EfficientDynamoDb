using System;
using System.Buffers;
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
            
            DocumentBuffer.Dispose();
            
            return document;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AttributeValue[] CreateListFromBuffer(ref ReusableBuffer<KeyValuePair<string, AttributeValue>> buffer)
        {
            if (buffer.RentedBuffer == null)
                return Array.Empty<AttributeValue>();
            
            var array = new AttributeValue[buffer.Index];

            for (var i = 0; i < buffer.Index; i++)
                array[i] = buffer.RentedBuffer[i].Value;

            buffer.Dispose();
            
            return array;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HashSet<string> CreateStringSetFromBuffer(ref ReusableBuffer<KeyValuePair<string, AttributeValue>> buffer)
        {
            if (buffer.RentedBuffer == null)
                return new HashSet<string>();
            
            var set = new HashSet<string>(buffer.Index);

            for (var i = 0; i < buffer.Index; i++)
                set.Add(buffer.RentedBuffer[i].Value._stringValue.Value);

            buffer.Dispose();
            
            return set;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[] CreateNumberArrayFromBuffer(ref ReusableBuffer<KeyValuePair<string, AttributeValue>> buffer)
        {
            if (buffer.RentedBuffer == null)
                return Array.Empty<string>();
            
            var array = new string[buffer.Index];

            for (var i = 0; i < buffer.Index; i++)
                array[i] = buffer.RentedBuffer[i].Value._stringValue.Value;

            buffer.Dispose();
            
            return array;
        }
    }
}