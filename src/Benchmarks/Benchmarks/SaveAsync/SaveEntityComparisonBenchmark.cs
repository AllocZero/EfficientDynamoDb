using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using BenchmarkDotNet.Attributes;
using Benchmarks.Mocks;
using Benchmarks.Mocks.Http;
using EfficientDynamoDb;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Configs.Http;
using EfficientDynamoDb.Extensions;
using EfficientDynamoDb.Internal.Crc;

namespace Benchmarks.Benchmarks.SaveAsync
{
    [MemoryDiagnoser]
    public class SaveEntityComparisonBenchmark
    {
        private readonly DynamoDBContext _awsDbContext;
        private readonly DynamoDbContext _efficientDbContext;
        
        private byte[] _describeTableBytes;
        private byte[] _responseContentBytes;
        
        public SaveEntityComparisonBenchmark()
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
        
        [Benchmark(Description = "aws-sdk-net")]
        public async Task AwsSdkNetBenchmark()
        {
            await _awsDbContext.SaveAsync(EntitiesFactory.CreateMixedEntity(0)).ConfigureAwait(false);
        }
        
        [Benchmark(Description = "EfficientDynamoDb")]
        public async Task EfficientDynamoDbBenchmark()
        {
            await _efficientDbContext.SaveAsync(EntitiesFactory.CreateMixedEntity(0)).ConfigureAwait(false);
        }
        
        [GlobalSetup]
        public void SetupMixedBenchmark() 
        {
            _responseContentBytes = UpdateItemResponseFactory.CreateResponse(EntitiesFactory.CreateMixedEntity(1).ToDocument());
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