using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.GetItem;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.GetItem
{
    public class GetItemHttpContent : DynamoDbHttpContent
    {
        private readonly GetItemRequest _request;
        private readonly string _tableName;
        private readonly string _pkName;
        private readonly string _skName;

        public GetItemHttpContent(GetItemRequest request, string tableName, string pkName, string skName) : base("DynamoDB_20120810.GetItem")
        {
            _request = request;
            _tableName = tableName;
            _pkName = pkName;
            _skName = skName;
        }

        protected override ValueTask WriteDataAsync(Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter)
        {
            writer.WriteStartObject();
            
            WritePrimaryKey(writer);
            
            writer.WriteString("TableName", _tableName);

            if (_request.ConsistentRead)
                writer.WriteBoolean("ConsistentRead", true);
            
            if (_request.ExpressionAttributeNames?.Count > 0)
                writer.WriteExpressionAttributeNames(_request.ExpressionAttributeNames);
            
            if (_request.ProjectionExpression?.Count > 0)
                writer.WriteString("ProjectionExpression", string.Join(",", _request.ProjectionExpression));
            
            if (_request.ReturnConsumedCapacity != ReturnConsumedCapacity.None)
                writer.WriteReturnConsumedCapacity(_request.ReturnConsumedCapacity);

            writer.WriteEndObject();

            return default;  
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WritePrimaryKey(Utf8JsonWriter writer)
        {
            writer.WritePropertyName("Key");
            writer.WriteStartObject();
            
            writer.WritePropertyName(_pkName);
            _request.Key!.PartitionKeyValue.Write(writer);

            if (_request.Key.SortKeyValue != null)
            {
                writer.WritePropertyName(_skName);
                _request.Key.SortKeyValue.Value.Write(writer);
            }
            
            writer.WriteEndObject();
        }
    }
}