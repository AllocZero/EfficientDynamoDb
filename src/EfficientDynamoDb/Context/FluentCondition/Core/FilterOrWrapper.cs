using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Core
{
    public class FilterOrWrapper : FilterBase
    {
        private readonly FilterBase[] _filters;

        public FilterOrWrapper(params FilterBase[] filters) => _filters = filters;
        
        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, HashSet<string> cachedNames, ref int valuesCount)
        {
            for (var i = 0; i < _filters.Length; i++)
            {
                if (i != 0)
                    builder.Append("OR");
                
                builder.Append('(');
                _filters[i].WriteExpressionStatement(ref builder, cachedNames, ref valuesCount);
                builder.Append(')');
            }
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount)
        {
            foreach (var filter in _filters)
            {
                filter.WriteAttributeValues(writer, metadata, ref valuesCount);
            }
        }
    }
}