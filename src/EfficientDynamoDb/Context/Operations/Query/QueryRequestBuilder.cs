using System;
using System.Collections.Generic;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.Query
{
    public class QueryRequestBuilder : IQueryRequestBuilder
    {
        private IFilter? _keyExpressionBuilder;
        private string? _indexName;
        private bool _consistentRead;
        private int? _limit;
        private IReadOnlyList<string>? _projectionExpression;
        private ReturnConsumedCapacity _returnConsumedCapacity;
        private Select _select;
        private bool _scanIndexForward = true;
        private IFilter? _filterExpressionBuilder;
        // TODO: Add ExclusiveStartKey support

        QueryHighLevelRequest IQueryRequestBuilder.Build(string tableName)
        {
            if (_keyExpressionBuilder == null)
                throw new InvalidOperationException("Key builder must be specified");

            return new QueryHighLevelRequest
            {
                Limit = _limit,
                Select = _select,
                ConsistentRead = _consistentRead,
                IndexName = _indexName,
                ProjectionExpression = _projectionExpression,
                TableName = tableName,
                ReturnConsumedCapacity = _returnConsumedCapacity,
                ScanIndexForward = _scanIndexForward,
                KeyExpression = _keyExpressionBuilder,
                FilterExpression = _filterExpressionBuilder
            };
        }

        public IQueryRequestBuilder WithKeyExpression(IFilter keyExpressionBuilder)
        {
            var copy = DeepCopy();
            copy._keyExpressionBuilder = keyExpressionBuilder;

            return copy;
        }

        public IQueryRequestBuilder FromIndex(string indexName)
        {
            var copy = DeepCopy();
            copy._indexName = indexName;

            return copy;
        }

        public IQueryRequestBuilder WithConsistentRead(bool useConsistentRead)
        {
            var copy = DeepCopy();
            copy._consistentRead = useConsistentRead;

            return copy;
        }
        
        public IQueryRequestBuilder WithLimit(int limit)
        {
            var copy = DeepCopy();
            copy._limit = limit;

            return copy;
        }
        
        public IQueryRequestBuilder WithProjectedAttributes(IReadOnlyList<string> projectedAttributes)
        {
            var copy = DeepCopy();
            copy._projectionExpression = projectedAttributes;

            return copy;
        }
        
        public IQueryRequestBuilder ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode)
        {
            var copy = DeepCopy();
            copy._returnConsumedCapacity = consumedCapacityMode;

            return copy;
        }
        
        public IQueryRequestBuilder WithSelectMode(Select selectMode)
        {
            var copy = DeepCopy();
            copy._select = selectMode;

            return copy;
        }
        
        public IQueryRequestBuilder BackwardSearch(bool useBackwardSearch)
        {
            var copy = DeepCopy();
            copy._scanIndexForward = !useBackwardSearch;

            return copy;
        }
        
        public IQueryRequestBuilder WithFilterExpression(IFilter filterExpressionBuilder)
        {
            var copy = DeepCopy();
            copy._filterExpressionBuilder = filterExpressionBuilder;

            return copy;
        }

        private QueryRequestBuilder DeepCopy() => new QueryRequestBuilder
        {
            _keyExpressionBuilder = _keyExpressionBuilder,
            _indexName = _indexName,
            _consistentRead = _consistentRead,
            _limit = _limit,
            _projectionExpression = _projectionExpression,
            _returnConsumedCapacity = _returnConsumedCapacity,
            _select = _select,
            _scanIndexForward = _scanIndexForward,
            _filterExpressionBuilder = _filterExpressionBuilder
        };
    }
    
}