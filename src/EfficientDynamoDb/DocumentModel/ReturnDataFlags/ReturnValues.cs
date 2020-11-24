namespace EfficientDynamoDb.DocumentModel.ReturnDataFlags
{
    public enum ReturnValues : byte
    {
        /// <summary>
        /// No values are returned in the response.
        /// </summary>
        None = 0,
        /// <summary>
        /// Returns all of the attributes of the item, as they appeared before the operation.
        /// </summary>
        AllOld = 1,
        /// <summary>
        /// Returns only the updated attributes, as they appeared before the operation.
        /// </summary>
        UpdatedOld = 2,
        /// <summary>
        /// Returns all of the attributes of the item, as they appear after the operation.
        /// </summary>
        AllNew = 3,
        /// <summary>
        /// Returns only the updated attributes, as they appear after the operation.
        /// </summary>
        UpdatedNew = 4
    }
}