using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal.Builder;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Api.DescribeTable
{
    public class DescribeTableRequestHttpContent : DynamoDbHttpContent
    {
        public string TableName { get; }

        public DescribeTableRequestHttpContent(string tableName)
        {
            TableName = tableName;
            
            Headers.Add("X-AMZ-Target", "DynamoDB_20120810.DescribeTable");
            Headers.ContentType = new MediaTypeHeaderValue("application/x-amz-json-1.0");
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            if (GlobalDynamoDbConfig.UsePooledBufferForJsonWrites)
            {
                // Pooled buffer may seems redundant while reviewing current method, but when passed to json writer it completely changes the write logic.
                // Instead of reallocating new in-memory arrays when json size grows and Flush is not called explicitly - it now uses pooled buffer.
                // With proper flushing logic amount of buffer growths/copies should be zero and amount of memory allocations should be zero as well.
                using var pooledBufferWriter = new PooledByteBufferWriter(DefaultBufferSize);
                await using var writer = new Utf8JsonWriter(pooledBufferWriter);
                
                WriteData(writer);
                
                // Call sync because we are flushing to in-memory buffer
                // ReSharper disable once MethodHasAsyncOverload
                writer.Flush();
                await pooledBufferWriter.WriteToStreamAsync(stream, CancellationToken.None).ConfigureAwait(false);
                
                await stream.FlushAsync().ConfigureAwait(false);
            }
            else
            {
                await using var writer = new Utf8JsonWriter(stream, JsonWriterOptions);
                
                WriteData(writer);
                await stream.FlushAsync().ConfigureAwait(false);
            }
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return false;
        }
        
        private void WriteData(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteString("TableName", TableName);
            writer.WriteEndObject();
        }
    }
}