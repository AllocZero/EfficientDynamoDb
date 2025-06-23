using System.Threading.Tasks;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.BatchWriteItem;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.BatchWriteItem
{
    internal class BatchWriteItemHttpContent : BatchItemHttpContent
    {
        private readonly BatchWriteItemRequest _request;
        private readonly ITableNameFormatter? _tableNameFormatter;

        public BatchWriteItemHttpContent(BatchWriteItemRequest request, ITableNameFormatter? tableNameFormatter) : base("DynamoDB_20120810.BatchWriteItem")
        {
            _request = request;
            _tableNameFormatter = tableNameFormatter;
        }

        protected override async ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

            writer.WritePropertyName("RequestItems");
            writer.WriteStartObject();
            
            foreach (var item in _request.RequestItems!)
            {
                WriteTableNameAsKey(writer, _tableNameFormatter, item.Key);
                writer.WriteStartArray();

                for (var i = 0; i < item.Value.Count; i++)
                {
                    var operation = item.Value[i];
                    if (operation.DeleteRequest != null)
                    {
                        writer.WriteStartObject();
                        writer.WritePropertyName("DeleteRequest");
                        writer.WriteStartObject();
            
                        writer.WritePropertyName("Key");
                        writer.WriteAttributesDictionary(operation.DeleteRequest.Key);
            
                        writer.WriteEndObject();
                        writer.WriteEndObject();
                    }
                    else if (operation.PutRequest != null)
                    {
                        writer.WriteStartObject();
                        writer.WritePropertyName("PutRequest");
                        writer.WriteStartObject();
            
                        writer.WritePropertyName("Item");
                        await writer.WriteAttributesDictionaryAsync(ddbWriter.BufferWriter, operation.PutRequest.Item).ConfigureAwait(false);
            
                        writer.WriteEndObject();
                        writer.WriteEndObject();
                    }
                }

                writer.WriteEndArray();
            }

            writer.WriteEndObject();

            if (_request.ReturnConsumedCapacity != ReturnConsumedCapacity.None)
                writer.WriteReturnConsumedCapacity(_request.ReturnConsumedCapacity);

            if (_request.ReturnItemCollectionMetrics != ReturnItemCollectionMetrics.None)
                writer.WriteReturnItemCollectionMetrics(_request.ReturnItemCollectionMetrics);
            
            writer.WriteEndObject();
        }
    }
}