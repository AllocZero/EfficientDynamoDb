using System;
using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.Internal.Extensions
{
    internal static class EntityMappingExtensions
    {
        public static ValueTask WriteEntityAsync(this DdbWriter writer, object entity,
            DynamoDbContextMetadata metadata) => WriteEntityAsync(writer, metadata.GetOrAddClassInfo(entity.GetType()), entity);
        
        public static async ValueTask WriteEntityAsync(this DdbWriter writer, DdbClassInfo entityClassInfo, object entity)
        {
            writer.JsonWriter.WriteStartObject();
            
            foreach (var property in entityClassInfo.Properties)
            {
                property.Write(entity, writer);
                if (writer.ShouldFlush)
                    await writer.FlushAsync().ConfigureAwait(false);
            }
            
            writer.JsonWriter.WriteEndObject();
        }
    }
}