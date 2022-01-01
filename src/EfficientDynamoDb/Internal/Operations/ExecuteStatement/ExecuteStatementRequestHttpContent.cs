using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Internal.Operations.Shared;
using System.Threading.Tasks;

namespace EfficientDynamoDb.Internal.Operations.ExecuteStatement
{
    internal class ExecuteStatementRequestHttpContent : DynamoDbHttpContent
    {
        private readonly string statement;

        public ExecuteStatementRequestHttpContent(string statement) : base("DynamoDB_20120810.ExecuteStatement")
        {
            this.statement = statement;
        }

        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var json = ddbWriter.JsonWriter;
            json.WriteStartObject();
            json.WritePropertyName("Statement");
            json.WriteStringValue(statement);
            json.WriteEndObject();

            return default;
        }
    }
}