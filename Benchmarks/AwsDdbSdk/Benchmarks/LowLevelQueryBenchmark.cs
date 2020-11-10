using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace Benchmarks.AwsDdbSdk.Benchmarks
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class LowLevelQueryBenchmark : QueryBenchmarkBase
    {
        protected override async Task<int> QueryAsync<T>(string pk)
        {
            var entities = await DbClient.QueryAsync(new QueryRequest("production_coins_system_v2")
            {
                Select = Select.ALL_ATTRIBUTES,
                KeyConditions = new Dictionary<string, Condition>
                {
                    {
                        "pk",
                        new Condition
                        {
                            ComparisonOperator = "EQ",
                            AttributeValueList = new List<AttributeValue> {new AttributeValue {S = pk}}
                        }
                    }
                }
            });
            
            return entities.Count;
        }
    }
}