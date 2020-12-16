using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Mapping.Extensions;

namespace EfficientDynamoDb.Internal.Mapping.Converters
{
    internal sealed class NestedObjectConverter<T> : DdbConverter<T> where T : class
    {
        public override T Read(in AttributeValue attributeValue)
        {
            var document = attributeValue.AsDocument();

            return document.ToObject<T>();
        }
    }
}