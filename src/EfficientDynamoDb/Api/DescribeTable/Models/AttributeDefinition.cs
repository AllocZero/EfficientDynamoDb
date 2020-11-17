namespace EfficientDynamoDb.Api.DescribeTable.Models
{
    public class AttributeDefinition
    {
        public string AttributeName { get; }
        
        public string AttributeType { get; }

        public AttributeDefinition(string attributeName, string attributeType)
        {
            AttributeName = attributeName;
            AttributeType = attributeType;
        }
    }
}