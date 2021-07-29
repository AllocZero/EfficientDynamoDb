using System;
// ReSharper disable IntroduceOptionalParameters.Global

namespace EfficientDynamoDb.Attributes
{
    public sealed class DynamoDbPropertyAttribute : Attribute
    {
        public string Name { get; }

        public Type? DdbConverterType { get; }
        
        public DynamoDbAttributeType AttributeType { get; }

        public DynamoDbPropertyAttribute(string name) : this(name, null, DynamoDbAttributeType.Regular)
        {
        }

        public DynamoDbPropertyAttribute(string name, Type? ddbConverterType) : this(name, ddbConverterType, DynamoDbAttributeType.Regular)
        {
        }

        public DynamoDbPropertyAttribute(string name, DynamoDbAttributeType attributeType) : this(name, null, attributeType)
        {
        }

        public DynamoDbPropertyAttribute(string name, Type? ddbConverterType, DynamoDbAttributeType attributeType)
        {
            Name = name;
            DdbConverterType = ddbConverterType;
            AttributeType = attributeType;
        }
    }
}