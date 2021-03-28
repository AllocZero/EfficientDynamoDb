using System;

namespace EfficientDynamoDb.Attributes
{
    public sealed class DynamoDbTableAttribute : Attribute
    {
        public string TableName { get; }

        public DynamoDbTableAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}