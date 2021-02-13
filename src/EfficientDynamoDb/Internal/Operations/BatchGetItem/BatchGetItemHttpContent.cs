using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.Operations.BatchGetItem;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.BatchGetItem
{
    internal sealed class BatchGetItemHttpContent : BatchItemHttpContent
    {
        private readonly BatchGetItemRequest _request;
        private readonly string? _tableNamePrefix;

        public BatchGetItemHttpContent(BatchGetItemRequest request, string? tableNamePrefix) : base("DynamoDB_20120810.BatchGetItem")
        {
            _request = request;
            _tableNamePrefix = tableNamePrefix;
        }

        protected override async ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

            writer.WritePropertyName("RequestItems");
            writer.WriteStartObject();

            foreach (var item in _request.RequestItems!)
            {
                WriteTableNameAsKey(writer, _tableNamePrefix, item.Key);
                writer.WriteStartObject();

                writer.WritePropertyName("Keys");
                writer.WriteStartArray();

                foreach (var primaryKey in item.Value.Keys!)
                {
                    writer.WriteAttributesDictionary(primaryKey);
                    if (ddbWriter.ShouldFlush)
                        await ddbWriter.FlushAsync().ConfigureAwait(false);
                }

                writer.WriteEndArray();

                if (item.Value.ProjectionExpression != null)
                    writer.WriteString("ProjectionExpression", item.Value.ProjectionExpression);
                if (item.Value.ExpressionAttributeNames?.Count > 0)
                    writer.WriteExpressionAttributeNames(item.Value.ExpressionAttributeNames);
                if (item.Value.ConsistentRead)
                    writer.WriteBoolean("ConsistentRead", item.Value.ConsistentRead);

                writer.WriteEndObject();
            }
            
            writer.WriteEndObject();

            if (_request.ReturnConsumedCapacity != ReturnConsumedCapacity.None)
                writer.WriteReturnConsumedCapacity(_request.ReturnConsumedCapacity);
            
            writer.WriteEndObject();
        }
    }
}