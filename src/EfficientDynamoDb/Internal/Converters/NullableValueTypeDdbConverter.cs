using System.Runtime.CompilerServices;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Internal.Converters
{
    internal sealed class NullableValueTypeDdbConverter<T> : DdbConverter<T?> where T : struct
    {
        private readonly DdbConverter<T> _converter;

        public NullableValueTypeDdbConverter(DdbConverter<T> converter) : base(true)
        {
            _converter = converter;
        }

        public override T? Read(in AttributeValue attributeValue)
        {
            return attributeValue.IsNull ? (T?) null : _converter.Read(in attributeValue);
        }

        public override T? Read(ref DdbReader reader) => reader.AttributeType == AttributeType.Null ? (T?)null : _converter.Read(ref reader);

        public override AttributeValue Write(ref T? value)
        {
            if(!value.HasValue)
                return AttributeValue.Null;
            
            var notNullableValue = value!.Value;
            return _converter.Write(ref notNullableValue);
        }

        public override void Write(in DdbWriter writer, ref T? value)
        {
            if (!value.HasValue)
            {
                writer.WriteDdbNull();
                return;
            }
            
            var realValue = value!.Value;
            _converter.Write(in writer, ref realValue);
        }
    }
}