using System;
using System.IO;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Extensions;

namespace Benchmarks.Mocks
{
    public static class QueryResponseFactory
    {
        public static byte[] CreateResponse(Func<int, Document> entityFactory, int itemsCount)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);
           
            writer.WriteStartObject();
            writer.WriteNumber("Count", itemsCount);
           
            writer.WritePropertyName("Items");
            writer.WriteStartArray();

            for (var i = 0; i < itemsCount; i++)
                writer.WriteAttributesDictionary(entityFactory(i));
            
            writer.WriteEndArray();

            writer.WriteNumber("ScannedCount", itemsCount);
            
            writer.WriteEndObject();
            writer.Flush();

            stream.Position = 0;
            return stream.ToArray();
        }
    }
}