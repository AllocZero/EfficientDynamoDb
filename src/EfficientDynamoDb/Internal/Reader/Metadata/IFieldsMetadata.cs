using System.Diagnostics.CodeAnalysis;

namespace EfficientDynamoDb.Internal.Reader
{
    public interface IFieldsMetadata
    {
        bool TryGetValue(string key, [NotNullWhen(true)] out JsonObjectMetadata? metadata);
    }
}