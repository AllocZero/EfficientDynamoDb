using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.ExecuteTransaction;
using EfficientDynamoDb.Operations.Shared;
using System.Threading.Tasks;

namespace EfficientDynamoDb.Internal.Operations.ExecuteTransaction
{
    internal class ExecuteTransactionRequestHttpContent : DynamoDbHttpContent
    {
        private readonly ExecuteTransactionRequest _request;

        public ExecuteTransactionRequestHttpContent(ExecuteTransactionRequest request) : base("DynamoDB_20120810.ExecuteTransaction")
        {
            _request = request;
        }

        protected override ValueTask WriteDataAsync(DdbWriter writer)
        {
            var json = writer.JsonWriter;
            json.WriteStartObject();

            json.WritePropertyName("TransactStatements");
            json.WriteStartArray();
            foreach (var statement in _request.TransactStatements)
            {
                json.WriteStartObject();

                json.WriteString("Statement", statement.Statement);

                json.WritePropertyName("Parameters");
                json.WriteStartArray();
                foreach (var parameter in statement.Parameters)
                {
                    parameter.Write(json);
                }
                json.WriteEndArray();

                json.WriteEndObject();
            }
            json.WriteEndArray();

            if (_request.ClientRequestToken != null)
                json.WriteString("ClientRequestToken", _request.ClientRequestToken);

            if (_request.ReturnConsumedCapacity != ReturnConsumedCapacity.None)
                json.WriteReturnConsumedCapacity(_request.ReturnConsumedCapacity);

            json.WriteEndObject();

            return default;
        }
    }
}
