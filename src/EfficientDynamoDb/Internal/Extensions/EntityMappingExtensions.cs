using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Internal.Extensions
{
    internal static class EntityMappingExtensions
    {
        public static async ValueTask WriteEntityAsync(this Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter, object entity,
            DynamoDbContextMetadata metadata)
        {
            var classInfo = metadata.GetOrAddClassInfo(entity.GetType());

            foreach (var property in classInfo.Properties)
            {
                property.Write(entity, writer);
                if (bufferWriter.ShouldWrite(writer))
                    await bufferWriter.WriteToStreamAsync().ConfigureAwait(false);
            }
        }
    }
}