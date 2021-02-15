using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.UpdateItem
{
    internal sealed class UpdateItemHighLevelHttpContent : DynamoDbHttpContent
    {
        private readonly BuilderNode? _node;
        private readonly DdbClassInfo _classInfo;
        private readonly string? _tablePrefix;
        private readonly DynamoDbContextMetadata _metadata;

        public UpdateItemHighLevelHttpContent(string? tablePrefix, DynamoDbContextMetadata metadata, DdbClassInfo classInfo, BuilderNode? node)
            : base("DynamoDB_20120810.UpdateItem")
        {
            _node = node;
            _classInfo = classInfo;
            _tablePrefix = tablePrefix;
            _metadata = metadata;
        }

        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            ddbWriter.JsonWriter.WriteStartObject();
            
            ddbWriter.JsonWriter.WriteTableName(_tablePrefix, _classInfo.TableName!);

            WriteUpdateItem(in ddbWriter);

            ddbWriter.JsonWriter.WriteEndObject();

            return new ValueTask();
        }

        private void WriteUpdateItem(in DdbWriter ddbWriter)
        {
            var visitor = new DdbExpressionVisitor(_metadata);
            var builder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);
            try
            {
                ddbWriter.WriteUpdateItem(_metadata, ref builder, visitor, _classInfo, _node);
            }
            finally
            {
                builder.Dispose();
            }
        }
    }
}