using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Internal.Reader
{
    internal readonly struct ReadResult<TValue> where TValue : class
    {
        public TValue? Value { get; }
        
        public uint Crc { get; }

        public ReadResult(TValue? value, uint crc)
        {
            Value = value;
            Crc = crc;
        }
    }
}