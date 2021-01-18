using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Core
{
    public class FilterOrWrapper : FilterBase
    {
        private readonly FilterBase[] _filters;

        public FilterOrWrapper(params FilterBase[] filters) => _filters = filters;
        
        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount,
            DdbExpressionVisitor visitor)
        {
            for (var i = 0; i < _filters.Length; i++)
            {
                if (i != 0)
                    builder.Append("OR");
                
                builder.Append('(');
                _filters[i].WriteExpressionStatement(ref builder, ref valuesCount, visitor);
                builder.Append(')');
            }
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            foreach (var filter in _filters)
            {
                filter.WriteAttributeValues(writer, metadata, ref valuesCount, visitor);
            }
        }
    }
}