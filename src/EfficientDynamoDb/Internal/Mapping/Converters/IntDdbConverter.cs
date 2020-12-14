using System.Globalization;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters
{
    public sealed class IntDdbConverter : DdbConverter<int>
    {
        public override int Read(AttributeValue attributeValue) => attributeValue.ToInt();
    }
}