using System.Collections.Generic;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.Operations.Shared;
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

    internal sealed class IndexNameNode<TRequest> : BuilderNode<TRequest, string> where TRequest: IIndexName
    {
        protected override void SetValue(TRequest request) => request.IndexName = Value;

        public IndexNameNode(string value, BuilderNode<TRequest>? next) : base(value, next)
        {
        }
    }

    internal sealed class KeyExpressionNode<TRequest> : BuilderNode<TRequest, FilterBase> where TRequest: IKeyExpression
    {
        protected override void SetValue(TRequest request) => request.KeyExpression = Value;

        public KeyExpressionNode(FilterBase value, BuilderNode<TRequest>? next) : base(value, next)
        {
        }
    }

    internal sealed class ConsistentReadNode<TRequest> : BuilderNode<TRequest, bool> where TRequest: IConsistentRead
    {
        protected override void SetValue(TRequest request) => request.ConsistentRead = Value;

        public ConsistentReadNode(bool value, BuilderNode<TRequest>? next) : base(value, next)
        {
        }
    }

    internal sealed class LimitNode<TRequest> : BuilderNode<TRequest, int> where TRequest: ILimit
    {
        protected override void SetValue(TRequest request) => request.Limit = Value;

        public LimitNode(int value, BuilderNode<TRequest>? next) : base(value, next)
        {
        }
    }

    internal sealed class ProjectedAttributesNode<TRequest> : BuilderNode<TRequest, IReadOnlyList<string>>  where TRequest: IProjectionExpression
    {
        protected override void SetValue(TRequest request) => request.ProjectionExpression = Value;

        public ProjectedAttributesNode(IReadOnlyList<string> value, BuilderNode<TRequest>? next) : base(value, next)
        {
        }
    }

    internal sealed class ReturnConsumedCapacityNode<TRequest> : BuilderNode<TRequest, ReturnConsumedCapacity> where TRequest: IReturnConsumedCapacity
    {
        protected override void SetValue(TRequest request) => request.ReturnConsumedCapacity = Value;

        public ReturnConsumedCapacityNode(ReturnConsumedCapacity value, BuilderNode<TRequest>? next) : base(value, next)
        {
        }
    }

    internal sealed class SelectNode<TRequest> : BuilderNode<TRequest, Select> where TRequest: ISelect
    {
        protected override void SetValue(TRequest request) => request.Select = Value;

        public SelectNode(Select value, BuilderNode<TRequest>? next) : base(value, next)
        {
        }
    }

    internal sealed class BackwardSearchNode<TRequest> : BuilderNode<TRequest, bool> where TRequest: IScanIndexForward
    {
        protected override void SetValue(TRequest request) => request.ScanIndexForward = !Value;

        public BackwardSearchNode(bool value, BuilderNode<TRequest>? next) : base(value, next)
        {
        }
    }

    internal sealed class FilterExpressionNode<TRequest> : BuilderNode<TRequest, FilterBase> where TRequest: IFilterExpression
    {
        protected override void SetValue(TRequest request) => request.FilterExpression = Value;

        public FilterExpressionNode(FilterBase value, BuilderNode<TRequest>? next) : base(value, next)
        {
        }
    }
}