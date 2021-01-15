using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal sealed class FilterContains<TEntity, TProperty> : FilterBase<TEntity>
    {
        private TProperty _value;

        internal FilterContains(string propertyName, TProperty value) : base(propertyName)
        {
            _value = value;
        }
        
        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, HashSet<string> cachedNames, ref int valuesCount)
        {
            // "contains (#a, :v0)"
            
            builder.Append("contains (#");
            builder.Append(PropertyName);
            builder.Append(",:v");
            builder.Append(valuesCount++);

            cachedNames.Add(PropertyName);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

            builder.Append(":v");
            builder.Append(valuesCount++);
            
            writer.JsonWriter.WritePropertyName(builder.GetBuffer());
            GetPropertyConverter<TProperty>(metadata).Write(in writer, ref _value);
        }
    }
}