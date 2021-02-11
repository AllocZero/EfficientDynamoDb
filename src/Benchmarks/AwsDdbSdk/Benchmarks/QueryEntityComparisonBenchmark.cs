using System;
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
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Configs.Http;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.DescribeTable.Models;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Crc;
using EfficientDynamoDb.Internal.JsonConverters;
using KeyType = EfficientDynamoDb.Context.Operations.DescribeTable.Models.Enums.KeyType;

namespace Benchmarks.AwsDdbSdk.Benchmarks
{
    [MemoryDiagnoser]
    public class QueryEntityComparisonBenchmark
    {
        [Params(10, 100, 1000)]
        public int EntitiesCount;
        
        private byte[] _responseContentBytes;
        private byte[] _describeTableBytes;

        private readonly DynamoDBContext _awsDbContext;
        private readonly DynamoDbContext _efficientDbContext;

        public QueryEntityComparisonBenchmark()
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

            _awsDbContext = new DynamoDBContext(dbClient, contextConfig);
            _efficientDbContext = new DynamoDbContext(new DynamoDbContextConfig(EfficientDynamoDb.Context.Config.RegionEndpoint.USEast1, new AwsCredentials("test", "test"))
            {
                HttpClientFactory = new DefaultHttpClientFactory(new HttpClient(new MockHttpClientHandler(CreateResponse)))
            });
        }
        
        [GlobalSetup]
        public void SetupMixedBenchmark() => SetupBenchmark<MixedEntity>(x => EntitiesFactory.CreateMixedEntity(x).ToDocument());

        [Benchmark(Description = "EfficientDynamoDb")]
        public async Task<int> EfficientDynamoDbBenchmark()
        {
            var result = await _efficientDbContext.Query<MixedEntity>()
                .WithKeyExpression(Filter<MixedEntity>.On(x => x.Pk).EqualsTo("test"))
                .ToListAsync().ConfigureAwait(false);

            return result.Count;
        }
        
        [Benchmark(Description = "aws-sdk-net")]
        public async Task<int> AwsSdkNetBenchmark()
        {
            var result = await _awsDbContext.QueryAsync<MixedEntity>("test").GetRemainingAsync().ConfigureAwait(false);
            return result.Count;
        }

        private void SetupBenchmark<T>(Func<int, Document> entityFactory) where T: KeysOnlyEntity, new()
        {
            _responseContentBytes = QueryResponseFactory.CreateResponse(entityFactory, EntitiesCount);
            _describeTableBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new EfficientDynamoDb.Context.Operations.DescribeTable.DescribeTableResponse(new EfficientDynamoDb.Context.Operations.DescribeTable.Models.TableDescription
            {
                TableName = "production_" + Tables.TestTable,
                KeySchema = new[] {new KeySchemaElement("pk", EfficientDynamoDb.Context.Operations.DescribeTable.Models.Enums.KeyType.HASH), new KeySchemaElement("sk", KeyType.RANGE)},
                AttributeDefinitions = new[] {new AttributeDefinition("pk", "S"), new AttributeDefinition("sk", "S")}
            }), new JsonSerializerOptions
            {
                Converters = { new DdbEnumJsonConverterFactory()}
            }));
        }
        
        private HttpResponseMessage CreateResponse(HttpRequestMessage request)
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
    }
}