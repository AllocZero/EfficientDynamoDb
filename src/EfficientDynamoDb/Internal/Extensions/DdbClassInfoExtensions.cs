using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.Internal.Extensions
{
    internal static class DdbClassInfoExtensions
    {
        public static string GetTableName(this DdbClassInfo classInfo) =>
            classInfo.TableName ?? throw new DdbException($"Entity '{classInfo.Type.Name}' has no DynamoDbTable attribute applied.");
    }
}