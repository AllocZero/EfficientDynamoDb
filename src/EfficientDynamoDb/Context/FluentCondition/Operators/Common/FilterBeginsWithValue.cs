using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal sealed class FilterBeginsWithValue<TEntity> : FilterBase<TEntity>
    {
        private readonly bool _useSize;
        private readonly string _prefix;

        public FilterBeginsWithValue(Expression expression, bool useSize, string prefix) : base(expression)
        {
            _useSize = useSize;
            _prefix = prefix;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // begins_with(#a,:v0)
            
            visitor.Visit<TEntity>(Expression);
            
            builder.Append("begins_with(");
            WriteEncodedExpressionName(visitor.Builder, _useSize, ref builder);
            builder.Append(",:v");
            
            builder.Append(valuesCount++);
            builder.Append(')');
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

            builder.Append(":v");
            builder.Append(valuesCount++);

            writer.JsonWriter.WriteString(builder.GetBuffer(), _prefix);
        }
    }
    
    internal sealed class FilterBeginsWithAttribute<TEntity> : FilterBase<TEntity>
    {
        private readonly bool _useSize;
        private readonly Expression _prefixExpression;
        private readonly bool _usePrefixSize;

        public FilterBeginsWithAttribute(Expression expression, bool useSize, Expression prefixExpression, bool usePrefixSize) : base(expression)
        {
            _useSize = useSize;
            _prefixExpression = prefixExpression;
            _usePrefixSize = usePrefixSize;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // begins_with(#a,#b)
            
            visitor.Visit<TEntity>(Expression);
            
            builder.Append("begins_with(");
            WriteEncodedExpressionName(visitor.Builder, _useSize, ref builder);
            builder.Append(",");
            
            visitor.Visit<TEntity>(_prefixExpression);
            WriteEncodedExpressionName(visitor.Builder, _usePrefixSize, ref builder);
            builder.Append(')');
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // Do nothing
        }
    }
}