using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Operations.BatchGetItem;
using EfficientDynamoDb.Operations.BatchWriteItem;
using EfficientDynamoDb.Operations.DeleteItem;
using EfficientDynamoDb.Operations.GetItem;
using EfficientDynamoDb.Operations.PutItem;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Scan;
using EfficientDynamoDb.Operations.TransactGetItems;
using EfficientDynamoDb.Operations.TransactWriteItems.Builders;
using EfficientDynamoDb.Operations.UpdateItem;

namespace EfficientDynamoDb
{
    public interface IDynamoDbContext
    {
        IDynamoDbLowLevelContext LowLevel { get; }
        
        T ToObject<T>(Document document) where T : class;
        
        Document ToDocument<T>(T entity) where T : class;
        
        IScanEntityRequestBuilder<TEntity> Scan<TEntity>() where TEntity : class;
        
        IUpdateEntityRequestBuilder<TEntity> UpdateItem<TEntity>() where TEntity : class;
        
        ITransactGetItemsEntityRequestBuilder TransactGet();
        
        ITransactWriteItemsRequestBuilder TransactWrite();
        
        IGetItemEntityRequestBuilder<TEntity> GetItem<TEntity>() where TEntity : class;

        Task<TEntity?> GetItemAsync<TEntity>(object partitionKey, CancellationToken cancellationToken = default)
            where TEntity : class;

        Task<TEntity?> GetItemAsync<TEntity>(object partitionKey, object sortKey, CancellationToken cancellationToken = default)
            where TEntity : class;

        Task<TEntity?> GetItemAsync<TEntity, TPartitionKey>(TPartitionKey partitionKey, CancellationToken cancellationToken = default)
            where TEntity : class;

        Task<TEntity?> GetItemAsync<TEntity, TPartitionKey, TSortKey>(TPartitionKey partitionKey, TSortKey sortKey,
            CancellationToken cancellationToken = default) where TEntity : class;

        IQueryEntityRequestBuilder<TEntity> Query<TEntity>() where TEntity : class;
        
        IDeleteItemEntityRequestBuilder<TEntity> DeleteItem<TEntity>() where TEntity : class;
        
        Task DeleteItemAsync<TEntity>(object partitionKey, CancellationToken cancellationToken = default) where TEntity : class;
        
        Task DeleteItemAsync<TEntity>(object partitionKey, object sortKey, CancellationToken cancellationToken = default) where TEntity : class;
        
        IPutItemRequestBuilder PutItem();
        
        Task PutItemAsync<TEntity>(TEntity item, CancellationToken cancellationToken = default) where TEntity : class;
        
        IBatchGetEntityRequestBuilder BatchGet();
        
        IBatchWriteItemRequestBuilder BatchWrite();

        internal Task<TResponse> ExecuteAsync<TResponse>(HttpContent httpContent, CancellationToken cancellationToken = default) where TResponse : class =>
            throw new NotSupportedException($"All internal implementations of {nameof(IDynamoDbContext)} should overload ExecuteAsync method");

        internal DynamoDbContextConfig Config =>
            throw new NotSupportedException($"All internal implementations of {nameof(IDynamoDbContext)} should overload Config property");
    }
}