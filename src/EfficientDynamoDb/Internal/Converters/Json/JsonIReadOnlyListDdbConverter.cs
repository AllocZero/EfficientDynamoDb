using System;
using System.Collections.Generic;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Internal.Converters.Json
{
    internal class JsonIReadOnlyListDdbConverter<TValue> : JsonCollectionDdbConverter<IReadOnlyList<TValue>, List<TValue>, TValue>
    {
        public JsonIReadOnlyListDdbConverter(DdbConverter<TValue> elementConverter) : base(elementConverter)
        {
        }

        public override IReadOnlyList<TValue> Read(in AttributeValue attributeValue)
        {
            throw new NotSupportedException("Should never be called.");
        }

        public override AttributeValue Write(ref IReadOnlyList<TValue> value)
        {
            throw new NotSupportedException("Should never be called.");
        }

        protected override void Add(List<TValue> collection, TValue item, int index) => collection.Add(item);

        protected override IReadOnlyList<TValue> ToResult(List<TValue> collection) => collection;
    }
}