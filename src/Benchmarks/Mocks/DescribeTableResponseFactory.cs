using System.Text;
using System.Text.Json;
using Benchmarks.Constants;
using EfficientDynamoDb.Internal.JsonConverters;
using EfficientDynamoDb.Operations.DescribeTable;
using EfficientDynamoDb.Operations.DescribeTable.Models;
using EfficientDynamoDb.Operations.DescribeTable.Models.Enums;

namespace Benchmarks.Mocks
{
    public static class DescribeTableResponseFactory
    {
        public static byte[] CreateResponse()
        {
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new DescribeTableResponse(new TableDescription
            {
                TableName = "production_" + Tables.TestTable,
                KeySchema = new[] {new KeySchemaElement("pk", EfficientDynamoDb.Operations.DescribeTable.Models.Enums.KeyType.Hash), new KeySchemaElement("sk", KeyType.Range)},
                AttributeDefinitions = new[] {new AttributeDefinition("pk", "S"), new AttributeDefinition("sk", "S")}
            }), new JsonSerializerOptions
            {
                Converters = { new DdbEnumJsonConverterFactory()}
            }));
        }
    }
}