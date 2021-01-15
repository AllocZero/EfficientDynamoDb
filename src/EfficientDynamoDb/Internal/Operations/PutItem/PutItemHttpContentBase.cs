using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.Operations.PutItem;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.PutItem
{
    internal abstract class PutItemHttpContentBase<TRequest> : DynamoDbHttpContent where TRequest: PutItemRequestBase
    {
        private readonly string? _tablePrefix;
        
        protected TRequest Request { get; }

        protected PutItemHttpContentBase(TRequest request, string? tablePrefix) : base("DynamoDB_20120810.PutItem")
        {
            Request = request;
            _tablePrefix = tablePrefix;
        }

        protected abstract ValueTask WriteItemAsync(DdbWriter writer);

        protected override async ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

            writer.WritePropertyName("Item");
            await WriteItemAsync(ddbWriter).ConfigureAwait(false);

            writer.WriteTableName(_tablePrefix, Request.TableName);
            
            if (Request.ConditionExpression != null)
                writer.WriteString("ConditionExpression", Request.ConditionExpression);
            
            if (Request.ExpressionAttributeNames?.Count > 0)
                writer.WriteExpressionAttributeNames(Request.ExpressionAttributeNames);
            
            if (Request.ExpressionAttributeValues?.Count > 0)
                writer.WriteExpressionAttributeValues(Request.ExpressionAttributeValues);

            if (Request.ReturnConsumedCapacity != ReturnConsumedCapacity.None)
                writer.WriteReturnConsumedCapacity(Request.ReturnConsumedCapacity);

            if (Request.ReturnItemCollectionMetrics != ReturnItemCollectionMetrics.None)
                writer.WriteReturnItemCollectionMetrics(Request.ReturnItemCollectionMetrics);
            
            if (Request.ReturnValues != ReturnValues.None)
                writer.WriteReturnValues(Request.ReturnValues);

            writer.WriteEndObject();
        }
    }
}