using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Json
{
    public static partial class DocumentJsonSerializer
    {
        private const int NullUtf8BytesCount = 4;
        private static readonly byte CommaUtf8Byte = Encoding.UTF8.GetBytes(",")[0];
        
        public static string Serialize(Document document)
        {
            using var buffer = new PooledByteBufferWriter();
            using var writer = new Utf8JsonWriter(buffer);
            
            WriteDocument(buffer, writer, document);

            writer.Flush();

            return Encoding.UTF8.GetString(buffer.WrittenMemory.Span);
        }

        public static string Serialize(IEnumerable<Document> documents)
        {
            using var buffer = new PooledByteBufferWriter();
            using var writer = new Utf8JsonWriter(buffer);

            writer.WriteStartArray();
            
            foreach (var document in documents)
                WriteDocument(buffer, writer, document);
            
            writer.WriteEndArray();

            writer.Flush();

            return Encoding.UTF8.GetString(buffer.WrittenMemory.Span);
        }

        public static async Task SerializeAsync(Stream stream, Document document, CancellationToken cancellationToken = default)
        {
            using var buffer = new PooledByteBufferWriter(stream);
            // ReSharper disable once UseAwaitUsing
            using var writer = new Utf8JsonWriter(buffer);

            writer.WriteStartObject();

            foreach (var (key, attributeValue) in document)
            {
                writer.WritePropertyName(key);

                WriteAttributeValue(buffer, writer, in attributeValue);

                if (buffer.ShouldFlush(writer))
                    await buffer.FlushAsync(writer, cancellationToken).ConfigureAwait(false);
            }
            
            writer.WriteEndObject();
            
            await buffer.FlushAsync(writer, cancellationToken).ConfigureAwait(false);
        }
        
        public static async Task SerializeAsync(Stream stream, IEnumerable<Document> documents, CancellationToken cancellationToken = default)
        {
            using var buffer = new PooledByteBufferWriter(stream);
            // ReSharper disable once UseAwaitUsing
            using var writer = new Utf8JsonWriter(buffer);

            writer.WriteStartArray();

            foreach (var document in documents)
            {
                writer.WriteStartObject();

                foreach (var (key, attributeValue) in document)
                {
                    writer.WritePropertyName(key);

                    WriteAttributeValue(buffer, writer, in attributeValue);

                    if (buffer.ShouldFlush(writer))
                        await buffer.FlushAsync(writer, cancellationToken).ConfigureAwait(false);
                }

                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            
            await buffer.FlushAsync(writer, cancellationToken).ConfigureAwait(false);
        }
        
        private static void WriteDocument(PooledByteBufferWriter buffer, Utf8JsonWriter writer, Document document)
        {
            writer.WriteStartObject();

            foreach (var (key, attributeValue) in document)
            {
                writer.WritePropertyName(key);

                WriteAttributeValue(buffer, writer, in attributeValue);
            }

            writer.WriteEndObject();
        }
    }
}