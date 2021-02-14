using System.Threading;
using System.Threading.Tasks;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders
{
    public interface ITransactWriteItemBuilder
    {
        Task ExecuteAsync(CancellationToken cancellationToken = default);

        Task<TransactWriteItemsEntityResponse> ToResponseAsync(CancellationToken cancellationToken = default);

        ITransactConditionCheckBuilder<TEntity> ConditionCheck<TEntity>() where TEntity : class;
        
        ITransactDeleteItemBuilder<TEntity> DeleteItem<TEntity>() where TEntity : class;
        
        ITransactPutItemBuilder<TEntity> PutItem<TEntity>(TEntity entity) where TEntity : class;
        
        ITransactUpdateItemBuilder<TEntity> UpdateItem<TEntity>() where TEntity : class;
    }
}