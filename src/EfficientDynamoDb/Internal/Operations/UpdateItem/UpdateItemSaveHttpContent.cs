using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.UpdateItem
{
    internal sealed class UpdateItemSaveHttpContent<TEntity> : DynamoDbHttpContent where TEntity : class
    {
        // TODO: Replace table prefix and metadata with context in all http content implementations
        private readonly DynamoDbContext _context;
        private readonly TEntity _entity;

        public UpdateItemSaveHttpContent(DynamoDbContext context, TEntity entity)
            : base("DynamoDB_20120810.UpdateItem")
        {
            _context = context;
            _entity = entity;
        }

        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            ddbWriter.JsonWriter.WriteStartObject();

            var classInfo = _context.Config.Metadata.GetOrAddClassInfo<TEntity>();
            
            ddbWriter.JsonWriter.WriteTableName(_context.Config.TableNamePrefix, classInfo.TableName!);
            
            ddbWriter.JsonWriter.WritePropertyName("Key");
            ddbWriter.JsonWriter.WriteStartObject();
            classInfo.PartitionKey!.Write(_entity, in ddbWriter);
            classInfo.SortKey?.Write(_entity, in ddbWriter);
            ddbWriter.JsonWriter.WriteEndObject();

            WriteUpdateItem(in ddbWriter, classInfo);

            ddbWriter.JsonWriter.WriteEndObject();

            return new ValueTask();
        }

        private void WriteUpdateItem(in DdbWriter ddbWriter, DdbClassInfo classInfo)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);
            try
            {
                var hasRemove = false;
                var firstAdd = true;

                // Write SET expression
                for (var i = 0; i < classInfo.Properties.Length; i++)
                {
                    var property = classInfo.Properties[i];
                    if (property.AttributeType != DynamoDbAttributeType.Regular)
                        continue;
                    
                    if (!property.ShouldWrite(_entity))
                    {
                        hasRemove = true;
                        continue;
                    }

                    if (firstAdd)
                        builder.Append("SET ");
                    else
                        builder.Append(',');

                    builder.Append("#f");
                    builder.Append(i);
                    builder.Append(" = ");
                    builder.Append(":v");
                    builder.Append(i);

                    firstAdd = false;
                }

                // Write Remove expression
                if (hasRemove)
                {
                    if(!firstAdd) 
                        builder.Append(' ');
                        
                    builder.Append("REMOVE ");

                    var firstRemove = true;
                    for (var i = 0; i < classInfo.Properties.Length; i++)
                    {
                        var property = classInfo.Properties[i];
                        if (property.AttributeType != DynamoDbAttributeType.Regular)
                            continue;
                        
                        if (property.ShouldWrite(_entity))
                            continue;
                        
                        if(!firstRemove)
                            builder.Append(',');
                        
                        builder.Append("#f");
                        builder.Append(i);

                        firstRemove = false;
                    }
                }
                
                ddbWriter.JsonWriter.WriteString("UpdateExpression", builder.GetBuffer());
                builder.Clear();

                WriteExpressionAttributeNames(in ddbWriter, ref builder, classInfo);
                WriteExpressionAttributeValues(in ddbWriter, ref builder, classInfo);
            }
            finally
            {
                builder.Dispose();
            }
        }

        private static void WriteExpressionAttributeNames(in DdbWriter ddbWriter, ref NoAllocStringBuilder builder, DdbClassInfo classInfo)
        {
            ddbWriter.JsonWriter.WritePropertyName("ExpressionAttributeNames");

            ddbWriter.JsonWriter.WriteStartObject();
            for (var i = 0; i < classInfo.Properties.Length; i++)
            {
                var property = classInfo.Properties[i];
                if (property.AttributeType != DynamoDbAttributeType.Regular)
                    continue;
                
                builder.Append("#f");
                builder.Append(i);

                ddbWriter.JsonWriter.WriteString(builder.GetBuffer(), property.AttributeName);

                builder.Clear();
            }
            
            ddbWriter.JsonWriter.WriteEndObject();
        }

        private void WriteExpressionAttributeValues(in DdbWriter ddbWriter, ref NoAllocStringBuilder builder, DdbClassInfo classInfo)
        {
            ddbWriter.JsonWriter.WritePropertyName("ExpressionAttributeValues");

            ddbWriter.JsonWriter.WriteStartObject();
            for (var i = 0; i < classInfo.Properties.Length; i++)
            {
                var property = classInfo.Properties[i];
                if (property.AttributeType != DynamoDbAttributeType.Regular)
                    continue;
                
                if (!property.ShouldWrite(_entity))
                    continue;
                
                builder.Append(":v");
                builder.Append(i);

                ddbWriter.JsonWriter.WritePropertyName(builder.GetBuffer());
                property.WriteValue(_entity, in ddbWriter);
                builder.Clear();
            }
            
            ddbWriter.JsonWriter.WriteEndObject();
        }
    }
}