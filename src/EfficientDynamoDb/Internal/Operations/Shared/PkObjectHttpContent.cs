using System.Threading.Tasks;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.Operations.Shared
{
    internal abstract class PkObjectHttpContent<TEntity> : DynamoDbHttpContent where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly object _pk;

        protected PkObjectHttpContent(string amzTarget, DynamoDbContext context, object pk) : base(amzTarget)
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
}