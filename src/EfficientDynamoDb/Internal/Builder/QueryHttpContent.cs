using System.Text.Json;
using EfficientDynamoDb.Context.Requests.Query;

namespace EfficientDynamoDb.Internal.Builder
{
    public class QueryHttpContent : DynamoDbHttpContent
    {
        private readonly string _tableName;
        private readonly QueryRequest _request;

        public QueryHttpContent(string tableName, QueryRequest request) : base("DynamoDB_20120810.Query")
        {
            _tableName = tableName;
            _request = request;
        }

        protected override void WriteData(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            
            writer.WriteString("TableName", _tableName);
            
            writer.WriteString("KeyConditionExpression", _request.KeyExpression!.ConditionExpression);

            if (_request.KeyExpression.AttributeNames?.Count > 0)
            {
                writer.WritePropertyName("ExpressionAttributeNames");
                
                writer.WriteStartObject();

                foreach (var pair in _request.KeyExpression.AttributeNames)
                    writer.WriteString(pair.Key, pair.Value);
                
                writer.WriteEndObject();
            }

            if (_request.KeyExpression.AttributeValues?.Count > 0)
            {
                writer.WritePropertyName("ExpressionAttributeValues");
                
                writer.WriteStartObject();

                foreach (var pair in _request.KeyExpression.AttributeValues)
                {
                    writer.WritePropertyName(pair.Key);

                    pair.Value.Write(writer);
                }
                
                writer.WriteEndObject();
            }
            
            writer.WriteEndObject();
        }
    }
}