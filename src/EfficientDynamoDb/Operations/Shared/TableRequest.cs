namespace EfficientDynamoDb.Operations.Shared
{
    public abstract class TableRequest
    {
        /// <summary>
        /// The name of the table containing the requested item.
        /// </summary>
        public string TableName { get; set; } = string.Empty;
    }
}