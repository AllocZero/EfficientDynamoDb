using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Benchmarks.AwsDdbSdk.Entities;
using Benchmarks.Http;
using Benchmarks.Mocks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Crc;

namespace Benchmarks.AwsDdbSdk.Benchmarks
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public abstract class DdbBenchmarkBase
    {
        private byte[] _responseContentBytes;
        
        protected DynamoDBContext DbContext { get; }
        protected AmazonDynamoDBClient DbClient { get; }

        protected abstract Task<IReadOnlyCollection<object>> QueryAsync<T>(string pk) where T: KeysOnlyEntity, new();

        protected DdbBenchmarkBase() => (DbContext, DbClient) = GetContext();

        protected async Task<int> RunBenchmarkAsync<T>(string pk) where T: KeysOnlyEntity, new()
        {
            var result = await QueryAsync<T>(pk).ConfigureAwait(false);

            return result.Count;
        }

        protected void SetupBenchmark<T>(Func<int, Document> entityFactory, int desiredEntitiesCount = 1000) where T: KeysOnlyEntity, new()
        {
            _responseContentBytes = QueryResponseFactory.CreateResponse(entityFactory, desiredEntitiesCount);
        }
        
        protected HttpResponseMessage CreateResponse(HttpRequestMessage request)
        {
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