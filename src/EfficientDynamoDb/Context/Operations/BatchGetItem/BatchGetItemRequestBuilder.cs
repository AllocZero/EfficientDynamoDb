using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.BatchGetItem
{
    internal sealed class BatchGetItemRequestBuilder<TTableEntity, TEntity> : IBatchGetItemRequestBuilder<TTableEntity, TEntity> where TTableEntity : class where TEntity : class
    {
        private readonly BatchGetTableRequestBuilder<TTableEntity> _batchGetTableRequestBuilder;

        public BatchGetItemRequestBuilder(BatchGetTableRequestBuilder<TTableEntity> batchGetTableRequestBuilder)
        {
            _batchGetTableRequestBuilder = batchGetTableRequestBuilder;
        }
        
        public IBatchGetTableRequestBuilder<TTableEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk)
        {
            var entityClassInfo = _batchGetTableRequestBuilder.BatchGetRequestBuilder.Context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            var primaryKeyNode = new EntityPartitionAndSortKeyNode<TPk, TSk>(entityClassInfo, pk, sk, _batchGetTableRequestBuilder.Node);

            return new BatchGetTableRequestBuilder<TTableEntity>(_batchGetTableRequestBuilder.BatchGetRequestBuilder, primaryKeyNode);
        }

        public IBatchGetTableRequestBuilder<TTableEntity> WithPrimaryKey<TPk>(TPk pk)
        {
            var entityClassInfo = _batchGetTableRequestBuilder.BatchGetRequestBuilder.Context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            var primaryKeyNode = new EntityPartitionKeyNode<TPk>(entityClassInfo, pk, _batchGetTableRequestBuilder.Node);

            return new BatchGetTableRequestBuilder<TTableEntity>(_batchGetTableRequestBuilder.BatchGetRequestBuilder, primaryKeyNode);
        }
    }
}