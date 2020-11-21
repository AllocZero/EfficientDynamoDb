using System.Text.Json;

namespace EfficientDynamoDb.Internal.Reader
{
    public interface IParsingOptions
    {
        public JsonObjectMetadata? Metadata { get; }
            
        bool HasNumberCallback { get; }

        void OnNumber(ref Utf8JsonReader reader, ref DdbReadStack state);
    }
}