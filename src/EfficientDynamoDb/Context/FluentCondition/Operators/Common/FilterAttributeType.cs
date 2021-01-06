using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal class FilterAttributeType<TEntity> : FilterBase<TEntity>
    {
        private readonly string _propertyName;
        private readonly AttributeType _type;

        internal FilterAttributeType(string propertyName, AttributeType type) : base(propertyName)
        {
            _propertyName = propertyName;
            _type = type;
        }
        
        protected override void WriteExpressionStatementInternal(ref NoAllocStringBuilder builder, HashSet<string> cachedNames, ref int valuesCount)
        {
            builder.Append("attribute_type(#");
            builder.Append(_propertyName);
            builder.Append(",:v");
            
            builder.Append(valuesCount++);
            builder.Append(')');

            cachedNames.Add(_propertyName);
        }

        protected override void WriteAttributeValuesInternal(Utf8JsonWriter writer, ref int valuesCount)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

            builder.Append(":v");
            builder.Append(valuesCount++);

            writer.WriteString(builder.GetBuffer(), _type.ToDdbTypeName());
        }
    }
}