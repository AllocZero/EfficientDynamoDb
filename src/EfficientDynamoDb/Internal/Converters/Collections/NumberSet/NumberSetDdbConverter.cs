using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;

namespace EfficientDynamoDb.Internal.Converters.Collections.NumberSet
{
    internal abstract class NumberSetDdbConverter<T> : DdbConverter<HashSet<T>> where T : struct
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

        public override AttributeValue Write(ref HashSet<T> value)
        {
            var array = new string[value.Count];

            var i = 0;
            foreach (var item in value)
                array[i++] = item.ToString();
            
            return new NumberSetAttributeValue(array);
        }
    }
}