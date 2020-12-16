using System.Collections.Generic;
using System.Globalization;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Collections.NumberSet
{
    internal abstract class NumberSetDdbConverter<T> : DdbConverter<HashSet<T>>
    {
        protected abstract T ParseValue(string value);
        
        public override HashSet<T> Read(in AttributeValue attributeValue)
        {
            var values = attributeValue.AsNumberSetAttribute().Items;
            var set = new HashSet<T>(values.Length);

            foreach (var value in values)
                set.Add(ParseValue(value));

            return set;
        }
    }
}