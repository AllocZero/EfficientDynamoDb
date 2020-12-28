using System;
using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.Internal.Converters.Collections.NumberSet
{
    internal abstract class NumberSetDdbConverter<T> : DdbConverter<HashSet<T>> where T : struct
    {
        private static readonly Type ElementTypeValue = typeof(T);
        
        internal sealed override DdbClassType ClassType => DdbClassType.Enumerable;

        public sealed override Type? ElementType => ElementTypeValue;

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

        public override HashSet<T> Read(ref Utf8JsonReader reader, AttributeType attributeType)
        {
            throw new NotSupportedException("Should never be called.");
        }
    }
}