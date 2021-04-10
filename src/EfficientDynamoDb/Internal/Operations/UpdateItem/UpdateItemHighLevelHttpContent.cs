using System.Threading.Tasks;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Internal.Operations.UpdateItem
{
    internal sealed class UpdateItemHighLevelHttpContent : DynamoDbHttpContent
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;
        private readonly DdbClassInfo _classInfo;

        public UpdateItemHighLevelHttpContent(DynamoDbContext context, DdbClassInfo classInfo, BuilderNode? node)
            : base("DynamoDB_20120810.UpdateItem")
        {
            _context = context;
            _node = node;
            _classInfo = classInfo;
        }

        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            ddbWriter.JsonWriter.WriteStartObject();

            var writeState = 0;
            
            WriteUpdateItem(in ddbWriter, ref writeState);

            if (!writeState.IsBitSet(NodeBits.TableName))
                ddbWriter.JsonWriter.WriteTableName(_context.Config.TableNamePrefix, _classInfo.TableName!);

            ddbWriter.JsonWriter.WriteEndObject();

            return new ValueTask();
        }

        private void WriteUpdateItem(in DdbWriter ddbWriter, ref int writeState)
        {
            var visitor = new DdbExpressionVisitor(_context.Config.Metadata);
            var builder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);
            try
            {
                ddbWriter.WriteUpdateItem(_context.Config, ref builder, visitor, _classInfo, _node, ref writeState);
            }
            finally
            {
                builder.Dispose();
            }
        }
    }
}