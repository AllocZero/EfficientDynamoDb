using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.Operations.Query
{
    public interface IBasicQueryRequestBuilder
    {
        public IQueryRequestBuilder WithKeyExpression(FilterBase keyExpressionBuilder);
    }
}