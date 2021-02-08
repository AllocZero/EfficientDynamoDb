using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.BatchWriteItem
{
    public interface IBatchWriteItemRequestBuilder
    {
        IBatchWriteItemRequestBuilder PutItem<TEntity>(TEntity entity) where TEntity : class;
        
        IBatchDeleteItemRequestBuilder DeleteItem<TEntity>() where TEntity : class;

        Task ExecuteAsync();
    }
}