using EfficientDynamoDb.Context.Operations.Query;

namespace EfficientDynamoDb.Context.Operations.BatchWriteItem
{
    internal sealed class BatchDeleteItemRequestBuilder<TEntity> : IBatchDeleteItemRequestBuilder where TEntity : class
    {
        private readonly BatchWriteItemRequestBuilder _batchWriteItemRequestBuilder;

        public BatchDeleteItemRequestBuilder(BatchWriteItemRequestBuilder batchWriteItemRequestBuilder)
        {
            _batchWriteItemRequestBuilder = batchWriteItemRequestBuilder;
        }

        public IBatchWriteItemRequestBuilder WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) =>
            new BatchWriteItemRequestBuilder(_batchWriteItemRequestBuilder.Context,
                new DeletePartitionAndSortKeyNode<TPk, TSk>(_batchWriteItemRequestBuilder.Context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)), pk, sk,
                    _batchWriteItemRequestBuilder.Node));

        public IBatchWriteItemRequestBuilder WithPrimaryKey<TPk>(TPk pk) =>
            new BatchWriteItemRequestBuilder(_batchWriteItemRequestBuilder.Context,
                new DeletePartitionKeyNode<TPk>(_batchWriteItemRequestBuilder.Context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)), pk, _batchWriteItemRequestBuilder.Node));
    }
}