using System;
using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;

namespace EfficientDynamoDb.Internal.Converters.Json
{
    internal sealed class JsonListDdbConverter<TValue> : JsonCollectionDdbConverter<List<TValue>, List<TValue>, TValue>
    {
        public JsonListDdbConverter(DdbConverter<TValue> elementConverter) : base(elementConverter)
        {
        }

        public override List<TValue> Read(in AttributeValue attributeValue)
        {
            throw new NotSupportedException("Should never be called.");
        }

        public override AttributeValue Write(ref List<TValue> value)
        {
            throw new NotSupportedException("Should never be called.");
        }

        protected override void Add(List<TValue> collection, TValue item, int index) => collection.Add(item);

        protected override List<TValue> ToResult(List<TValue> collection) => collection;
    }
}