using System.Text.Json;
using EfficientDynamoDb.Internal.Reader.Metadata;

namespace EfficientDynamoDb.Internal.Reader
{
    internal interface IParsingOptions
    {
        public JsonObjectMetadata? Metadata { get; }
            
        bool HasNumberCallback { get; }

        void OnNumber(ref Utf8JsonReader reader, ref DdbReadStack state);

        void StartParsing(ref DdbReadStack state);
    }
}