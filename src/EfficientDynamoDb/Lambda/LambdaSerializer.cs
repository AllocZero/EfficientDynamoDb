using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Internal.Converters.Json;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Lambda
{
    public static class LambdaSerializer
    {
        private static readonly DynamoDbContextMetadata EmptyMetadata = new DynamoDbContextMetadata(Array.Empty<DdbConverter>());
        private static readonly DdbClassInfo EventClassInfo = EmptyMetadata.GetOrAddClassInfo(typeof(DynamoDbEvent), typeof(JsonObjectDdbConverter<DynamoDbEvent>));
        
        public static async Task<DynamoDbEvent> DeserializeAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            var result = await EntityDdbJsonReader.ReadAsync<DynamoDbEvent>(stream, EventClassInfo, EmptyMetadata, false, cancellationToken: cancellationToken).ConfigureAwait(false);

            return result.Value!;
        }
    }
}