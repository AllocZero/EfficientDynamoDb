using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Benchmarks.Http;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Configs.Http;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.Config;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace Benchmarks.AwsDdbSdk.Benchmarks
{
    public class EfficientQueryBenchmark : QueryBenchmarkBase
    {
        private readonly DynamoDbContext _efficientContext;
        
        public EfficientQueryBenchmark()
        {
            _efficientContext = new DynamoDbContext(new DynamoDbContextConfig(RegionEndpoint.USEast1, new AwsCredentials("test", "test"))
            {
                HttpClientFactory = new DefaultHttpClientFactory(new HttpClient(new MockHttpClientHandler(CreateResponse)))
            });
        }

        protected override async Task<IReadOnlyCollection<object>> QueryAsync<T>(string pk)
        {
            var result = await _efficientContext.QueryAsync(new QueryRequest
            {
                KeyConditionExpression = "pk = :pk",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":pk", "test"}
                }
            }).ConfigureAwait(false);

            return result.Items;
        }
    }
}