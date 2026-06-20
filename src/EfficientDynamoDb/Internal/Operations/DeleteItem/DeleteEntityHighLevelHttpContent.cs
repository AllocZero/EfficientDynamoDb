using System.Threading.Tasks;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.DeleteItem
{
    internal sealed class DeleteEntityHighLevelHttpContent<TEntity> : DynamoDbHttpContent where TEntity : class
    {
        private readonly DynamoDbContextConfig _config;
        private readonly TEntity _entity;

        public DeleteEntityHighLevelHttpContent(DynamoDbContextConfig config, TEntity entity) : base("DynamoDB_20120810.DeleteItem")
        {
            _config = config;
            _entity = entity;
        }

        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

            var classInfo = _config.Metadata.GetOrAddClassInfo<TEntity>();
            writer.WriteTableName(_config.TableNameFormatter, classInfo.TableName!);

            writer.WritePropertyName("Key");
            writer.WriteStartObject();
            classInfo.PartitionKey!.Write(_entity, in ddbWriter);
            classInfo.SortKey?.Write(_entity, in ddbWriter);
            writer.WriteEndObject();

            if (classInfo.Version != null)
            {
                var isVersionNull = classInfo.Version.IsNull(_entity);
                writer.WriteString("ConditionExpression", isVersionNull ? "attribute_not_exists(#version)" : "#version = :version");

                writer.WritePropertyName("ExpressionAttributeNames");

                writer.WriteStartObject();
                writer.WriteString("#version", classInfo.Version.AttributeName);
                writer.WriteEndObject();

                if (!isVersionNull)
                {
                    writer.WritePropertyName("ExpressionAttributeValues");

                    writer.WriteStartObject();
                    writer.WritePropertyName(":version");
                    classInfo.Version.WriteValue(_entity, in ddbWriter);
                    writer.WriteEndObject();
                }
            }

            writer.WriteEndObject();

            return default;
        }
    }
}