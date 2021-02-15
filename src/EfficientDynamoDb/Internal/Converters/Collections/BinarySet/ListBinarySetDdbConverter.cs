using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.Internal.Converters.Collections.BinarySet
{
    internal sealed class ListBinarySetDdbConverter : BinarySetDdbConverter<List<byte[]>?, List<byte[]>>
    {
        public override List<byte[]>? Read(in AttributeValue attributeValue) => attributeValue.IsNull ? null : attributeValue.AsBinarySetAttribute().Items;

        public override AttributeValue Write(ref List<byte[]>? value)
        {
            return value == null ? AttributeValue.Null : new AttributeValue(new BinarySetAttributeValue(value));
        }
        
        protected override void Add(List<byte[]> collection, byte[] item, int index) => collection.Add(item);

        protected override List<byte[]> ToResult(List<byte[]> collection) => collection;

        public override void Write(in DdbWriter writer, ref List<byte[]>? value)
        {
            if (value == null)
            {
                writer.WriteDdbNull();
                return;
            }

            WriteInlined(in writer, ref value);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInlined(in DdbWriter writer, ref List<byte[]> value)
        {
            writer.JsonWriter.WriteStartObject();
            writer.JsonWriter.WritePropertyName(DdbTypeNames.BinarySet);
            
            writer.JsonWriter.WriteStartArray();

            foreach (var item in value)
                writer.JsonWriter.WriteBase64StringValue(item);

            writer.JsonWriter.WriteEndArray();
            writer.JsonWriter.WriteEndObject();
        }
    }
}