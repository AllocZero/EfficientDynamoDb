using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.GetItem
{
    public readonly struct GetItemRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public GetItemRequestBuilder(DynamoDbContext context)
        {
            _context = context;
            _node = null;
        }

        private GetItemRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public GetItemRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead) =>
            new GetItemRequestBuilder<TEntity>(_context, new ConsistentReadNode(useConsistentRead, _node));

        public GetItemRequestBuilder<TEntity> WithProjectedAttributes<TProjection>() where TProjection : class =>
            new GetItemRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(typeof(TProjection), null, _node));

        public GetItemRequestBuilder<TEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class =>
            new GetItemRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(typeof(TProjection), properties, _node));

        public GetItemRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties) =>
            new GetItemRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(typeof(TEntity), properties, _node));

        public GetItemRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode) =>
            new GetItemRequestBuilder<TEntity>(_context, new ReturnConsumedCapacityNode(consumedCapacityMode, _node));

        public GetItemRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) =>
            new GetItemRequestBuilder<TEntity>(_context, new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, _node));

        public GetItemRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk) =>
            new GetItemRequestBuilder<TEntity>(_context, new PartitionKeyNode<TPk>(pk, _node));
        
        public async Task<TEntity?> ToEntityAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.GetItemAsync<TEntity>(classInfo, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<Document?> ToDocumentAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.GetItemAsync<Document>(classInfo, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<GetItemEntityResponse<TEntity>> ToEntityResponseAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.GetItemResponseAsync<TEntity>(classInfo, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<GetItemEntityResponse<Document>> ToDocumentResponseAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.GetItemResponseAsync<Document>(classInfo, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty batch get item request.");
    }
}