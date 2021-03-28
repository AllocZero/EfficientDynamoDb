using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace Benchmarks.Benchmarks.Query
{
    public class LowLevelQueryBenchmark : QueryBenchmarkBase
    {
        protected override async Task<IReadOnlyCollection<object>> QueryAsync<T>(string pk)
        {
            var result = await DbClient.QueryAsync(new QueryRequest
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

            return result.Items;
        }
    }
}