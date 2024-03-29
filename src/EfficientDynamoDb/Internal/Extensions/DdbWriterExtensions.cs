using System.Text;
using System.Threading.Tasks;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.Internal.Extensions
{
    internal static class DdbWriterExtensions
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
        
        public static void WriteEntity(this DdbWriter writer, DdbClassInfo entityClassInfo, object entity)
        {
            writer.JsonWriter.WriteStartObject();
            
            foreach (var property in entityClassInfo.Properties)
                property.Write(entity, writer);
            
            writer.JsonWriter.WriteEndObject();
        }
        
        public static void WritePaginationToken(this DdbWriter writer, string paginationToken)
        {
            writer.JsonWriter.WritePropertyName("ExclusiveStartKey");
            
            writer.JsonWriter.WriteNullValue();
            // Flush to make sure our changes don't overlap with pending changes
            writer.JsonWriter.Flush();

            // Dirty hack until System.Text.Json supports writing raw json
            writer.BufferWriter.Advance(-4);
            var bytesSize = Encoding.UTF8.GetByteCount(paginationToken);

            var bytesWritten = Encoding.UTF8.GetBytes(paginationToken, writer.BufferWriter.GetSpan(bytesSize));
            writer.BufferWriter.Advance(bytesWritten);
        }
    }
}