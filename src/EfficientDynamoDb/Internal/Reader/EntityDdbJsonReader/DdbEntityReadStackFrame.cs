using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader.Metadata;

namespace EfficientDynamoDb.Internal.Reader
{
    [StructLayout(LayoutKind.Auto)]
    internal struct DdbEntityReadStackFrame
    {
        public AttributeType AttributeType;
        
        public DdbStackFrameObjectState ObjectState; 
        
        public DdbStackFramePropertyState PropertyState;

        public int BufferLengthHint;
        
        public DdbClassInfo? ClassInfo;
        
        public DdbPropertyInfo? PropertyInfo;

        public DdbClassInfo? NextClassInfo;

        public object? ReturnValue;
        
        public object? DictionaryKey;

        public int CollectionIndex;
        
        public const int DefaultAttributeBufferSize = 32;
        
        public ReusableBuffer<string> StringBuffer;
        public ReusableBuffer<AttributeValue> AttributesBuffer;
        
        public void Reset()
        {
            PropertyInfo = null;
            BufferLengthHint = default;
            AttributeType = default;
            ClassInfo = default;
            PropertyInfo = default;
            ObjectState = default;
            PropertyState = default;
            CollectionIndex = default;
            NextClassInfo = default;
            DictionaryKey = default;
            StringBuffer.Index = 0;
            AttributesBuffer.Index = 0;
        }
    }
}