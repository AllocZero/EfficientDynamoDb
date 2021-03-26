using System;

namespace EfficientDynamoDb.Attributes
{
    public class DynamoDbConverterAttribute : Attribute
    {
        public Type ConverterType { get; }

        public DynamoDbConverterAttribute(Type converterType)
        {
            ConverterType = converterType;
        }
    }
}