using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Update.AssignSum
{
    internal sealed class UpdateAssignAttributesSumLeftFallback<TEntity, TProperty> : UpdateBase
    {
        private readonly Expression _left;
        private TProperty _leftFallbackValue;
        private readonly Expression _right;

        public UpdateAssignAttributesSumLeftFallback(Expression expression, Expression left, TProperty leftFallbackValue, Expression right) : base(expression)
        {
            _left = left;
            _leftFallbackValue = leftFallbackValue;
            _right = right;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "SET #a = if_not_exists(#b, :v0) + #c"
            
            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.GetEncodedExpressionName());

            builder.Append(" = ");
            
            WriteIfNotExistsBlock<TEntity>(ref builder, visitor, _left, ref valuesCount);
            
            builder.Append(" + ");
            
            visitor.Visit<TEntity>(_right);
            builder.Append(visitor.GetEncodedExpressionName());
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);
            
            WriteAttributeValue<TEntity, TProperty>(ref builder, writer, ref _leftFallbackValue, visitor, ref valuesCount);
        }
    }
}