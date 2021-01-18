using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal sealed class FilterLessThan<TEntity, TProperty> : FilterBase<TEntity>
    {
        private TProperty _value;

        public FilterLessThan(Expression expression, TProperty value) : base(expression) => _value = value;

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount,
            DdbExpressionVisitor visitor)
        {
            // "#a < :v0"

            visitor.Visit<TEntity>(Expression);
            
            builder.Append('#');
            builder.Append(visitor.GetEncodedExpressionName());
            builder.Append(" < :v");
            builder.Append(valuesCount++);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

            builder.Append(":v");
            builder.Append(valuesCount++);
            
            writer.JsonWriter.WritePropertyName(builder.GetBuffer());
            GetPropertyConverter<TProperty>(visitor).Write(in writer, ref _value);
        }
    }
}