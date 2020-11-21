using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Context.RequestBuilders
{
    public class DdbKey<T> where T: IAttributeValue
    {
        public string AttributeName { get; }
        
        public T Value { get; }

        public DdbKey(string attributeName, T value)
        {
            AttributeName = attributeName;
            Value = value;
        }

        public override string ToString() => $"{AttributeName}: {Value}";
    }
}