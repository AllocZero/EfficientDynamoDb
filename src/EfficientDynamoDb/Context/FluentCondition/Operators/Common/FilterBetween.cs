using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal sealed class FilterBetween<TEntity, TProperty> : FilterBase<TEntity>
    {
        private TProperty _min;
        private TProperty _max;

        public FilterBetween(Expression expression, TProperty min, TProperty max) : base(expression)
        {
            _min = min;
            _max = max;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount,
            DdbExpressionVisitor visitor)
        {
            // "#a BETWEEN :v1 AND :v2"
            
            visitor.Visit<TEntity>(Expression);
            
            builder.Append('#');
            builder.Append(visitor.GetEncodedExpressionName());
            builder.Append(" BETWEEN :v");
            builder.Append(valuesCount++);
            builder.Append(" AND :v");
            builder.Append(valuesCount++);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);
            var converter = GetPropertyConverter<TProperty>(visitor);
            
            builder.Append(":v");
            builder.Append(valuesCount++);
            writer.JsonWriter.WritePropertyName(builder.GetBuffer());
            converter.Write(in writer, ref _min);
            
            builder.Clear();
            builder.Append(":v");
            builder.Append(valuesCount++);
            writer.JsonWriter.WritePropertyName(builder.GetBuffer());
            converter.Write(in writer, ref _max);
        }
    }
}