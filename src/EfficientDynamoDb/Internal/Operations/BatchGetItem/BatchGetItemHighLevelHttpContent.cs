using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.BatchGetItem
{
    internal sealed class BatchGetItemHighLevelHttpContent : BatchItemHttpContent
    {
        private readonly BuilderNode _node;
        private readonly string? _tableNamePrefix;
        private readonly DynamoDbContextMetadata _metadata;

        public BatchGetItemHighLevelHttpContent(BuilderNode node, string? tableNamePrefix, DynamoDbContextMetadata metadata) : base("DynamoDB_20120810.BatchGetItem")
        {
            _node = node;
            _tableNamePrefix = tableNamePrefix;
            _metadata = metadata;
        }

        protected override async ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writeState = 0;
            
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

            writer.WritePropertyName("RequestItems");
            writer.WriteStartObject();

            DdbExpressionVisitor? visitor = null;
            foreach (var node in _node)
            {
                var tableNode = (BatchGetTableNode) node;

                WriteTableNameAsKey(writer, _tableNamePrefix, tableNode.ClassInfo.TableName!);
                writer.WriteStartObject();

                writer.WritePropertyName("Keys");
                writer.WriteStartArray();

                var hasProjections = false;
                var hasPrimitives = false;

                foreach (var itemNode in tableNode.Value)
                {
                    switch (itemNode.Type)
                    {
                        case BuilderNodeType.Primitive:
                            hasPrimitives = true;
                            break;
                        case BuilderNodeType.ProjectedAttributes:
                            hasProjections = true;
                            break;
                        case BuilderNodeType.PrimaryKey:
                            ((EntityPrimaryKeyNodeBase) itemNode).WriteValueWithoutKey(in ddbWriter);
                    
                            if (ddbWriter.ShouldFlush)
                                await ddbWriter.FlushAsync().ConfigureAwait(false);
                            break;
                    }
                }

                writer.WriteEndArray();

                if (hasPrimitives)
                {
                    foreach (var itemNode in tableNode.Value)
                    {
                        if (itemNode.Type != BuilderNodeType.Primitive)
                            continue;
                        
                        itemNode.WriteValue(in ddbWriter, ref writeState);
                    }
                }

                if (hasProjections)
                    writer.WriteProjectionExpression(ref visitor, tableNode.Value, _metadata);

                writer.WriteEndObject();
            }

            writer.WriteEndObject();
            
            writer.WriteEndObject();
        }
    }
}