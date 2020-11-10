using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using AWSSDK.Core.NetStandard.Amazon.Runtime.Pipeline.HttpHandler;
using Benchmarks.AwsDdbSdk.Entities;

namespace Benchmarks.AwsDdbSdk.Benchmarks
{
    public abstract class DdbBenchmarkBase
    {
        protected DynamoDBContext DbContext { get; }
        protected AmazonDynamoDBClient DbClient { get; }

        protected abstract Task<int> QueryAsync<T>(string pk) where T: KeysOnlyEntity, new();

        protected DdbBenchmarkBase() => (DbContext, DbClient) = GetContext();

        protected async Task SetupBenchmarkAsync<T>(string pk, int desiredEntitiesCount = 1000) where T: KeysOnlyEntity, new()
        {
            HttpHandlerConfig.IsCacheEnabled = false;
            HttpHandlerConfig.IsCacheEnabled = true;
            var entitiesCount = await QueryAsync<T>(pk).ConfigureAwait(false);
            if (entitiesCount >= desiredEntitiesCount)
                return;

            HttpHandlerConfig.IsCacheEnabled = false;
            await Task.WhenAll(Enumerable.Range(0, desiredEntitiesCount).Select(i => DbContext.SaveAsync(new T {Pk = pk, Sk = $"sk_{i:0000}"})))
                .ConfigureAwait(false);

            HttpHandlerConfig.IsCacheEnabled = true;
            await QueryAsync<T>(pk).ConfigureAwait(false);
        }
        
        private static (DynamoDBContext dbContext, AmazonDynamoDBClient dbClient) GetContext()
        {
            var ddbConfig = new AmazonDynamoDBConfig {RegionEndpoint = RegionEndpoint.USEast1};
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