using System;

namespace EfficientDynamoDb.DocumentModel.Attributes
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