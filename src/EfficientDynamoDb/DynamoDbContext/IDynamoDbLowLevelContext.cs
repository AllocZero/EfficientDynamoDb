using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Operations.BatchExecuteStatement;
using EfficientDynamoDb.Operations.BatchGetItem;
using EfficientDynamoDb.Operations.BatchWriteItem;
using EfficientDynamoDb.Operations.DeleteItem;
using EfficientDynamoDb.Operations.ExecuteStatement;
using EfficientDynamoDb.Operations.ExecuteTransaction;
using EfficientDynamoDb.Operations.GetItem;
using EfficientDynamoDb.Operations.PutItem;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Scan;
using EfficientDynamoDb.Operations.TransactGetItems;
using EfficientDynamoDb.Operations.TransactWriteItems;
using EfficientDynamoDb.Operations.UpdateItem;

namespace EfficientDynamoDb
{
    public interface IDynamoDbLowLevelContext
    {
        Task<GetItemResponse> GetItemAsync(GetItemRequest request, CancellationToken cancellationToken = default);
        
        Task<BatchGetItemResponse> BatchGetItemAsync(BatchGetItemRequest request, CancellationToken cancellationToken = default);
        
        Task<BatchWriteItemResponse> BatchWriteItemAsync(BatchWriteItemRequest request, CancellationToken cancellationToken = default);
        
        Task<QueryResponse> QueryAsync(QueryRequest request, CancellationToken cancellationToken = default);
        
        Task<ScanResponse> ScanAsync(ScanRequest request, CancellationToken cancellationToken = default);
        
        Task<TransactGetItemsResponse> TransactGetItemsAsync(TransactGetItemsRequest request, CancellationToken cancellationToken = default);
        
        Task<PutItemResponse> PutItemAsync(PutItemRequest request, CancellationToken cancellationToken = default);
        
        Task<UpdateItemResponse> UpdateItemAsync(UpdateItemRequest request, CancellationToken cancellationToken = default);
        
        Task<DeleteItemResponse> DeleteItemAsync(DeleteItemRequest request, CancellationToken cancellationToken = default);
        
        Task<TransactWriteItemsResponse> TransactWriteItemsAsync(TransactWriteItemsRequest request, CancellationToken cancellationToken = default);

        Task<ExecuteStatementResponse> ExecuteStatementAsync(ExecuteStatementRequest request, CancellationToken cancellationToken = default);

        Task<BatchExecuteStatementResponse> BatchExecuteStatementAsync(BatchExecuteStatementRequest request, CancellationToken cancellationToken = default);

        T ToObject<T>(Document document) where T : class;
        
        Document ToDocument<T>(T entity) where T : class;
    }
}