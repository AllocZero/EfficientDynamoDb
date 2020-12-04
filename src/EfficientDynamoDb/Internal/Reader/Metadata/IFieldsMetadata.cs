using System.Diagnostics.CodeAnalysis;

namespace EfficientDynamoDb.Internal.Reader.Metadata
{
    internal interface IFieldsMetadata
    {
        bool TryGetValue(string key, [NotNullWhen(true)] out JsonObjectMetadata? metadata);
    }
}