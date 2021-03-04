using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Benchmarks.Http;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Configs.Http;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.Config;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace Benchmarks.AwsDdbSdk.Benchmarks
{
    public class EfficientEntityQueryBenchmark : QueryBenchmarkBase
    {
        private readonly DynamoDbContext _context;
        
        public EfficientEntityQueryBenchmark()
        {
            _context = new DynamoDbContext(new DynamoDbContextConfig(RegionEndpoint.USEast1, new AwsCredentials("test", "test"))
            {
                HttpClientFactory = new DefaultHttpClientFactory(new HttpClient(new MockHttpClientHandler(CreateResponse)))
            });
        }
    
        protected override async Task<IReadOnlyCollection<object>> QueryAsync<T>(string pk)
        {
            return await _context.Query<T>()
                .WithKeyExpression(Condition<T>.On(x => x.Pk).EqualsTo("test"))
                .ToListAsync().ConfigureAwait(false);
        }
    }
}