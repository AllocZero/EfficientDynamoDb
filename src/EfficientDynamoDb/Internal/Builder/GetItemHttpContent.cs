using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Internal.Builder
{
    public class GetItemHttpContent<TPkAttribute, TSkAttribute> : DynamoDbHttpContent
        where TPkAttribute : IAttributeValue
        where TSkAttribute : IAttributeValue
    {
        private readonly string _tableName;
        private readonly string _pkName;
        private readonly TPkAttribute _pkAttributeValue;
        private readonly string _skName;
        private readonly TSkAttribute _skAttributeValue;

        public GetItemHttpContent(string tableName, string pkName, TPkAttribute pkAttributeValue, string skName, TSkAttribute skAttributeValue)
        {
            _tableName = tableName;
            _pkName = pkName;
            _pkAttributeValue = pkAttributeValue;
            _skName = skName;
            _skAttributeValue = skAttributeValue;

            Headers.Add("X-AMZ-Target", "DynamoDB_20120810.GetItem");
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
            
            writer.WritePropertyName("Key");
            writer.WriteStartObject();
            
            writer.WritePropertyName(_pkName);
            _pkAttributeValue.Write(writer);
            
            // TODO: Consider flushing, review JsonSerializer.Write.Helpers.WriteAsyncCore
            // Flushing is not needed for GetItem because JSON should mostly be smaller than DefaultBufferSize, but will be needed for bigger request classes

            writer.WritePropertyName(_skName);
            _skAttributeValue.Write(writer);
            
            writer.WriteEndObject();
            
            writer.WriteString("TableName", _tableName);

            writer.WriteEndObject();
        }
    }
}