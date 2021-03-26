using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.DeleteItem
{
    internal sealed class DeleteItemByPkObjectHttpContent<TEntity> : PkObjectHttpContent<TEntity> where TEntity : class
    {
        public DeleteItemByPkObjectHttpContent(DynamoDbContext context, object pk) : base("DynamoDB_20120810.DeleteItem", context, pk)
        {
        }
    }

    internal sealed class DeleteItemByPkAndSkObjectHttpContent<TEntity> : PkAndSkObjectHttpContent<TEntity> where TEntity : class
    {
        public DeleteItemByPkAndSkObjectHttpContent(DynamoDbContext context, object pk, object sk) : base("DynamoDB_20120810.DeleteItem", context, pk, sk)
        {
        }
    }
}