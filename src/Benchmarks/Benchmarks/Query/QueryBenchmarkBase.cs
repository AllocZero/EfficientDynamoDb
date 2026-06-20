using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Benchmarks.Constants;
using Benchmarks.Entities;
using Benchmarks.Mocks;
using Benchmarks.Mocks.Http;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Crc;
using EfficientDynamoDb.Internal.JsonConverters;
using EfficientDynamoDb.Operations.DescribeTable;
using EfficientDynamoDb.Operations.DescribeTable.Models;
using KeyType = EfficientDynamoDb.Operations.DescribeTable.Models.Enums.KeyType;

namespace Benchmarks.Query
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public abstract class QueryBenchmarkBase
    {
        private const string KeysOnlyEntityPk = "keys_only_bench";
        private const string MediumEntityPk = "medium_bench_v4";
        private const string MediumComplexEntityPk = "medium_complex_bench_v4";
        private const string LargeEntityPk = "large_bench";
        private const string MixedEntityPk = "mixed_bench_v2";
        
        [Params(10, 100, 1000)]
        public int EntitiesCount;
        
        private byte[] _responseContentBytes;
        private byte[] _describeTableBytes;
        
        protected DynamoDBContext DbContext { get; }
        protected AmazonDynamoDBClient DbClient { get; }

        protected abstract Task<IReadOnlyCollection<object>> QueryAsync<T>(string pk) where T: KeysOnlyEntity, new();

        protected QueryBenchmarkBase() => (DbContext, DbClient) = GetContext();
        
        [GlobalSetup(Target = nameof(KeysOnlyBenchmarkAsync))]
        public void SetupKeysOnlyBenchmark() => SetupBenchmark<KeysOnlyEntity>(x => EntitiesFactory.CreateKeysOnlyEntity(x).ToDocument());

        [Benchmark(Baseline = true)]
        public Task<int> KeysOnlyBenchmarkAsync() => RunBenchmarkAsync<KeysOnlyEntity>(KeysOnlyEntityPk);

        [GlobalSetup(Target = nameof(MediumBenchmarkAsync))]
        public void SetupMediumBenchmark() => SetupBenchmark<MediumStringFieldsEntity>(x => EntitiesFactory.CreateMediumStringEntity(x).ToDocument());
        
        [Benchmark]
        public Task<int> MediumBenchmarkAsync() => RunBenchmarkAsync<MediumStringFieldsEntity>(MediumEntityPk);
        
        [GlobalSetup(Target = nameof(MediumComplexBenchmarkAsync))]
        public void SetupComplexBenchmark() => SetupBenchmark<MediumComplexFieldsEntity>(x => EntitiesFactory.CreateMediumComplexEntity(x).ToDocument());
        
        [Benchmark]
        public Task<int> MediumComplexBenchmarkAsync() => RunBenchmarkAsync<MediumComplexFieldsEntity>(MediumComplexEntityPk);
        
        [GlobalSetup(Target = nameof(LargeBenchmarkAsync))]
        public void SetupLargeBenchmark() => SetupBenchmark<LargeStringFieldsEntity>(x => EntitiesFactory.CreateLargeStringEntity(x).ToDocument());
        
        [Benchmark]
        public Task<int> LargeBenchmarkAsync() => RunBenchmarkAsync<LargeStringFieldsEntity>(LargeEntityPk);
        
        [GlobalSetup(Target = nameof(MixedBenchmarkAsync))]
        public void SetupMixedBenchmark() => SetupBenchmark<MixedEntity>(x => EntitiesFactory.CreateMixedEntity(x).ToDocument());
        
        [Benchmark]
        public Task<int> MixedBenchmarkAsync() => RunBenchmarkAsync<MixedEntity>(MixedEntityPk);

        protected async Task<int> RunBenchmarkAsync<T>(string pk) where T: KeysOnlyEntity, new()
        {
            var result = await QueryAsync<T>(pk).ConfigureAwait(false);

            return result.Count;
        }

        protected void SetupBenchmark<T>(Func<int, Document> entityFactory) where T: KeysOnlyEntity, new()
        {
            _responseContentBytes = QueryResponseFactory.CreateResponse(entityFactory, EntitiesCount);
            _describeTableBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new DescribeTableResponse(new TableDescription
            {
                TableName = "production_" + Tables.TestTable,
                KeySchema = new[] {new KeySchemaElement("pk", KeyType.Hash), new KeySchemaElement("sk", KeyType.Range)},
                AttributeDefinitions = new[] {new AttributeDefinition("pk", "S"), new AttributeDefinition("sk", "S")}
            }), new JsonSerializerOptions
            {
                Converters = { new DdbEnumJsonConverterFactory()}
            }));
        }
        
        protected HttpResponseMessage CreateResponse(HttpRequestMessage request)
        {
            if(request.Headers.Contains("X-AMZ-Target") && request.Headers.GetValues("X-AMZ-Target").First().Contains("DescribeTable"))
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(_describeTableBytes),
                    Headers = {{"x-amz-crc32", Crc32Algorithm.Compute(_describeTableBytes).ToString()}}
                };
            
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(_responseContentBytes),
                Headers = {{"x-amz-crc32", Crc32Algorithm.Compute(_responseContentBytes).ToString()}}
            };
        }
        
        private (DynamoDBContext dbContext, AmazonDynamoDBClient dbClient) GetContext()
        {
            var ddbConfig = new AmazonDynamoDBConfig {RegionEndpoint = RegionEndpoint.USEast1, HttpClientFactory = new MockHttpClientFactory(CreateResponse)};
            var dbClient = new AmazonDynamoDBClient(
                new BasicAWSCredentials(Environment.GetEnvironmentVariable("DEV_AWS_PUBLIC_KEY"), Environment.GetEnvironmentVariable("DEV_AWS_PRIVATE_KEY")),
                ddbConfig);

            var contextConfig = new DynamoDBContextConfig
            {
                TableNamePrefix = "production_",
                Conversion = DynamoDBEntryConversion.V2
            };

            return (new DynamoDBContext(dbClient, contextConfig), dbClient);
        }
    }
}