using System.Collections.Generic;
using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.Operations.PutItem;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.PutItem
{
    internal sealed class PutItemHighLevelHttpContent : DynamoDbHttpContent
    {
        private readonly PutItemHighLevelRequest _request;
        private readonly string? _tablePrefix;
        private readonly DynamoDbContextMetadata _metadata;

        public PutItemHighLevelHttpContent(PutItemHighLevelRequest request, string? tablePrefix, DynamoDbContextMetadata metadata)
            : base("DynamoDB_20120810.PutItem")
        {
            _request = request;
            _tablePrefix = tablePrefix;
            _metadata = metadata;
        }

        protected override async ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

            var entityClassInfo = _metadata.GetOrAddClassInfo(_request.ItemType!);
            
            writer.WritePropertyName("Item");
            await ddbWriter.WriteEntityAsync(entityClassInfo, _request.Item!).ConfigureAwait(false);

            writer.WriteTableName(_tablePrefix, entityClassInfo.TableName!);

            if (_request.ReturnConsumedCapacity != ReturnConsumedCapacity.None)
                writer.WriteReturnConsumedCapacity(_request.ReturnConsumedCapacity);

            if (_request.ReturnItemCollectionMetrics != ReturnItemCollectionMetrics.None)
                writer.WriteReturnItemCollectionMetrics(_request.ReturnItemCollectionMetrics);

            if (_request.ReturnValues != ReturnValues.None)
                writer.WriteReturnValues(_request.ReturnValues);

            if (_request.UpdateCondition != null)
                WriteCondition(in ddbWriter, _request.UpdateCondition);

            writer.WriteEndObject();
        }

        private void WriteCondition(in DdbWriter writer, FilterBase condition)
        {
            var cachedExpressionNames = new HashSet<string>();
            var expressionValuesCount = 0;

            var builder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);
            try
            {
                condition.WriteExpressionStatement(ref builder, cachedExpressionNames, ref expressionValuesCount);
                writer.JsonWriter.WriteString("ConditionExpression", builder.GetBuffer());
            }
            finally
            {
                builder.Dispose();
            }

            if (cachedExpressionNames.Count > 0)
                writer.JsonWriter.WriteExpressionAttributeNames(cachedExpressionNames);

            if (expressionValuesCount > 0)
                writer.WriteExpressionAttributeValues(_metadata, condition);
        }
    }
}