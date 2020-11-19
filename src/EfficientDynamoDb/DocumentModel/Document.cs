using System;
using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel.AttributeValues;

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
    }
}