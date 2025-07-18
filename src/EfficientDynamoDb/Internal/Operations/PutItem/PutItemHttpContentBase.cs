using System.Threading.Tasks;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.PutItem;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.PutItem
{
    internal abstract class PutItemHttpContentBase<TRequest> : DynamoDbHttpContent where TRequest: PutItemRequestBase
    {
        private readonly ITableNameFormatter? _tableNameFormatter;
        
        protected TRequest Request { get; }

        protected PutItemHttpContentBase(TRequest request, ITableNameFormatter? tableNameFormatter) : base("DynamoDB_20120810.PutItem")
        {
            Request = request;
            _tableNameFormatter = tableNameFormatter;
        }

        protected abstract ValueTask WriteItemAsync(DdbWriter writer);

        protected override async ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

            writer.WritePropertyName("Item");
            await WriteItemAsync(ddbWriter).ConfigureAwait(false);

            writer.WriteTableName(_tableNameFormatter, Request.TableName);
            
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