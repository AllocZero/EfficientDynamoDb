namespace EfficientDynamoDb.Internal.Builder.GetItemHttpContents
{
    public abstract class GetItemHttpContentBase : DynamoDbHttpContent
    {
        protected readonly string TableName;

        protected GetItemHttpContentBase(string tableName) : base("DynamoDB_20120810.GetItem") => TableName = tableName;
    }
}