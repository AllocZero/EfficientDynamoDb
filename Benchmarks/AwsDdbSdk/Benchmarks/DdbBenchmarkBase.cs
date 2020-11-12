using System;
using System.Collections.Generic;
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

        protected abstract Task<IReadOnlyCollection<object>> QueryAsync<T>(string pk) where T: KeysOnlyEntity, new();

        protected DdbBenchmarkBase() => (DbContext, DbClient) = GetContext();

        protected async Task<int> RunBenchmarkAsync<T>(string pk) where T: KeysOnlyEntity, new()
        {
            var result = await QueryAsync<T>(pk).ConfigureAwait(false);

            return result.Count;
        }

        protected async Task<IReadOnlyCollection<object>> SetupBenchmarkAsync<T>(string pk, int desiredEntitiesCount = 1000) where T: KeysOnlyEntity, new()
        {
            HttpHandlerConfig.IsCacheEnabled = false;
            HttpHandlerConfig.IsCacheEnabled = true;
            var entities = await QueryAsync<T>(pk).ConfigureAwait(false);
            if (entities.Count >= desiredEntitiesCount)
                return entities;

            HttpHandlerConfig.IsCacheEnabled = false;
            await Task.WhenAll(Enumerable.Range(0, desiredEntitiesCount).Select(i => DbContext.SaveAsync(new T {Pk = pk, Sk = $"sk_{i:0000}"})))
                .ConfigureAwait(false);

            HttpHandlerConfig.IsCacheEnabled = true;
            return await QueryAsync<T>(pk).ConfigureAwait(false);
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