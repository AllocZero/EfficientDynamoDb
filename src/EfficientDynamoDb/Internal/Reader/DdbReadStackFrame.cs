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
        public const int DefaultAttributeBufferSize = 32;
        
        public ReusableBuffer<string> StringBuffer;
        public ReusableBuffer<AttributeValue> AttributesBuffer;

        public string? KeyName;

        public AttributeType AttributeType;

        public int BufferLengthHint;

        public bool ReturnDocuments;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsProcessingValue() => KeyName != null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            KeyName = null;
            BufferLengthHint = DefaultAttributeBufferSize;
            AttributeType = default;
            ReturnDocuments = default;
            StringBuffer.Index = 0;
            AttributesBuffer.Index = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Document? CreateDocumentFromBuffer()
        {
            if (StringBuffer.Index == 0)
                return null;
            
            var document = new Document(StringBuffer.Index);
            
            for (var i = 0; i < StringBuffer.Index; i++)
                document.Add(StringBuffer.RentedBuffer![i], AttributesBuffer.RentedBuffer![i]);

            return document;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AttributeValue[] CreateListFromBuffer(ref ReusableBuffer<AttributeValue> buffer)
        {
            return buffer.RentedBuffer.AsSpan(0, buffer.Index).ToArray();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HashSet<string> CreateStringSetFromBuffer(ref ReusableBuffer<string> buffer)
        {
            if (buffer.Index == 0)
                return new HashSet<string>();
            
            var set = new HashSet<string>(buffer.Index);

            for (var i = 0; i < buffer.Index; i++)
                set.Add(buffer.RentedBuffer![i]);

            return set;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[] CreateNumberArrayFromBuffer(ref ReusableBuffer<string> buffer)
        {
            return buffer.RentedBuffer.AsSpan(0, buffer.Index).ToArray();
        }
    }
}