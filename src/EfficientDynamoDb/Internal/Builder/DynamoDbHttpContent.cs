using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
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
    }
}