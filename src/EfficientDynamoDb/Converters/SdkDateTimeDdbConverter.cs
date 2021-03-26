using System;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Converters
{
    public sealed class SdkDateTimeDdbConverter : DateTimeDdbConverter
    {
        public SdkDateTimeDdbConverter() : base("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK")
        {
        }

        public override AttributeValue Write(ref DateTime value)
        {
            var utcValue = value.ToUniversalTime();
            return base.Write(ref utcValue);
        }

        public override void Write(in DdbWriter writer, ref DateTime value)
        {
            var utcValue = value.ToUniversalTime();
            base.Write(in writer, ref utcValue);
        }

        public override void WritePropertyName(in DdbWriter writer, ref DateTime value)
        {
            var utcValue = value.ToUniversalTime();
            base.WritePropertyName(in writer, ref utcValue);
        }

        public override string WriteStringValue(ref DateTime value)
        {
            var utcValue = value.ToUniversalTime();
            return base.WriteStringValue(ref utcValue);
        }

        public override void WriteStringValue(in DdbWriter writer, ref DateTime value)
        {
            var utcValue = value.ToUniversalTime();
            base.WriteStringValue(in writer, ref utcValue);
        }
    }
}