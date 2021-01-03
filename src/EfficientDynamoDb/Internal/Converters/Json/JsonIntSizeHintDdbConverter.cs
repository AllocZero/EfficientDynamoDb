using System;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Json
{
    internal sealed class JsonIntSizeHintDdbConverter : DdbConverter<int>
    {
        public override int Read(in AttributeValue attributeValue)
        {
            throw new NotSupportedException("Should never be called.");
        }

        public override AttributeValue Write(ref int value)
        {
            throw new NotSupportedException("Should never be called.");
        }

        public override int Read(ref DdbReader reader)
        {
            var value = reader.JsonReaderValue.GetInt32();

            reader.State.GetCurrent().BufferLengthHint = value;
            
            if(GlobalDynamoDbConfig.InternAttributeNames && value > 1)
                reader.State.KeysCache = new KeysCache(DdbReadStack.DefaultKeysCacheSize, DdbReadStack.MaxKeysCacheSize);

            return value;
        }
    }
}