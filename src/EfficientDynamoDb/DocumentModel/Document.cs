using System;
using System.Collections.Generic;
using System.Linq;

namespace EfficientDynamoDb.DocumentModel
{
    public class Document : Dictionary<string, AttributeValue>
    {
        public Document() : base(StringComparer.Ordinal)
        {
        }

        public Document(int capacity) : base(capacity, StringComparer.Ordinal)
        {
        }
        
        public override string ToString() => string.Join(", ", this.Select(x => $"{x.Key}: {x.Value.ToString()}"));
    }
}