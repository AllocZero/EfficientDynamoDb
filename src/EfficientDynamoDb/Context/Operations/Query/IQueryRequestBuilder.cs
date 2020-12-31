using System.Collections.Generic;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.Query
{
    public interface IQueryRequestBuilder
    {
        internal QueryHighLevelRequest Build(string tableName);

        public IQueryRequestBuilder WithKeyExpression(IFilter keyExpressionBuilder);

        public IQueryRequestBuilder FromIndex(string indexName);

        public IQueryRequestBuilder WithConsistentRead(bool useConsistentRead);

        public IQueryRequestBuilder WithLimit(int limit);

        public IQueryRequestBuilder WithProjectedAttributes(IReadOnlyList<string> projectedAttributes);

        public IQueryRequestBuilder ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);

        public IQueryRequestBuilder WithSelectMode(Select selectMode);

        public IQueryRequestBuilder BackwardSearch(bool useBackwardSearch);

        public IQueryRequestBuilder WithFilterExpression(IFilter filterExpressionBuilder);
    }
}