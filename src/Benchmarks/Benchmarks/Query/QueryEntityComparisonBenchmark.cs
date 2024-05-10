using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using BenchmarkDotNet.Attributes;
using Benchmarks.Entities;
using Benchmarks.Mocks;
using Benchmarks.Mocks.Http;
using EfficientDynamoDb;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Configs.Http;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Crc;
using RegionEndpoint = EfficientDynamoDb.Configs.RegionEndpoint;

namespace Benchmarks.Query
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
            var ddbConfig = new AmazonDynamoDBConfig {RegionEndpoint = Amazon.RegionEndpoint.USEast1, HttpClientFactory = new MockHttpClientFactory(CreateResponse)};
            var dbClient = new AmazonDynamoDBClient(
                new BasicAWSCredentials(Environment.GetEnvironmentVariable("DEV_AWS_PUBLIC_KEY"), Environment.GetEnvironmentVariable("DEV_AWS_PRIVATE_KEY")),
                ddbConfig);

            var contextConfig = new DynamoDBContextConfig
            {
                TableNamePrefix = "production_",
                Conversion = DynamoDBEntryConversion.V2
            };

            _awsDbContext = new DynamoDBContext(dbClient, contextConfig);
            _efficientDbContext = new DynamoDbContext(new DynamoDbContextConfig(RegionEndpoint.USEast1, new AwsCredentials("test", "test"))
            {
                HttpClientFactory = new DefaultHttpClientFactory(new HttpClient(new MockHttpClientHandler(CreateResponse)))
            });
        }
        
        [GlobalSetup(Target = nameof(EfficientDynamoDbFromInterfaceBenchmark))]
        public void SetupMixedFromInterfaceBenchmark() => SetupBenchmark<MixedEntityFromInterface>(x => EntitiesFactory.CreateMixedEntityFromInterface(x).ToDocument());

        [GlobalSetup]
        public void SetupMixedBenchmark() => SetupBenchmark<MixedEntity>(x => EntitiesFactory.CreateMixedEntity(x).ToDocument());

        [Benchmark(Description = "EfficientDynamoDb")]
        public async Task<int> EfficientDynamoDbBenchmark()
        {
            var result = await _efficientDbContext.Query<MixedEntity>()
                .WithKeyExpression(Condition<MixedEntity>.On(x => x.Pk).EqualTo("test"))
                .ToListAsync().ConfigureAwait(false);

            return result.Count;
        }
        
        [Benchmark(Description = "EfficientDynamoDb-FromInterface")]
        public async Task<int> EfficientDynamoDbFromInterfaceBenchmark()
        {
            var result = await _efficientDbContext.Query<MixedEntityFromInterface>()
                .WithKeyExpression(Condition<MixedEntityFromInterface>.On(x => x.Pk).EqualTo("test"))
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
            _describeTableBytes = DescribeTableResponseFactory.CreateResponse();
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