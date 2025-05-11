using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.Shared;
using EfficientDynamoDb.Operations.UpdateItem;

namespace EfficientDynamoDb.Internal.Operations.UpdateItem
{
    internal class UpdateItemHttpContent : DynamoDbHttpContent
    {
        private readonly string _pkName;
        private readonly string _skName;
        private readonly UpdateItemRequest _request;
        private readonly ITableNameFormatter? _tableNameFormatter;

        public UpdateItemHttpContent(UpdateItemRequest request, ITableNameFormatter? tableNameFormatter, string pkName, string skName) : base("DynamoDB_20120810.UpdateItem")
        {
            _request = request;
            _tableNameFormatter = tableNameFormatter;
            _pkName = pkName;
            _skName = skName;
        }

        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

            WritePrimaryKey(writer);

            writer.WriteTableName(_tableNameFormatter, _request.TableName);
            
            if (_request.ConditionExpression != null)
                writer.WriteString("ConditionExpression", _request.ConditionExpression);
            
            if (_request.ExpressionAttributeNames?.Count > 0)
                writer.WriteExpressionAttributeNames(_request.ExpressionAttributeNames);
            
            if (_request.ExpressionAttributeValues?.Count > 0)
                writer.WriteExpressionAttributeValues(_request.ExpressionAttributeValues);

            if (_request.ReturnConsumedCapacity != ReturnConsumedCapacity.None)
                writer.WriteReturnConsumedCapacity(_request.ReturnConsumedCapacity);

            if (_request.ReturnItemCollectionMetrics != ReturnItemCollectionMetrics.None)
                WriteReturnItemCollectionMetrics(writer);
            
            if (_request.ReturnValues != ReturnValues.None)
                WriteReturnValues(writer);

            if (_request.UpdateExpression != null)
                writer.WriteString("UpdateExpression", _request.UpdateExpression);

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
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteReturnValues(Utf8JsonWriter writer)
        {
            writer.WriteString("ReturnValues", _request.ReturnValues switch
            {
                ReturnValues.None => "NONE",
                ReturnValues.AllOld => "ALL_OLD",
                ReturnValues.UpdatedOld => "UPDATED_OLD",
                ReturnValues.AllNew => "ALL_NEW",
                ReturnValues.UpdatedNew => "UPDATED_NEW",
                _ => "NONE"
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteReturnItemCollectionMetrics(Utf8JsonWriter writer)
        {
            writer.WriteString("ReturnItemCollectionMetrics", _request.ReturnItemCollectionMetrics switch
            {
                ReturnItemCollectionMetrics.None => "NONE",
                ReturnItemCollectionMetrics.Size => "SIZE",
                _ => "NONE"
            });
        }
    }
}