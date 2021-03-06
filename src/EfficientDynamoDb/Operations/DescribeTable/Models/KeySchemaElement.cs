using EfficientDynamoDb.Operations.DescribeTable.Models.Enums;

namespace EfficientDynamoDb.Operations.DescribeTable.Models
{
    public class KeySchemaElement
    {
        public string AttributeName { get; }

        public KeyType KeyType { get; }

        public KeySchemaElement(string attributeName, KeyType keyType)
        {
            AttributeName = attributeName;
            KeyType = keyType;
        }
    }
}