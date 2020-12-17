using System;

namespace EfficientDynamoDb.DocumentModel.Attributes
{
    public sealed class DynamoDBPropertyAttribute : Attribute
    {
        public string Name { get; }

        public Type? DdbConverterType { get; }
        
        public DynamoDBPropertyAttribute(string name)
        {
            Name = name;
        }

        public DynamoDBPropertyAttribute(string name, Type? ddbConverterType)
        {
            Name = name;
            DdbConverterType = ddbConverterType;
        }
    }
}