using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Core
{
    public interface IFilter
    {
        // TODO: Get rid of hashset
        internal void WriteExpressionStatement(ref NoAllocStringBuilder builder, HashSet<string> cachedNames, ref int valuesCount);

        internal void WriteAttributeValues(Utf8JsonWriter writer, ref int valuesCount);
    }
}