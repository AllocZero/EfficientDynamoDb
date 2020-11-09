using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using AWSSDK.Core.NetStandard.Amazon.Runtime.Pipeline.HttpHandler;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Benchmarks.AwsDdbSdk.Entities;

namespace Benchmarks.AwsDdbSdk.Benchmarks
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class LowLevelQueryBenchmark
    {
        private DynamoDBContext _dbContext;
        private AmazonDynamoDBClient _dbClient;

        private const string KeysOnlyEntityPk = "keys_only_bench";
        private const string MediumEntityPk = "medium_bench";
        
        [GlobalSetup]
        public async Task SetupAsync()
        {
            (_dbContext, _dbClient) = GetContext();

            // await SetupKeysOnlyBenchAsync().ConfigureAwait(false);
            await SetupMediumBenchAsync().ConfigureAwait(false);
        }

        [Benchmark]
        public async Task<int> MediumBenchmarkAsync()
        {
            var entities = await _dbClient.QueryAsync(new QueryRequest("production_coins_system_v2")
            {
                Select = Select.ALL_ATTRIBUTES,
                KeyConditions = new Dictionary<string, Condition>
                {
                    {
                        "pk",
                        new Condition
                        {
                            ComparisonOperator = "EQ",
                            AttributeValueList = new List<AttributeValue> {new AttributeValue {S = MediumEntityPk}}
                        }
                    }
                }
            });
            
            return entities.Count;
        }

        private async Task SetupKeysOnlyBenchAsync()
        {
            const int desiredEntitiesCount = 1000;
            HttpHandlerConfig.IsCacheEnabled = true;
            var entities = await _dbContext.QueryAsync<KeysOnlyEntity>(KeysOnlyEntityPk).GetRemainingAsync().ConfigureAwait(false);
            if (entities.Count >= desiredEntitiesCount)
                return;

            HttpHandlerConfig.IsCacheEnabled = false;
            await Task.WhenAll(Enumerable.Range(0, desiredEntitiesCount)
                    .Select(i => _dbContext.SaveAsync(new KeysOnlyEntity {Pk = KeysOnlyEntityPk, Sk = $"sk_{i:0000}"})))
                .ConfigureAwait(false);

            HttpHandlerConfig.IsCacheEnabled = true;
            await _dbContext.QueryAsync<KeysOnlyEntity>(KeysOnlyEntityPk).GetRemainingAsync().ConfigureAwait(false);
        }
        
        private async Task SetupMediumBenchAsync()
        {
            const int desiredEntitiesCount = 1000;
            HttpHandlerConfig.IsCacheEnabled = true;
            var entities = await _dbContext.QueryAsync<MediumStringFieldsEntity>(MediumEntityPk).GetRemainingAsync().ConfigureAwait(false);
            if (entities.Count >= desiredEntitiesCount)
                return;

            HttpHandlerConfig.IsCacheEnabled = false;
            await Task.WhenAll(Enumerable.Range(0, desiredEntitiesCount)
                    .Select(i => _dbContext.SaveAsync(new MediumStringFieldsEntity {Pk = MediumEntityPk, Sk = $"sk_{i:0000}"})))
                .ConfigureAwait(false);

            HttpHandlerConfig.IsCacheEnabled = true;
            await _dbContext.QueryAsync<MediumStringFieldsEntity>(MediumEntityPk).GetRemainingAsync().ConfigureAwait(false);
        }

        private (DynamoDBContext dbContext, AmazonDynamoDBClient dbClient) GetContext()
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