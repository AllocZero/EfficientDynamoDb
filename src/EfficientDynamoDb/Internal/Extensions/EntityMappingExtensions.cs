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
        public static ValueTask WriteEntityAsync(this Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter, object entity,
            DynamoDbContextMetadata metadata) => WriteEntityAsync(writer, bufferWriter, metadata.GetOrAddClassInfo(entity.GetType()), entity);
        
        public static async ValueTask WriteEntityAsync(this Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter, DdbClassInfo entityClassInfo, object entity)
        {
            foreach (var property in entityClassInfo.Properties)
            {
                property.Write(entity, writer);
                if (bufferWriter.ShouldWrite(writer))
                    await bufferWriter.WriteToStreamAsync().ConfigureAwait(false);
            }
        }
    }
}