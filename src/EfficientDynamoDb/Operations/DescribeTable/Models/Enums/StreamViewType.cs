namespace EfficientDynamoDb.Operations.DescribeTable.Models.Enums
{
    /// <summary>
    /// Indicates the format of the records within the stream.
    /// </summary>
    public enum StreamViewType
    {
        Undefined = 0,
        /// <summary>
        /// Only the key attributes of items that were modified in the DynamoDB table.
        /// </summary>
        KeysOnly = 10,
        /// <summary>
        /// Entire items from the table, as they appeared after they were modified.
        /// </summary>
        NewImage = 20,
        /// <summary>
        /// Entire items from the table, as they appeared before they were modified.
        /// </summary>
        OldImage = 30,
        /// <summary>
        /// Both the new and the old images of the items from the table.
        /// </summary>
        NewAndOldImages = 40
    }
}