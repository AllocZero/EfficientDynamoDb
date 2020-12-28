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

        public override int Read(ref Utf8JsonReader reader, AttributeType attributeType)
        {
            throw new NotSupportedException("Should never be called.");
        }

        internal override bool TryRead(ref Utf8JsonReader reader, ref DdbEntityReadStack state, out int value)
        {
            value = reader.GetInt32();

            state.GetCurrent().BufferLengthHint = value;

            return true;
        }
    }
}