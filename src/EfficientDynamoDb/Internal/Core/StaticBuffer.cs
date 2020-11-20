using System.Runtime.InteropServices;

namespace EfficientDynamoDb.Internal.Core
{
    [StructLayout(LayoutKind.Auto)]
    public struct StaticBuffer<TValue>
    {
        public TValue[]? Buffer;
        
        public int Index;

        public StaticBuffer(int size)
        {
            Buffer = new TValue[size];
            Index = 0;
        }
    }
}