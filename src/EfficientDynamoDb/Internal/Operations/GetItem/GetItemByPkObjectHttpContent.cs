using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.GetItem
{
    internal sealed class GetItemByPkObjectHttpContent<TEntity> : DynamoDbHttpContent where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly object _pk;

        public GetItemByPkObjectHttpContent(DynamoDbContext context,object pk) : base("DynamoDB_20120810.GetItem")
        {
            _context = context;
            _pk = pk;
        }

        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

            var classInfo = _context.Config.Metadata.GetOrAddClassInfo<TEntity>();
            
            writer.WriteTableName(_context.Config.TableNamePrefix, classInfo.TableName!);
            
            writer.WritePropertyName("Key");
            writer.WriteStartObject();
            writer.WritePropertyName(classInfo.PartitionKey!.AttributeName);
            classInfo.PartitionKey!.ConverterBase.Write(in ddbWriter, _pk);
            writer.WriteEndObject();
            
            writer.WriteEndObject();

            return new ValueTask();
        }
    }

    internal sealed class GetItemByPkAndSkObjectHttpContent<TEntity> : DynamoDbHttpContent where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly object _pk;
        private readonly object _sk;

        public GetItemByPkAndSkObjectHttpContent(DynamoDbContext context, object pk, object sk) : base("DynamoDB_20120810.GetItem")
        {
            _context = context;
            _pk = pk;
            _sk = sk;
        }

        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

            var classInfo = _context.Config.Metadata.GetOrAddClassInfo<TEntity>();

            writer.WriteTableName(_context.Config.TableNamePrefix, classInfo.TableName!);
            
            writer.WritePropertyName("Key");
            writer.WriteStartObject();
            writer.WritePropertyName(classInfo.PartitionKey!.AttributeName);
            classInfo.PartitionKey!.ConverterBase.Write(in ddbWriter, _pk);
            writer.WritePropertyName(classInfo.SortKey!.AttributeName);
            classInfo.SortKey!.ConverterBase.Write(in ddbWriter, _sk);
            writer.WriteEndObject();
            
            writer.WriteEndObject();

            return new ValueTask();
        }
    }
}