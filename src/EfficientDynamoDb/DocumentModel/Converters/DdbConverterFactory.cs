using System;
using System.Diagnostics;
using System.Reflection;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.DocumentModel.Converters
{
    public abstract class DdbConverterFactory : DdbConverter
    {
        public abstract DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata);

        internal sealed override DdbPropertyInfo CreateDdbPropertyInfo(PropertyInfo propertyInfo, string attributeName, DynamoDbContextMetadata dynamoDbContextMetadata)
        {
            Debug.Fail("We should never get here.");

            throw new InvalidOperationException();
        }

        public override DdbClassType ClassType => throw new NotImplementedException();

        public override Type? ElementType => throw new NotImplementedException();
    }
}