namespace EfficientDynamoDb.DocumentModel.ReturnDataFlags
{
    public enum ReturnConsumedCapacity : byte
    {
        /// <summary>
        /// No consumed capacity details are included in the response.
        /// </summary>
        None = 0,
        /// <summary>
        /// The response includes the aggregate consumed capacity for the operation, together with consumed capacity for each table and secondary index that was accessed. <br/><br/>
        /// Note that some operations, such as GetItem and BatchGetItem, do not access any indexes at all. In these cases, specifying <see cref="Indexes"/> will only return consumed capacity information for table(s).
        /// </summary>
        Indexes = 1,
        /// <summary>
        /// The response includes only the aggregate consumed capacity for the operation.
        /// </summary>
        Total = 2
    }
}