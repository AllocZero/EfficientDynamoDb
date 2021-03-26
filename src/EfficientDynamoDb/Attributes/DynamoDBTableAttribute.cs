using System;

namespace EfficientDynamoDb.Attributes
{
    public class DynamoDBTableAttribute : Attribute
    {
        public string TableName { get; }

        public DynamoDBTableAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}