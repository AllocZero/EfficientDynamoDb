using EfficientDynamoDb.Converters;
using EfficientDynamoDb.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.FluentCondition.Core
{
    public class FilterAndWrapper : FilterBase
    {
        private readonly FilterBase[] _filters;

        public FilterAndWrapper(params FilterBase[] filters) => _filters = filters;

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount,
            DdbExpressionVisitor visitor)
        {
            for (var i = 0; i < _filters.Length; i++)
            {
                if (i != 0)
                    builder.Append("AND");
                
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