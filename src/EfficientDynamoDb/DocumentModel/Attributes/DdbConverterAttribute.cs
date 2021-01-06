using System;

namespace EfficientDynamoDb.DocumentModel.Attributes
{
    public class DdbConverterAttribute : Attribute
    {
        public Type ConverterType { get; }

        public DdbConverterAttribute(Type converterType)
        {
            ConverterType = converterType;
        }
    }
}