using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Mapping.Extensions;

namespace EfficientDynamoDb.Internal.Mapping.Converters
{
    public sealed class NestedObjectConverter<T> : DdbConverter<T> where T : class
    {
        public override T Read(AttributeValue attributeValue)
        {
            var document = attributeValue.AsDocument();

            return document.ToObject<T>();
        }
    }
}