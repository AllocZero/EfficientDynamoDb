using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.Internal.Converters.Collections.BinarySet
{
    internal sealed class IListBinarySetDdbConverter : BinarySetDdbConverter<IList<byte[]>?, List<byte[]>>
    {
        public override IList<byte[]>? Read(in AttributeValue attributeValue) => attributeValue.IsNull ? null : attributeValue.AsBinarySetAttribute().Items;

        public override AttributeValue Write(ref IList<byte[]>? value)
        {
            return value == null ? AttributeValue.Null : WriteInlined(ref value);
        }
        
        protected override void Add(List<byte[]> collection, byte[] item, int index) => collection.Add(item);

        protected override IList<byte[]> ToResult(List<byte[]> collection) => collection;

        public override void Write(in DdbWriter writer, ref IList<byte[]>? value)
        {
            if (value == null)
            {
                writer.WriteDdbNull();
                return;
            }

            WriteInlined(in writer, ref value);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private AttributeValue WriteInlined(ref IList<byte[]> value)
        {
            if (value is List<byte[]> list)
                return new AttributeValue(new BinarySetAttributeValue(list));

            list = new List<byte[]>(value.Count);

            foreach (var item in value)
                list.Add(item);

            return new AttributeValue(new BinarySetAttributeValue(list));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInlined(in DdbWriter writer, ref IList<byte[]> value)
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