using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Benchmarks.Mocks.Http;
using EfficientDynamoDb;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Configs.Http;

namespace Benchmarks.Benchmarks.Query
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