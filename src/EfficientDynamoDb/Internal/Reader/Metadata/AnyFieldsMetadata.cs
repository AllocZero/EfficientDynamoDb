using System.Diagnostics.CodeAnalysis;

namespace EfficientDynamoDb.Internal.Reader.Metadata
{
    internal class AnyFieldsMetadata : IFieldsMetadata
    {
        private readonly JsonObjectMetadata _metadata;

        public AnyFieldsMetadata(JsonObjectMetadata metadata)
        {
            _metadata = metadata;
        }
        
        public bool TryGetValue(string key, [NotNullWhen(true)] out JsonObjectMetadata? metadata)
        {
            metadata = _metadata;
            return true;
        }
    }
}