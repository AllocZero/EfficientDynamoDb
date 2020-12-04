using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model.Internal.MarshallTransformations;
using Amazon.Runtime.Internal.Transform;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Benchmarks.AwsDdbSdk.Benchmarks.Deserialization.Models;
using EfficientDynamoDb;
using EfficientDynamoDb.Internal.Operations.Query;
using EfficientDynamoDb.Internal.Reader;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Benchmarks.AwsDdbSdk.Benchmarks.Deserialization
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public abstract class DeserializationBenchmarkBase<TModel>
    {
        private string _json;
        private byte[] _jsonBytes;
        private QueryResponseUnmarshaller _unmarshaller;
        
        [GlobalSetup]
        public void Setup()
        {
            const int itemsCount = 1000;
            
            _unmarshaller = new QueryResponseUnmarshaller();
            _jsonBytes = GenerateQueryResponseJson(itemsCount);
            _json = Encoding.UTF8.GetString(_jsonBytes);

            GlobalDynamoDbConfig.InternAttributeNames = true;
        }

        [GlobalSetup(Target = nameof(EfficientReaderWithoutInternBenchmark))]
        public void SetupWithoutIntern()
        {
            Setup();
            
            GlobalDynamoDbConfig.InternAttributeNames = false;
        }
        
        [Benchmark]
        public int NewtonsoftBenchmark()
        {
            var entities = JsonConvert.DeserializeObject<QueryModel<TModel>>(_json);
            return entities.Count;
        }
        
        [Benchmark()]
        public int TextJsonBenchmark()
        {
            var entities = JsonSerializer.Deserialize<QueryModel<TModel>>(_json);
            
            return entities!.Count;
        }

        [Benchmark]
        public async Task<int> EfficientReaderBenchmark()
        {
            var items = await DdbJsonReader.ReadAsync(new MemoryStream(_jsonBytes), QueryParsingOptions.Instance, false).ConfigureAwait(false);

            return items.Value!.Count;
        }
        
        [Benchmark]
        public async Task<int> EfficientReaderWithoutInternBenchmark()
        {
            var items = await DdbJsonReader.ReadAsync(new MemoryStream(_jsonBytes), QueryParsingOptions.Instance, false).ConfigureAwait(false);

            return items.Value!.Count;
        }

        [Benchmark]
        public object AwsUnmarshallerBenchmark()
        {
            return _unmarshaller.Unmarshall(new JsonUnmarshallerContext(new MemoryStream(_jsonBytes), false, null, false));
        }

        protected abstract void WriteEntity(Utf8JsonWriter writer, int index);

        private byte[] GenerateQueryResponseJson(int itemsCount)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);
           
            writer.WriteStartObject();
            writer.WriteNumber("Count", itemsCount);
           
            writer.WritePropertyName("Items");
            writer.WriteStartArray();

            for (var i = 0; i < itemsCount; i++)
                WriteEntity(writer, i);
            
            writer.WriteEndArray();

            writer.WriteNumber("ScannedCount", itemsCount);
            
            writer.WriteEndObject();
            writer.Flush();

            stream.Position = 0;
            return stream.ToArray();
        }
    }
}