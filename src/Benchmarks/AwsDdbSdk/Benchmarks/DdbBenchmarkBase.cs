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
using Benchmarks.AwsDdbSdk.Constants;
using Benchmarks.AwsDdbSdk.Entities;
using Benchmarks.Http;
using Benchmarks.Mocks;
using EfficientDynamoDb.Context.Operations.DescribeTable;
using EfficientDynamoDb.Context.Operations.DescribeTable.Models;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Crc;
using EfficientDynamoDb.Internal.JsonConverters;
using KeyType = EfficientDynamoDb.Context.Operations.DescribeTable.Models.Enums.KeyType;

namespace Benchmarks.AwsDdbSdk.Benchmarks
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public abstract class DdbBenchmarkBase
    {
        [Params(10, 100, 1000)]
        public int EntitiesCount;
        
        private byte[] _responseContentBytes;
        private byte[] _describeTableBytes;
        
        protected DynamoDBContext DbContext { get; }
        protected AmazonDynamoDBClient DbClient { get; }

        protected abstract Task<IReadOnlyCollection<object>> QueryAsync<T>(string pk) where T: KeysOnlyEntity, new();

        protected DdbBenchmarkBase() => (DbContext, DbClient) = GetContext();

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
                KeySchema = new[] {new KeySchemaElement("pk", KeyType.HASH), new KeySchemaElement("sk", KeyType.RANGE)},
                AttributeDefinitions = new[] {new AttributeDefinition("pk", "S"), new AttributeDefinition("sk", "S")}
            }), new JsonSerializerOptions
            {
                Converters = { new DdbEnumJsonConverterFactory()}
            }));
        }
        
        protected HttpResponseMessage CreateResponse(HttpRequestMessage request)
        {
            if(request.Headers.GetValues("X-AMZ-Target").First().Contains("DescribeTable"))
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