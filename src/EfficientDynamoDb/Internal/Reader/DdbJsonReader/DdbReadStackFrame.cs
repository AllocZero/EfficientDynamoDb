using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Reader.Metadata;

namespace EfficientDynamoDb.Internal.Reader
{
    [StructLayout(LayoutKind.Auto)]
    internal struct DdbReadStackFrame
    {
        public const int DefaultAttributeBufferSize = 32;
        
        public ReusableBuffer<string> StringBuffer;
        public ReusableBuffer<AttributeValue> AttributesBuffer;

        public string? KeyName;

        public AttributeType AttributeType;

        public int BufferLengthHint;

        public JsonObjectMetadata? Metadata;  
        public JsonObjectMetadata? NextMetadata;  

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsProcessingValue() => KeyName != null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            KeyName = null;
            BufferLengthHint = DefaultAttributeBufferSize;
            AttributeType = default;
            Metadata = default;
            NextMetadata = default;
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
        public static Document[] CreateDocumentListFromBuffer(ref ReusableBuffer<AttributeValue> buffer)
        {
            var documents = new Document[buffer.Index];

            for (var i = 0; i < documents.Length; i++)
                documents[i] = buffer.RentedBuffer![i]._mapValue.Value;

            return documents;
        }
    }
}