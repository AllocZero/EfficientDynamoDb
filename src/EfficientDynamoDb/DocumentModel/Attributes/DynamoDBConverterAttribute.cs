using System;

namespace EfficientDynamoDb.DocumentModel.Attributes
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