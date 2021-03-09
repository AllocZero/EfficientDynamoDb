using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.Operations.Shared
{
    internal abstract class PkAndSkObjectHttpContent<TEntity> : DynamoDbHttpContent where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly object _pk;
        private readonly object _sk;

        protected PkAndSkObjectHttpContent(string amzTarget, DynamoDbContext context, object pk, object sk) : base(amzTarget)
        {
            _context = context;
            _pk = pk;
            _sk = sk;
        }
        
        protected sealed override ValueTask WriteDataAsync(DdbWriter ddbWriter)
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