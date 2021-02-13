
namespace EfficientDynamoDb.Context.Operations.BatchGetItem
{
    public interface IBatchGetRequestBuilder
    {
        IBatchGetTableRequestBuilder<TTableEntity> FromTable<TTableEntity>() where TTableEntity : class;
    }
}