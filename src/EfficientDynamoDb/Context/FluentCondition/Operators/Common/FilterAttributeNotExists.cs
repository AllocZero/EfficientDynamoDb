using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal sealed class FilterAttributeNotExists<TEntity> : FilterBase<TEntity>
    {
        public FilterAttributeNotExists(Expression expression) : base(expression)
        {
        }
        
        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // attribute_not_exists(#a)
            
            visitor.Visit<TEntity>(Expression);
            
            builder.Append("attribute_not_exists(");
            builder.Append(visitor.Builder);
            builder.Append(')');
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // Do nothing
        }
    }
}