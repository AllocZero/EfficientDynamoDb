using System;
using System.IO;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Extensions;

namespace Benchmarks.Mocks
{
    public static class UpdateItemResponseFactory
    {
        public static byte[] CreateResponse(Document entity)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);
            
            writer.WriteStartObject();
            
            writer.WritePropertyName("Attributes");
            writer.WriteAttributesDictionary(entity);
            
            writer.WriteEndObject();
            writer.Flush();

            stream.Position = 0;
            return stream.ToArray();
        }
    }
}