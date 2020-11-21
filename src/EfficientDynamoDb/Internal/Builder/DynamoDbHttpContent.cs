using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal.Core;
using Microsoft.IO;

namespace EfficientDynamoDb.Internal.Builder
{
    public abstract class DynamoDbHttpContent : HttpContent
    {
        protected const int DefaultBufferSize = 16 * 1024;
        private const int DefaultFlushThreshold = (int) (DefaultBufferSize * 0.9);
        
        private static readonly RecyclableMemoryStreamManager MemoryStreamManager = new RecyclableMemoryStreamManager();
        protected static readonly JsonWriterOptions JsonWriterOptions = new JsonWriterOptions {SkipValidation = true};
        
        private MemoryStream? _pooledContentStream;

        protected DynamoDbHttpContent(string amzTarget)
        {
            Headers.Add("X-AMZ-Target", amzTarget);
            Headers.ContentType = new MediaTypeHeaderValue("application/x-amz-json-1.0");
        }

        protected override Task<Stream> CreateContentReadStreamAsync()
        {
            return GlobalDynamoDbConfig.UseMemoryStreamPooling ? CreatePooledContentReadStreamAsync() : base.CreateContentReadStreamAsync();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            
            if(disposing)
                _pooledContentStream?.Dispose();
        }
        
        private async Task<Stream> CreatePooledContentReadStreamAsync()
        {
            if (_pooledContentStream != null)
                return _pooledContentStream;

            _pooledContentStream = MemoryStreamManager.GetStream();

            await SerializeToStreamAsync(_pooledContentStream, null).ConfigureAwait(false);

            _pooledContentStream.Position = 0;
            return _pooledContentStream;
        }
        
        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext? context)
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

        protected abstract void WriteData(Utf8JsonWriter writer);
    }
}