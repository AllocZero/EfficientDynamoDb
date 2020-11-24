using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.DocumentModel
{
    public class DdbAttribute
    {
        public string Name { get; }
        
        public AttributeValue Value { get; }

        public DdbAttribute(string name, AttributeValue value)
        {
            Name = name;
            Value = value;
        }
    }
}