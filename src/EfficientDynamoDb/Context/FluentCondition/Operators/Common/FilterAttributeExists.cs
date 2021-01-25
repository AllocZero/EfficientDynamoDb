using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal sealed class FilterAttributeExists<TEntity> : FilterBase<TEntity>
    {
        public FilterAttributeExists(Expression expression) : base(expression)
        {
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // attribute_exists(#a)

            visitor.Visit<TEntity>(Expression);
            
            builder.Append("attribute_exists(");
            builder.Append(visitor.GetEncodedExpressionName());
            builder.Append(')');
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // Do nothing
        }
    }
}