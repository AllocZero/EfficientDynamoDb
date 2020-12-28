using System;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Json
{
    internal class JsonIntDdbConverter : DdbConverter<int>
    {
        public override int Read(in AttributeValue attributeValue)
        {
            throw new NotSupportedException("Should never be called.");
        }

        public override AttributeValue Write(ref int value)
        {
            throw new NotSupportedException("Should never be called.");
        }

        public override int Read(ref DdbReader reader) => reader.JsonReaderValue.GetInt32();
    }
}