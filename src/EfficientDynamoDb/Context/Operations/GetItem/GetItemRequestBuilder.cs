using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.GetItem
{
    internal sealed class GetItemEntityRequestBuilder<TEntity> : IGetItemEntityRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public GetItemEntityRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        private GetItemEntityRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public IGetItemEntityRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead) =>
            new GetItemEntityRequestBuilder<TEntity>(_context, new ConsistentReadNode(useConsistentRead, _node));
        
        public IGetItemEntityRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode) =>
            new GetItemEntityRequestBuilder<TEntity>(_context, new ReturnConsumedCapacityNode(consumedCapacityMode, _node));

        public IGetItemEntityRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) =>
            new GetItemEntityRequestBuilder<TEntity>(_context, new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, _node));

        public IGetItemEntityRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk) =>
            new GetItemEntityRequestBuilder<TEntity>(_context, new PartitionKeyNode<TPk>(pk, _node));
        
        public IGetItemEntityRequestBuilder<TEntity, TProjection> WithProjectedAttributes<TProjection>() where TProjection : class =>
            new GetItemEntityRequestBuilder<TEntity, TProjection>(_context, new ProjectedAttributesNode(typeof(TProjection), null, _node));

        public IGetItemEntityRequestBuilder<TEntity, TProjection> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class =>
            new GetItemEntityRequestBuilder<TEntity, TProjection>(_context, new ProjectedAttributesNode(typeof(TProjection), properties, _node));

        public IGetItemEntityRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties) =>
            new GetItemEntityRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(typeof(TEntity), properties, _node));

        public IGetItemDocumentRequestBuilder<TEntity> AsDocument() => new GetItemDocumentRequestBuilder<TEntity>(_context, _node);

        public async Task<TEntity?> ToItemAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.GetItemAsync<TEntity>(classInfo, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<GetItemEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.GetItemResponseAsync<TEntity>(classInfo, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty batch get item request.");
    }
    
    internal sealed class GetItemEntityRequestBuilder<TEntity, TProjection> : IGetItemEntityRequestBuilder<TEntity, TProjection> where TEntity : class where TProjection : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public GetItemEntityRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        internal GetItemEntityRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public IGetItemEntityRequestBuilder<TEntity, TProjection> WithConsistentRead(bool useConsistentRead) =>
            new GetItemEntityRequestBuilder<TEntity, TProjection>(_context, new ConsistentReadNode(useConsistentRead, _node));
        
        public IGetItemEntityRequestBuilder<TEntity, TProjection> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode) =>
            new GetItemEntityRequestBuilder<TEntity, TProjection>(_context, new ReturnConsumedCapacityNode(consumedCapacityMode, _node));

        public IGetItemEntityRequestBuilder<TEntity, TProjection> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) =>
            new GetItemEntityRequestBuilder<TEntity, TProjection>(_context, new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, _node));

        public IGetItemEntityRequestBuilder<TEntity, TProjection> WithPrimaryKey<TPk>(TPk pk) =>
            new GetItemEntityRequestBuilder<TEntity, TProjection>(_context, new PartitionKeyNode<TPk>(pk, _node));

        public IGetItemDocumentRequestBuilder<TEntity> AsDocument() => new GetItemDocumentRequestBuilder<TEntity>(_context, _node);

        public async Task<TProjection?> ToItemAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.GetItemAsync<TProjection>(classInfo, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<GetItemEntityResponse<TProjection>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.GetItemResponseAsync<TProjection>(classInfo, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty batch get item request.");
    }
    
    internal sealed class GetItemDocumentRequestBuilder<TEntity> : IGetItemDocumentRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public GetItemDocumentRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        internal GetItemDocumentRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public IGetItemDocumentRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead) =>
            new GetItemDocumentRequestBuilder<TEntity>(_context, new ConsistentReadNode(useConsistentRead, _node));

        public IGetItemDocumentRequestBuilder<TEntity> WithProjectedAttributes<TProjection>() where TProjection : class =>
            new GetItemDocumentRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(typeof(TProjection), null, _node));

        public IGetItemDocumentRequestBuilder<TEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class =>
            new GetItemDocumentRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(typeof(TProjection), properties, _node));

        public IGetItemDocumentRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties) =>
            new GetItemDocumentRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(typeof(TEntity), properties, _node));

        public IGetItemDocumentRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode) =>
            new GetItemDocumentRequestBuilder<TEntity>(_context, new ReturnConsumedCapacityNode(consumedCapacityMode, _node));

        public IGetItemDocumentRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) =>
            new GetItemDocumentRequestBuilder<TEntity>(_context, new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, _node));

        public IGetItemDocumentRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk) =>
            new GetItemDocumentRequestBuilder<TEntity>(_context, new PartitionKeyNode<TPk>(pk, _node));
        

        public async Task<Document?> ToItemAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.GetItemAsync<Document>(classInfo, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<GetItemEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.GetItemResponseAsync<Document>(classInfo, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty batch get item request.");
    }
}