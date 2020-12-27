using System;
using System.Buffers.Text;
using System.Text;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Primitives.Numbers
{
    internal sealed class IntDdbConverter : NumberDdbConverter<int>
    {
        public override int Read(in AttributeValue attributeValue) => attributeValue.ToInt();
        
        public override void Write(Utf8JsonWriter writer, string attributeName, ref int value)
        {
            writer.WritePropertyName(attributeName);
            
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, value);
            writer.WriteEndObject();
        }

        internal override bool TryRead(ref Utf8JsonReader reader, ref DdbEntityReadStack state, AttributeType attributeType, out int value)
        {
            Utf8Parser.TryParse(reader.ValueSpan, out value, out _);
            return true;
        }
    }
}