namespace EfficientDynamoDb.Operations
{
    /// <summary>
    /// The type of data modification that was performed on the DynamoDB table.
    /// </summary>
    public enum EventName
    {
        Undefined = 0,
        /// <summary>
        /// A new item was added to the table.
        /// </summary>
        Insert = 10,
        /// <summary>
        /// One or more of an existing item's attributes were modified.
        /// </summary>
        Modify = 20,
        /// <summary>
        /// The item was deleted from the table.
        /// </summary>
        Remove = 30
    }
}