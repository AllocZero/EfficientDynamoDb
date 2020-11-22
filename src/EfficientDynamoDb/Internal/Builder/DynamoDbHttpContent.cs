using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel.AttributeValues;
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
            // Pooled buffer may seems redundant while reviewing current method, but when passed to json writer it completely changes the write logic.
            // Instead of reallocating new in-memory arrays when json size grows and Flush is not called explicitly - it now uses pooled buffer.
            // With proper flushing logic amount of buffer growths/copies should be zero and amount of memory allocations should be zero as well.
            using var bufferWriter = new PooledByteBufferWriter(stream, DefaultBufferSize);
            await using var writer = new Utf8JsonWriter(bufferWriter);

            await WriteDataAsync(writer, bufferWriter).ConfigureAwait(false);

            await bufferWriter.WriteToStreamAsync().ConfigureAwait(false);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return false;
        }

        /// <summary>
        /// When implementation suspects that amount of bytes written is close to 16KB, it should call <see cref="PooledByteBufferWriter.ShouldWrite"/> and in case of true result - call <see cref="PooledByteBufferWriter.WriteToStreamAsync"/>. <br/><br/>
        /// Keep in mind that <see cref="ValueTask"/> calls have a certain overhead, so we only want keep asynchronous writes for request content builders. <br/>
        /// Write implementations of <see cref="IAttributeValue"/> continue to be synchronous and in general are not expected to produce JSON bigger than 16KB.
        /// </summary>
        protected abstract ValueTask WriteDataAsync(Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter);
    }
}