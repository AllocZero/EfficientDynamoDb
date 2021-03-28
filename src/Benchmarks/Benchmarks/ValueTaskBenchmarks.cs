using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Core;

namespace Benchmarks
{
    public class ValueTaskBenchmarks
    {
        private AttributeValue[] _listInput;

        [GlobalSetup]
        public void Setup()
        {
            var random = new Random();
            _listInput = Enumerable.Range(0, 100)
                .Select(_ => new AttributeValue(new StringAttributeValue(new string('x', random.Next(1, 10)))))
                .ToArray();
        }

        [Benchmark]
        public async Task<long> SyncBenchmark()
        {
            await using var stream = new MemoryStream();
            using var bufferWriter = new PooledByteBufferWriter(stream, 16 * 1024);
            await using var writer = new Utf8JsonWriter(bufferWriter);
            
            WriteData(writer, bufferWriter);

            await bufferWriter.WriteToStreamAsync().ConfigureAwait(false);

            return stream.Length;
        }
        
        [Benchmark]
        public async Task<long> AsyncBenchmark()
        {
            await using var stream = new MemoryStream();
            using var bufferWriter = new PooledByteBufferWriter(stream, 160 * 1024);
            await using var writer = new Utf8JsonWriter(bufferWriter);
            
            await WriteDataAsync(writer, bufferWriter).ConfigureAwait(false);

            await bufferWriter.WriteToStreamAsync().ConfigureAwait(false);

            return stream.Length;
        }
        
        private void WriteData(Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter)
        {
            writer.WriteStartObject();

            for (var i = 0; i < 100; i++)
            {
                writer.WritePropertyName($"test_i");
                WriteValue(writer, bufferWriter, _listInput[i].AsStringAttribute());
            }
            
            writer.WriteEndObject();
        }
        
        private async ValueTask WriteDataAsync(Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter)
        {
            writer.WriteStartObject();

            for (var i = 0; i < 100; i++)
            {
                writer.WritePropertyName($"test_i");
                await WriteValueAsync(writer, bufferWriter, _listInput[i].AsStringAttribute()).ConfigureAwait(false);
            }
            
            writer.WriteEndObject();
        }

        private void WriteValue(Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter, StringAttributeValue attributeValue)
        {
            writer.WriteStringValue(attributeValue.Value);
            
            if (bufferWriter.ShouldWrite(writer))
                bufferWriter.WriteToStreamAsync();
        }
        
        private async ValueTask WriteValueAsync(Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter, StringAttributeValue attributeValue)
        {
            writer.WriteStringValue(attributeValue.Value);
            
            if (bufferWriter.ShouldWrite(writer))
                await bufferWriter.WriteToStreamAsync().ConfigureAwait(false);
        }
    }
}