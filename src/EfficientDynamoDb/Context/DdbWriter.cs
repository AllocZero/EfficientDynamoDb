using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal.Constants;
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
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteDdbNull()
        {
            JsonWriter.WriteStartObject();
            JsonWriter.WriteBoolean(DdbTypeNames.Null, true);
            JsonWriter.WriteEndObject();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteDdbString(string value)
        {
            JsonWriter.WriteStartObject();
            JsonWriter.WriteString(DdbTypeNames.String, value);
            JsonWriter.WriteEndObject();
        }
    }
}