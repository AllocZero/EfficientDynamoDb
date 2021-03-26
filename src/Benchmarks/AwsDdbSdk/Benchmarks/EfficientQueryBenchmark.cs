using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Benchmarks.Http;
using EfficientDynamoDb;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Configs.Http;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Operations.Query;

namespace Benchmarks.AwsDdbSdk.Benchmarks
{
    public class EfficientQueryBenchmark : QueryBenchmarkBase
    {
        private readonly DynamoDbLowLevelContext _efficientLowLevelContext;
        
        public EfficientQueryBenchmark()
        {
            var context = new DynamoDbContext(new DynamoDbContextConfig(RegionEndpoint.USEast1, new AwsCredentials("test", "test"))
            {
                HttpClientFactory = new DefaultHttpClientFactory(new HttpClient(new MockHttpClientHandler(CreateResponse)))
            });
            _efficientLowLevelContext = context.LowContext;
        }

        protected override async Task<IReadOnlyCollection<object>> QueryAsync<T>(string pk)
        {
            var result = await _efficientLowLevelContext.QueryAsync(new QueryRequest
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