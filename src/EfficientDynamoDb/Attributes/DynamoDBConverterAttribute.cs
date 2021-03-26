using System;

namespace EfficientDynamoDb.Attributes
{
    public class DynamoDBConverterAttribute : Attribute
    {
        public Type ConverterType { get; }

        public DynamoDBConverterAttribute(Type converterType)
        {
            ConverterType = converterType;
        }
    }
}