using System;
using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal sealed class StringSetDdbConverter : DdbConverter<HashSet<string>>
    {
        private static readonly Type ElementTypeValue = typeof(string);
        
        internal override DdbClassType ClassType => DdbClassType.Enumerable;

        public override Type? ElementType => ElementTypeValue;
        
        public override HashSet<string> Read(in AttributeValue attributeValue) => attributeValue.AsStringSetAttribute().Items;

        public override AttributeValue Write(ref HashSet<string> value) => new StringSetAttributeValue(value);

        public override HashSet<string> Read(ref DdbReader reader)
        {
            throw new NotSupportedException("Should never be called.");
        }
    }
}