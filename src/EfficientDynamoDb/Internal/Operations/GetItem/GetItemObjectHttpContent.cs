using EfficientDynamoDb.Context;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.GetItem
{
    internal sealed class GetItemByPkObjectHttpContent<TEntity> : PkObjectHttpContent<TEntity> where TEntity : class
    {
        public GetItemByPkObjectHttpContent(DynamoDbContext context, object pk) : base("DynamoDB_20120810.GetItem", context, pk)
        {
        }
    }

    internal sealed class GetItemByPkAndSkObjectHttpContent<TEntity> : PkAndSkObjectHttpContent<TEntity> where TEntity : class
    {
        public GetItemByPkAndSkObjectHttpContent(DynamoDbContext context, object pk, object sk) : base("DynamoDB_20120810.GetItem", context, pk, sk)
        {
        }
    }
}