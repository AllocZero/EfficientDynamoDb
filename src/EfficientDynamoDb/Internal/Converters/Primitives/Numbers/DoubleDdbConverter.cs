using System.Text.Json;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.Internal.Converters.Primitives.Numbers
{
    internal sealed class DoubleDdbConverter : NumberDdbConverter<double>
    {
        public override double Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToDouble();
        
        public override void Write(Utf8JsonWriter writer, string attributeName, ref double value)
        {
            writer.WritePropertyName(attributeName);
            
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, value);
            writer.WriteEndObject();
        }
    }
}