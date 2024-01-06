namespace EfficientDynamoDb.Operations
{
    public class ListStreamsRequest
    {
        /// <summary>
        /// The ARN (Amazon Resource Name) of the first item that this operation will evaluate.
        /// Use the value that was returned for <see cref="ListStreamsResponse.LastEvaluatedStreamArn"/> in the previous operation.
        /// </summary>
        public string? ExclusiveStartStreamArn { get; set; }

        /// <summary>
        /// The maximum number of streams to return.
        /// Values less or equal to zero are interpreted as no limit.
        /// The upper limit is 100.
        /// </summary>
        public int Limit { get; set; } = -1;
        
        /// <summary>
        /// If this parameter is provided, then only the streams associated with this table name are returned.
        /// </summary>
        public string? TableName { get; set; }
    }
}