using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Internal.Reader
{
    public readonly struct DocumentResult
    {
        public Document? Value { get; }
        
        public uint Crc { get; }

        public DocumentResult(Document? value, uint crc)
        {
            Value = value;
            Crc = crc;
        }
    }
}