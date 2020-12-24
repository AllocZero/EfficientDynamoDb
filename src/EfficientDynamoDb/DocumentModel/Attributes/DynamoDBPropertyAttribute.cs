using System;

namespace EfficientDynamoDb.DocumentModel.Attributes
{
    public sealed class DynamoDBPropertyAttribute : Attribute
    {
        public string Name { get; }

        public Type? DdbConverterType { get; }
        
        public DynamoDbAttributeType AttributeType { get; }

        public DynamoDBPropertyAttribute(string name) : this(name, null, DynamoDbAttributeType.Regular)
        {
        }

        public DynamoDBPropertyAttribute(string name, Type? ddbConverterType) : this(name, ddbConverterType, DynamoDbAttributeType.Regular)
        {
        }

        public DynamoDBPropertyAttribute(string name, DynamoDbAttributeType attributeType) : this(name, null, DynamoDbAttributeType.Regular)
        {
        }

        public DynamoDBPropertyAttribute(string name, Type? ddbConverterType, DynamoDbAttributeType attributeType)
        {
            Name = name;
            DdbConverterType = ddbConverterType;
            AttributeType = attributeType;
        }
    }
}