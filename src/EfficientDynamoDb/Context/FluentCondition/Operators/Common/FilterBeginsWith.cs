using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal class FilterBeginsWith<TEntity> : FilterBase<TEntity>
    {
        private readonly string _propertyName;
        private readonly string _prefix;

        internal FilterBeginsWith(string propertyName, string prefix) : base(propertyName)
        {
            _propertyName = propertyName;
            _prefix = prefix;
        }
        
        protected override void WriteExpressionStatementInternal(ref NoAllocStringBuilder builder, HashSet<string> cachedNames, ref int valuesCount)
        {
            builder.Append("begins_with(#");
            builder.Append(_propertyName);
            builder.Append(",:v");
            
            builder.Append(valuesCount++);
            builder.Append(')');

            cachedNames.Add(_propertyName);
        }

        protected override void WriteAttributeValuesInternal(Utf8JsonWriter writer, ref int valuesCount)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);
            
            builder.Append(":v");
            builder.Append(valuesCount++);

            writer.WriteString(builder.GetBuffer(), _prefix);
        }
    }
}