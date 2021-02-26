using System;
using System.Diagnostics;
using System.Reflection;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.DocumentModel.Converters
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

        public override Type? ElementType => throw new NotSupportedException("Should never be called.");
    }
}