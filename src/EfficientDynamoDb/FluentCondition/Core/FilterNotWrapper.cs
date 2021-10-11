using EfficientDynamoDb.Converters;
using EfficientDynamoDb.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.FluentCondition.Core
{
    public class FilterNotWrapper : FilterBase
    {
        private readonly FilterBase _filter;
    
        public FilterNotWrapper(FilterBase filter) => _filter = filter;
    
        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount,
            DdbExpressionVisitor visitor)
        {
            builder.Append("NOT");
            
            builder.Append('(');
            _filter.WriteExpressionStatement(ref builder, ref valuesCount, visitor);
            builder.Append(')');
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            _filter.WriteAttributeValues(writer, metadata, ref valuesCount, visitor);
        }
    }
}