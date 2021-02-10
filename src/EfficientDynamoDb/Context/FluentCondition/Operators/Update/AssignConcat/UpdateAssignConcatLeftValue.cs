using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Update.AssignConcat
{
    internal sealed class UpdateAssignConcatLeftValue<TEntity, TProperty> : UpdateBase
    {
        private TProperty _leftValue;
        private readonly Expression _right;

        public UpdateAssignConcatLeftValue(Expression expression, TProperty leftValue, Expression right) : base(expression)
        {
            _right = right;
            _leftValue = leftValue;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "SET #a = list_append(:v0, #b)"
            
            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.Builder);
            
            builder.Append(" = list_append(");
            
            builder.Append(":v");
            builder.Append(valuesCount++);
            
            builder.Append(',');
            
            visitor.Visit<TEntity>(_right);
            builder.Append(visitor.Builder);

            builder.Append(')');
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

            WriteAttributeValue<TEntity, TProperty>(ref builder, writer, ref _leftValue, visitor, ref valuesCount);
        }
    }
}