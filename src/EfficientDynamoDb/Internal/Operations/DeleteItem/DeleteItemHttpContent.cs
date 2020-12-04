using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.DeleteItem;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.DeleteItem
{
    internal class DeleteItemHttpContent : DynamoDbHttpContent
    {
        private readonly DeleteItemRequest _request;
        private readonly string _pkName;
        private readonly string? _skName;
        private readonly string? _tablePrefix;

        public DeleteItemHttpContent(DeleteItemRequest request, string pkName, string? skName, string? tablePrefix) : base("DynamoDB_20120810.DeleteItem")
        {
            _request = request;
            _pkName = pkName;
            _skName = skName;
            _tablePrefix = tablePrefix;
        }

        protected override ValueTask WriteDataAsync(Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter)
        {
            writer.WriteStartObject();
            
            WritePrimaryKey(writer);
            
            writer.WriteString("TableName", _tablePrefix + _request.TableName);

            if (_request.ExpressionAttributeNames?.Count > 0)
                writer.WriteExpressionAttributeNames(_request.ExpressionAttributeNames);
            
            if(_request.ExpressionAttributeValues != null)
                writer.WriteExpressionAttributeValues(_request.ExpressionAttributeValues);
            
            if (_request.ConditionExpression != null)
                writer.WriteString("ConditionExpression", _request.ConditionExpression);

            if (_request.ReturnConsumedCapacity != ReturnConsumedCapacity.None)
                writer.WriteReturnConsumedCapacity(_request.ReturnConsumedCapacity);

            if(_request.ReturnConsumedCapacity != ReturnConsumedCapacity.None)
                writer.WriteReturnConsumedCapacity(_request.ReturnConsumedCapacity);
            
            if (_request.ReturnItemCollectionMetrics != ReturnItemCollectionMetrics.None)
                writer.WriteReturnItemCollectionMetrics(_request.ReturnItemCollectionMetrics);
            
            if (_request.ReturnValues != ReturnValues.None)
                writer.WriteReturnValues(_request.ReturnValues);
            
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
                writer.WritePropertyName(_skName!);
                _request.Key.SortKeyValue.Value.Write(writer);
            }
            
            writer.WriteEndObject();
        }
    }
}