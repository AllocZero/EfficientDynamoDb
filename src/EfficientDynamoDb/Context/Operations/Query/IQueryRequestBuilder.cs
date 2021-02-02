using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.Query
{
    public interface IQueryRequestBuilder
    {
        public Task<IReadOnlyList<TEntity>> ToListAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class;
        
        public Task<QueryEntityResponse<TEntity>> ToResponseAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class;
        
        public IQueryRequestBuilder WithKeyExpression(FilterBase keyExpressionBuilder);
        
        public IQueryRequestBuilder FromIndex(string indexName);

        public IQueryRequestBuilder WithConsistentRead(bool useConsistentRead);

        public IQueryRequestBuilder WithLimit(int limit);

        public IQueryRequestBuilder WithProjectedAttributes(IReadOnlyList<string> projectedAttributes);

        public IQueryRequestBuilder ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);

        public IQueryRequestBuilder WithSelectMode(Select selectMode);

        public IQueryRequestBuilder BackwardSearch(bool useBackwardSearch);

        public IQueryRequestBuilder WithFilterExpression(FilterBase filterExpressionBuilder);
        
        IQueryRequestBuilder WithPaginationToken(string? paginationToken);
    }
}