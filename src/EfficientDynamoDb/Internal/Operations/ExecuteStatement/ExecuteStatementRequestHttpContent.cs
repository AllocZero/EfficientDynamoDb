using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.ExecuteStatement;
using EfficientDynamoDb.Operations.Shared;
using System.Threading.Tasks;

namespace EfficientDynamoDb.Internal.Operations.ExecuteStatement
{
    internal class ExecuteStatementRequestHttpContent : DynamoDbHttpContent
    {
        private readonly ExecuteStatementRequest _request;

        public ExecuteStatementRequestHttpContent(ExecuteStatementRequest executeStatementRequest) : base("DynamoDB_20120810.ExecuteStatement")
        {
            _request = executeStatementRequest;
        }

        protected override ValueTask WriteDataAsync(DdbWriter writer)
        {
            var json = writer.JsonWriter;
            json.WriteStartObject();

            json.WritePropertyName("Statement");
            json.WriteStringValue(_request.Statement);

            json.WritePropertyName("Parameters");
            json.WriteStartArray();
            foreach (var parameter in _request.Parameters)
            {
                parameter.Write(json);
            }
            json.WriteEndArray();

            if (_request.ConsistentRead)
                json.WriteBoolean("ConsistentRead", true);

            if (_request.Limit.HasValue)
                json.WriteNumber("Limit", _request.Limit.Value);

            if (_request.NextToken != null)
                json.WriteString("NextToken", _request.NextToken);

            if (_request.ReturnConsumedCapacity != ReturnConsumedCapacity.None)
                json.WriteReturnConsumedCapacity(_request.ReturnConsumedCapacity);

            if (_request.ReturnItemCollectionMetrics != ReturnItemCollectionMetrics.None)
                json.WriteReturnItemCollectionMetrics(_request.ReturnItemCollectionMetrics);

            if (_request.ReturnValuesOnConditionCheckFailure != ReturnValuesOnConditionCheckFailure.None)
                json.WriteReturnValuesOnConditionCheckFailure(_request.ReturnValuesOnConditionCheckFailure);

            json.WriteEndObject();
            return default;
        }
    }
}
