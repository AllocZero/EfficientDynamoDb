using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Update.AssignSum
{
    internal sealed class UpdateAssignAttributesSumRightFallback<TEntity, TProperty> : UpdateBase
    {
        private readonly Expression _left;
        private TProperty _rightFallbackValue;
        private readonly Expression _right;

        public UpdateAssignAttributesSumRightFallback(Expression expression, Expression left, Expression right, TProperty rightFallbackValue) : base(expression)
        {
            _left = left;
            _rightFallbackValue = rightFallbackValue;
            _right = right;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "SET #a = #b + if_not_exists(#c, :v0)"
            
            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.GetEncodedExpressionName());

            builder.Append(" = ");
            
            visitor.Visit<TEntity>(_left);
            builder.Append(visitor.GetEncodedExpressionName());
            
            builder.Append(" + ");
            
            WriteIfNotExistsBlock<TEntity>(ref builder, visitor, _right, ref valuesCount);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

            WriteAttributeValue<TEntity, TProperty>(ref builder, writer, ref _rightFallbackValue, visitor, ref valuesCount);
        }
    }
}