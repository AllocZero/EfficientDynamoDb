using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Json
{
    public static partial class DocumentJsonSerializer
    {
        private const int MaxInitialBufferSize = 1024 * 1024;
        private static readonly DynamoDbContextMetadata EmptyMetadata = new DynamoDbContextMetadata(Array.Empty<DdbConverter>());
        private static readonly DdbClassInfo DocumentClassInfo = EmptyMetadata.GetOrAddClassInfo(typeof(Document), typeof(DocumentJsonRootConverter));
        private static readonly DdbClassInfo DocumentArrayClassInfo = EmptyMetadata.GetOrAddClassInfo(typeof(Document), typeof(DocumentArrayJsonRootConverter));
        
        public static Document Deserialize(string json)
        {
            var stringStream = new Utf8StringStream(json);
            return DeserializeAsync(stringStream, Math.Min((int) stringStream.Length, MaxInitialBufferSize)).Result;
        }
        
        public static IReadOnlyList<Document> DeserializeArray(string json)
        {
            var stringStream = new Utf8StringStream(json);
            return DeserializeArrayAsync(stringStream, Math.Min((int) stringStream.Length, MaxInitialBufferSize)).Result;
        }

        public static async Task<Document> DeserializeAsync(Stream utf8JsonStream, CancellationToken cancellationToken = default)
            => await DeserializeAsync(utf8JsonStream, EntityDdbJsonReader.DefaultBufferSize, cancellationToken).ConfigureAwait(false);
        
        public static async Task<IReadOnlyList<Document>> DeserializeArrayAsync(Stream utf8JsonStream, CancellationToken cancellationToken = default)
            => await DeserializeArrayAsync(utf8JsonStream, EntityDdbJsonReader.DefaultBufferSize, cancellationToken).ConfigureAwait(false);

        private static async ValueTask<Document> DeserializeAsync(Stream utf8JsonStream, int defaultBufferSize, CancellationToken cancellationToken = default)
        {
            var result = await EntityDdbJsonReader.ReadAsync<Document>(utf8JsonStream, DocumentClassInfo, EmptyMetadata, false, defaultBufferSize, cancellationToken)
                .ConfigureAwait(false);
            
            return result.Value ?? throw new InvalidOperationException("JSON does not contain a valid document.");
        }
        
        private static async ValueTask<IReadOnlyList<Document>> DeserializeArrayAsync(Stream utf8JsonStream, int defaultBufferSize, CancellationToken cancellationToken = default)
        {
            var result = await EntityDdbJsonReader.ReadAsync<IReadOnlyList<Document>>(utf8JsonStream, DocumentArrayClassInfo, EmptyMetadata, false, defaultBufferSize, cancellationToken)
                .ConfigureAwait(false);
            
            return result.Value ?? throw new InvalidOperationException("JSON does not contain a valid document.");
        }

        private sealed class Utf8StringStream : Stream
        {
            private readonly string _value;

            public Utf8StringStream(string value)
            {
                _value = value;
                Length = Encoding.UTF8.GetByteCount(value);
            }

            public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = new CancellationToken())
            {
                var left = Length - Position;
                var toRead = Math.Min((int) left, buffer.Length);

                var bytesRead = Encoding.UTF8.GetBytes(_value.AsSpan((int) Position, toRead), buffer.Span);
                Position += bytesRead;
                return new ValueTask<int>(bytesRead);
            }

            public override void Flush() => throw new NotSupportedException();

            public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

            public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

            public override void SetLength(long value) => throw new NotSupportedException();

            public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

            public override bool CanRead => true;
            public override bool CanSeek => false;
            public override bool CanWrite => false;
            
            public override long Length { get; }
            
            public override long Position { get; set; }
        }
    }
}