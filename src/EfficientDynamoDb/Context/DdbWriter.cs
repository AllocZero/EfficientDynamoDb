using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context
{
    public readonly struct DdbWriter
    {
        public readonly Utf8JsonWriter JsonWriter;

        internal readonly PooledByteBufferWriter BufferWriter;

        internal DdbWriter(Utf8JsonWriter jsonJsonWriter, PooledByteBufferWriter bufferWriter)
        {
            JsonWriter = jsonJsonWriter;
            BufferWriter = bufferWriter;
        }

        public bool ShouldFlush => BufferWriter.ShouldWrite(JsonWriter);

        public ValueTask FlushAsync() => BufferWriter.WriteToStreamAsync();
    }
}