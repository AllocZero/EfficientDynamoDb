using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Builder
{
    public class GetItemHttpContent<TPkAttribute, TSkAttribute> : HttpContent
        where TPkAttribute : IAttributeValue
        where TSkAttribute : IAttributeValue
    {
        private readonly string _tableName;
        private readonly string _pkName;
        private readonly TPkAttribute _pkAttributeValue;
        private readonly string _skName;
        private readonly TSkAttribute _skAttributeValue;

        public GetItemHttpContent(string tableName, string pkName, TPkAttribute pkAttributeValue, string skName, TSkAttribute skAttributeValue)
        {
            _tableName = tableName;
            _pkName = pkName;
            _pkAttributeValue = pkAttributeValue;
            _skName = skName;
            _skAttributeValue = skAttributeValue;

            Headers.Add("X-AMZ-Target", "DynamoDB_20120810.GetItem");
            Headers.ContentType = new MediaTypeHeaderValue("application/x-amz-json-1.0");
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            using var writer = new Utf8JsonWriter(stream);
            
            writer.WriteStartObject();
            
            writer.WritePropertyName("Key");
            writer.WriteStartObject();
            
            writer.WritePropertyName(_pkName);
            _pkAttributeValue.Write(writer);
            
            writer.WritePropertyName(_skName);
            _skAttributeValue.Write(writer);
            
            writer.WriteEndObject();
            
            writer.WriteString("TableName", _tableName);

            writer.WriteEndObject();

            return Task.CompletedTask;
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return false;
        }
    }
}