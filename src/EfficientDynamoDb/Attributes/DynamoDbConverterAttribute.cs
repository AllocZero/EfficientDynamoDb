using System;

namespace EfficientDynamoDb.Attributes
{
    public sealed class DynamoDbConverterAttribute : Attribute
    {
        public Type ConverterType { get; }

        public DynamoDbConverterAttribute(Type converterType)
        {
            ConverterType = converterType;
        }
    }
}