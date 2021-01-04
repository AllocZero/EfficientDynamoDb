using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Core
{
    public class FilterAndWrapper : IFilter
    {
        private readonly IFilter[] _filters;

        public FilterAndWrapper(params IFilter[] filters) => _filters = filters;

        void IFilter.WriteExpressionStatement(ref NoAllocStringBuilder builder, HashSet<string> cachedNames, ref int valuesCount)
        {
            for (var i = 0; i < _filters.Length; i++)
            {
                if (i != 0)
                    builder.Append("AND");
                
                builder.Append('(');
                _filters[i].WriteExpressionStatement(ref builder, cachedNames, ref valuesCount);
                builder.Append(')');
            }
        }
        
        void IFilter.WriteAttributeValues(Utf8JsonWriter writer, ref int valuesCount)
        {
            foreach (var filter in _filters)
            {
                filter.WriteAttributeValues(writer, ref valuesCount);
            }
        }
    }
}