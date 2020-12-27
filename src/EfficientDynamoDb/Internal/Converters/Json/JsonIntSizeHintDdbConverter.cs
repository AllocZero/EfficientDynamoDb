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
            throw new NotImplementedException();
        }

        public override AttributeValue Write(ref int value)
        {
            throw new NotImplementedException();
        }

        internal override bool TryRead(ref Utf8JsonReader reader, ref DdbEntityReadStack state, AttributeType attributeType, out int value)
        {
            value = reader.GetInt32();

            state.GetCurrent().BufferLengthHint = value;

            return true;
        }
    }
}