using System;
using System.Diagnostics;
using System.Reflection;
using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.Converters
{
    public abstract class DdbConverterFactory : DdbConverter
    {
        public abstract DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata);

        internal sealed override DdbPropertyInfo CreateDdbPropertyInfo(PropertyInfo propertyInfo, string attributeName, DynamoDbAttributeType attributeType, DynamoDbContextMetadata dynamoDbContextMetadata)
        {
            Debug.Fail("We should never get here.");

            throw new InvalidOperationException();
        }

        internal override DdbClassType ClassType => throw new NotSupportedException("Should never be called.");

        internal override Type? ElementType => throw new NotSupportedException("Should never be called.");

        internal override Type Type => throw new NotSupportedException("Should never be called.");

        internal override void Write(in DdbWriter writer, object value) => throw new NotSupportedException("Should never be called.");
    }
}