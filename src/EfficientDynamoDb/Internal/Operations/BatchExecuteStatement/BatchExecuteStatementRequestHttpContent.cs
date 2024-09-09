using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.BatchExecuteStatement;
using EfficientDynamoDb.Operations.Shared;
using System.Text;
using System;
using System.Threading.Tasks;

namespace EfficientDynamoDb.Internal.Operations.BatchExecuteStatement
{
    internal class BatchExecuteStatementRequestHttpContent : DynamoDbHttpContent
    {
        private readonly BatchExecuteStatementRequest _request;

        public BatchExecuteStatementRequestHttpContent(BatchExecuteStatementRequest request) : base("DynamoDB_20120810.BatchExecuteStatement")
        {
            _request = request;
        }

        protected override ValueTask WriteDataAsync(DdbWriter writer)
        {
            var json = writer.JsonWriter;
            json.WriteStartObject();

            json.WritePropertyName("Statements");
            json.WriteStartArray();
            foreach (var statementRequest in _request.Statements)
            {
                json.WriteStartObject();

                json.WritePropertyName("Statement");
                json.WriteStringValue(statementRequest.Statement);

                json.WritePropertyName("Parameters");
                json.WriteStartArray();
                foreach (var parameter in statementRequest.Parameters)
                {
                    parameter.Write(json);
                }
                json.WriteEndArray();

                json.WritePropertyName("ConsistentRead");
                json.WriteBooleanValue(statementRequest.ConsistentRead);

                if (statementRequest.ReturnValuesOnConditionCheckFailure != ReturnValuesOnConditionCheckFailure.None)
                    json.WriteReturnValuesOnConditionCheckFailure(statementRequest.ReturnValuesOnConditionCheckFailure);

                json.WriteEndObject();
            }
            json.WriteEndArray();

            if (_request.ReturnConsumedCapacity != ReturnConsumedCapacity.None)
                json.WriteReturnConsumedCapacity(_request.ReturnConsumedCapacity);

            json.WriteEndObject();

            return default;
        }
    }
}
