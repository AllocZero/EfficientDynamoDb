using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.PutItem;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.PutItem
{
    internal class PutItemHttpContent : DynamoDbHttpContent
    {
        private readonly PutItemRequest _request;
        private readonly string? _tablePrefix;

        public PutItemHttpContent(PutItemRequest request, string? tablePrefix) : base("DynamoDB_20120810.PutItem")
        {
            _request = request;
            _tablePrefix = tablePrefix;
        }

        protected override async ValueTask WriteDataAsync(Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("Item");
            await writer.WriteAttributesDictionaryAsync(bufferWriter, _request.Item!).ConfigureAwait(false);

            writer.WriteTableName(_tablePrefix, _request.TableName);
            
            if (_request.ConditionExpression != null)
                writer.WriteString("ConditionExpression", _request.ConditionExpression);
            
            if (_request.ExpressionAttributeNames?.Count > 0)
                writer.WriteExpressionAttributeNames(_request.ExpressionAttributeNames);
            
            if (_request.ExpressionAttributeValues?.Count > 0)
                writer.WriteExpressionAttributeValues(_request.ExpressionAttributeValues);

            if (_request.ReturnConsumedCapacity != ReturnConsumedCapacity.None)
                writer.WriteReturnConsumedCapacity(_request.ReturnConsumedCapacity);

            if (_request.ReturnItemCollectionMetrics != ReturnItemCollectionMetrics.None)
                writer.WriteReturnItemCollectionMetrics(_request.ReturnItemCollectionMetrics);
            
            if (_request.ReturnValues != ReturnValues.None)
                writer.WriteReturnValues(_request.ReturnValues);

            writer.WriteEndObject();
        }
    }
}