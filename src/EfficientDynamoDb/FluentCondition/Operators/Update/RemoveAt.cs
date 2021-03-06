using System.Linq.Expressions;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.FluentCondition.Operators.Update
{
    internal sealed class UpdateRemoveAt<TEntity> : UpdateBase
    {
        private readonly int _index;

        public UpdateRemoveAt(Expression expression, int index) : base(expression)
        {
            _index = index;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "REMOVE #a[0]"

            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.Builder);
            builder.Append('[');
            builder.Append(_index);
            builder.Append(']');
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // Do nothing
        }
    }
}