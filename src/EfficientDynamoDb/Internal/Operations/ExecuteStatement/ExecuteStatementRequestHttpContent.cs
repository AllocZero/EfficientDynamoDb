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

            json.WritePropertyName("ConsistentRead");
            json.WriteBooleanValue(_request.ConsistentRead);

            if (_request.Limit > 0)
            {
                json.WritePropertyName("Limit");
                json.WriteNumberValue(_request.Limit);
            }

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
