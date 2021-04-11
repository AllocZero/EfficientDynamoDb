using EfficientDynamoDb.Converters;

namespace EfficientDynamoDb.Internal.Converters
{
    internal interface IRootDdbConverter<TValue>
    {
        bool TryReadRoot(ref DdbReader reader, out TValue value);
    }
}