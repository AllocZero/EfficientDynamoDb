using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.DocumentModel
{
    public class Document : Dictionary<string, AttributeValue>
    {
        public Document()
        {
        }

        public Document(int capacity) : base(capacity)
        {
        }
    }
}