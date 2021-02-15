using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal sealed class FilterContains<TEntity, TProperty> : FilterBase<TEntity>
    {
        private readonly bool _useSize;
        private TProperty _value;

        public FilterContains(Expression expression, bool useSize, TProperty value) : base(expression)
        {
            _useSize = useSize;
            _value = value;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "contains (#a, :v0)"
            
            visitor.Visit<TEntity>(Expression);

            builder.Append("contains (");
            WriteEncodedExpressionName(visitor.Builder, _useSize, ref builder);
            builder.Append(",:v");
            builder.Append(valuesCount++);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

            builder.Append(":v");
            builder.Append(valuesCount++);
            
            writer.JsonWriter.WritePropertyName(builder.GetBuffer());
            
            visitor.Visit<TEntity>(Expression);
            GetPropertyConverter<TProperty>(visitor).Write(in writer, ref _value);
        }
    }
    
    internal sealed class FilterContains<TEntity> : FilterBase<TEntity>
    {
        private readonly bool _useSize;
        private readonly Expression _valueExpression;
        private readonly bool _useValueSize;

        public FilterContains(Expression expression, bool useSize, Expression valueExpression, bool useValueSize) : base(expression)
        {
            _useSize = useSize;
            _valueExpression = valueExpression;
            _useValueSize = useValueSize;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "contains (#a,#b)"
            
            visitor.Visit<TEntity>(Expression);
            
            builder.Append("contains (");
            WriteEncodedExpressionName(visitor.Builder, _useSize, ref builder);
            builder.Append(",");

            visitor.Visit<TEntity>(_valueExpression);
            WriteEncodedExpressionName(visitor.Builder, _useValueSize, ref builder);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // Do nothing
        }
    }
}