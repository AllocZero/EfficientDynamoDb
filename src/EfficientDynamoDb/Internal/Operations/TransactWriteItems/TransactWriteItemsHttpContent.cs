using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.TransactWriteItems;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.TransactWriteItems
{
    internal class TransactWriteItemsHttpContent : DynamoDbHttpContent
    {
        private readonly TransactWriteItemsRequest _request;
        private readonly string? _tablePrefix;

        public TransactWriteItemsHttpContent(TransactWriteItemsRequest request, string? tablePrefix) : base("DynamoDB_20120810.TransactWriteItems")
        {
            _request = request;
            _tablePrefix = tablePrefix;
        }

        protected override async ValueTask WriteDataAsync(Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter)
        {
            writer.WriteStartObject();
            
            if (_request.ReturnConsumedCapacity != ReturnConsumedCapacity.None)
                writer.WriteReturnConsumedCapacity(_request.ReturnConsumedCapacity);
            
            if(_request.ReturnItemCollectionMetrics != ReturnItemCollectionMetrics.None)
                writer.WriteReturnItemCollectionMetrics(_request.ReturnItemCollectionMetrics);

            if (_request.ClientRequestToken != null)
                writer.WriteString("ClientRequestToken", _request.ClientRequestToken);
            
            writer.WritePropertyName("TransactItems");
            
            writer.WriteStartArray();

            foreach (var transactItem in _request.TransactItems)
            {
                writer.WriteStartObject();

                if (transactItem.ConditionCheck != null)
                {
                    WriteConditionCheck(writer, transactItem.ConditionCheck);

                    if (bufferWriter.ShouldWrite(writer))
                        await bufferWriter.WriteToStreamAsync().ConfigureAwait(false);
                }

                if (transactItem.Delete != null)
                {
                    WriteDelete(writer, transactItem.Delete);
                    
                    if (bufferWriter.ShouldWrite(writer))
                        await bufferWriter.WriteToStreamAsync().ConfigureAwait(false);
                }

                if (transactItem.Put != null)
                {
                    writer.WritePropertyName("Put");
                    
                    writer.WriteStartObject();
                    
                    writer.WriteString("TableName", _tablePrefix + transactItem.Put.TableName);
            
                    if (transactItem.Put.ConditionExpression != null)
                        writer.WriteString("ConditionExpression", transactItem.Put.ConditionExpression);
            
                    if (transactItem.Put.ExpressionAttributeNames != null)
                        writer.WriteExpressionAttributeNames(transactItem.Put.ExpressionAttributeNames);

                    if (transactItem.Put.ExpressionAttributeValues != null)
                        writer.WriteExpressionAttributeValues(transactItem.Put.ExpressionAttributeValues);

                    if(transactItem.Put.ReturnValuesOnConditionCheckFailure != ReturnValuesOnConditionCheckFailure.None)
                        writer.WriteReturnValuesOnConditionCheckFailure(transactItem.Put.ReturnValuesOnConditionCheckFailure);
                    
                    if (bufferWriter.ShouldWrite(writer))
                        await bufferWriter.WriteToStreamAsync().ConfigureAwait(false);

                    writer.WritePropertyName("Item");
                    await writer.WriteAttributesDictionaryAsync(bufferWriter, transactItem.Put.Item!).ConfigureAwait(false);
                    
                    writer.WriteEndObject();
                }
                
                if (transactItem.Update != null)
                {
                    WriteUpdate(writer, transactItem.Update);
                    
                    if (bufferWriter.ShouldWrite(writer))
                        await bufferWriter.WriteToStreamAsync().ConfigureAwait(false);
                }

                writer.WriteEndObject();
            }
            
            writer.WriteEndArray();
            
            writer.WriteEndObject();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteConditionCheck(Utf8JsonWriter writer, ConditionCheck conditionCheck)
        {
            writer.WritePropertyName("ConditionCheck");
            
            writer.WriteStartObject();

            if (conditionCheck.ConditionExpression != null)
                writer.WriteString("ConditionExpression", conditionCheck.ConditionExpression);

            if (conditionCheck.ExpressionAttributeNames != null)
                writer.WriteExpressionAttributeNames(conditionCheck.ExpressionAttributeNames);

            if (conditionCheck.ExpressionAttributeValues != null)
                writer.WriteExpressionAttributeValues(conditionCheck.ExpressionAttributeValues);

            if (conditionCheck.Key != null)
                writer.WritePrimaryKey(conditionCheck.Key);

            if (conditionCheck.ReturnValuesOnConditionCheckFailure != ReturnValuesOnConditionCheckFailure.None)
                writer.WriteReturnValuesOnConditionCheckFailure(conditionCheck.ReturnValuesOnConditionCheckFailure);
            
            writer.WriteEndObject();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteDelete(Utf8JsonWriter writer, TransactDeleteItem deleteItem)
        {
            writer.WritePropertyName("Delete");
            
            writer.WriteStartObject();
            
            writer.WriteString("TableName", _tablePrefix + deleteItem.TableName);
            
            if (deleteItem.ConditionExpression != null)
                writer.WriteString("ConditionExpression", deleteItem.ConditionExpression);
            
            if (deleteItem.ExpressionAttributeNames != null)
                writer.WriteExpressionAttributeNames(deleteItem.ExpressionAttributeNames);

            if (deleteItem.ExpressionAttributeValues != null)
                writer.WriteExpressionAttributeValues(deleteItem.ExpressionAttributeValues);
            
            if(deleteItem.Key != null)
                writer.WritePrimaryKey(deleteItem.Key);
            
            if(deleteItem.ReturnValuesOnConditionCheckFailure != ReturnValuesOnConditionCheckFailure.None)
                writer.WriteReturnValuesOnConditionCheckFailure(deleteItem.ReturnValuesOnConditionCheckFailure);
            
            writer.WriteEndObject();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteUpdate(Utf8JsonWriter writer, TransactUpdateItem updateItem)
        {
            writer.WritePropertyName("Update");
                    
            writer.WriteStartObject();
                    
            writer.WriteString("TableName",_tablePrefix + updateItem.TableName);
            
            if (updateItem.ConditionExpression != null)
                writer.WriteString("ConditionExpression", updateItem.ConditionExpression);
            
            if (updateItem.ExpressionAttributeNames != null)
                writer.WriteExpressionAttributeNames(updateItem.ExpressionAttributeNames);

            if (updateItem.ExpressionAttributeValues != null)
                writer.WriteExpressionAttributeValues(updateItem.ExpressionAttributeValues);
            
            if(updateItem.Key != null)
                writer.WritePrimaryKey(updateItem.Key);
            
            if(updateItem.UpdateExpression != null)
                writer.WriteString("UpdateExpression", updateItem.UpdateExpression);

            if(updateItem.ReturnValuesOnConditionCheckFailure != ReturnValuesOnConditionCheckFailure.None)
                writer.WriteReturnValuesOnConditionCheckFailure(updateItem.ReturnValuesOnConditionCheckFailure);
            
            writer.WriteEndObject();
        }
    }
}