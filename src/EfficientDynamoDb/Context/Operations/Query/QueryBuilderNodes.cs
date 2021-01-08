using System.Collections.Generic;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.Query
{
    internal abstract class BuilderNode<TRequest>
    {
        private BuilderNode<TRequest>? Next { get; }

        protected BuilderNode(BuilderNode<TRequest>? next) => Next = next;

        protected abstract void SetValue(TRequest request);

        public void SetValues(TRequest request)
        {
            Next?.SetValue(request);
            SetValue(request);
        }
    }

    internal abstract class BuilderNode<TRequest, TValue> : BuilderNode<TRequest>
    {
        protected TValue Value { get; }

        protected BuilderNode(TValue value, BuilderNode<TRequest>? next) : base(next) => Value = value;
    }

    internal sealed class IndexNameNode : BuilderNode<QueryHighLevelRequest, string>
    {
        protected override void SetValue(QueryHighLevelRequest request) => request.IndexName = Value;

        public IndexNameNode(string value, BuilderNode<QueryHighLevelRequest>? next) : base(value, next)
        {
        }
    }

    internal sealed class KeyExpressionNode : BuilderNode<QueryHighLevelRequest, FilterBase>
    {
        protected override void SetValue(QueryHighLevelRequest request) => request.KeyExpression = Value;

        public KeyExpressionNode(FilterBase value, BuilderNode<QueryHighLevelRequest>? next) : base(value, next)
        {
        }
    }

    internal sealed class ConsistentReadNode : BuilderNode<QueryHighLevelRequest, bool>
    {
        protected override void SetValue(QueryHighLevelRequest request) => request.ConsistentRead = Value;

        public ConsistentReadNode(bool value, BuilderNode<QueryHighLevelRequest>? next) : base(value, next)
        {
        }
    }

    internal sealed class LimitNode : BuilderNode<QueryHighLevelRequest, int>
    {
        protected override void SetValue(QueryHighLevelRequest request) => request.Limit = Value;

        public LimitNode(int value, BuilderNode<QueryHighLevelRequest>? next) : base(value, next)
        {
        }
    }

    internal sealed class ProjectedAttributesNode : BuilderNode<QueryHighLevelRequest, IReadOnlyList<string>>
    {
        protected override void SetValue(QueryHighLevelRequest request) => request.ProjectionExpression = Value;

        public ProjectedAttributesNode(IReadOnlyList<string> value, BuilderNode<QueryHighLevelRequest>? next) : base(value, next)
        {
        }
    }

    internal sealed class ReturnConsumedCapacityNode : BuilderNode<QueryHighLevelRequest, ReturnConsumedCapacity>
    {
        protected override void SetValue(QueryHighLevelRequest request) => request.ReturnConsumedCapacity = Value;

        public ReturnConsumedCapacityNode(ReturnConsumedCapacity value, BuilderNode<QueryHighLevelRequest>? next) : base(value, next)
        {
        }
    }

    internal sealed class SelectNode : BuilderNode<QueryHighLevelRequest, Select>
    {
        protected override void SetValue(QueryHighLevelRequest request) => request.Select = Value;

        public SelectNode(Select value, BuilderNode<QueryHighLevelRequest>? next) : base(value, next)
        {
        }
    }

    internal sealed class BackwardSearchNode : BuilderNode<QueryHighLevelRequest, bool>
    {
        protected override void SetValue(QueryHighLevelRequest request) => request.ScanIndexForward = !Value;

        public BackwardSearchNode(bool value, BuilderNode<QueryHighLevelRequest>? next) : base(value, next)
        {
        }
    }

    internal sealed class FilterExpressionNode : BuilderNode<QueryHighLevelRequest, FilterBase>
    {
        protected override void SetValue(QueryHighLevelRequest request) => request.FilterExpression = Value;

        public FilterExpressionNode(FilterBase value, BuilderNode<QueryHighLevelRequest>? next) : base(value, next)
        {
        }
    }
}