using System;

namespace EfficientDynamoDb.Attributes
{
    public class DynamoDbTableAttribute : Attribute
    {
        public string TableName { get; }

        public DynamoDbTableAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}