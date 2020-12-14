using System.Collections.Generic;
using System.Globalization;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters
{
    public class NumberSetDdbConverter : DdbConverter<HashSet<int>>
    {
        public override HashSet<int> Read(AttributeValue attributeValue)
        {
            var values = attributeValue.AsNumberSetAttribute().Items;
            var set = new HashSet<int>(values.Length);

            foreach (var value in values)
                set.Add(int.Parse(value, CultureInfo.InvariantCulture));

            return set;
        }
    }
}