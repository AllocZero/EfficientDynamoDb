using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader.Metadata;

namespace EfficientDynamoDb.Internal.Reader
{
    [StructLayout(LayoutKind.Auto)]
    internal struct DdbEntityReadStackFrame
    {
        public const int DefaultAttributeBufferSize = 32;
        
        public AttributeType AttributeType;

        public int BufferLengthHint;
        
        public DdbClassInfo? ClassInfo;
        
        public DdbPropertyInfo? PropertyInfo;

        public object? ReturnValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsProcessingValue() => PropertyInfo != null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            PropertyInfo = null;
            BufferLengthHint = DefaultAttributeBufferSize;
            AttributeType = default;
            ClassInfo = default;
            PropertyInfo = default;
        }
    }
}