using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Capacity;
using EfficientDynamoDb.Internal.Converters.Json;

namespace EfficientDynamoDb.Context.Operations.Query
{
    public abstract class IterableResponse<T>
    {
        /// <summary>
        /// The primary key of the item where the operation stopped, inclusive of the previous result set. Use this value to start a new operation, excluding this value in the new request.<br/><br/>
        /// If <see cref="LastEvaluatedKey"/> is null, then the "last page" of results has been processed and there is no more data to be retrieved.<br/><br/>
        /// If <see cref="LastEvaluatedKey"/> is not null, it does not necessarily mean that there is more data in the result set. The only way to know when you have reached the end of the result set is when <see cref="LastEvaluatedKey"/> is null.
        /// </summary>
        public IReadOnlyDictionary<string, AttributeValue>? LastEvaluatedKey { get; set; }
        
        /// <summary>
        /// The capacity units consumed by the Query operation. The data returned includes the total provisioned throughput consumed, along with statistics for the table and any indexes involved in the operation. <see cref="ConsumedCapacity"/> is only returned if the <see cref="QueryRequest.ReturnConsumedCapacity"/> parameter was specified.
        /// </summary>
        [DynamoDBProperty("ConsumedCapacity", typeof(JsonObjectDdbConverter<FullConsumedCapacity>))]
        public FullConsumedCapacity? ConsumedCapacity { get; set; }
        
        /// <summary>
        /// The number of items in the response.<br/><br/>
        /// If you used a <see cref="QueryRequest.FilterExpression"/> in the request, then <see cref="Count"/> is the number of items returned after the filter was applied, and <see cref="ScannedCount"/> is the number of matching items before the filter was applied.<br/><br/>
        ///If you did not use a filter in the request, then <see cref="Count"/> and <see cref="ScannedCount"/> are the same.
        /// </summary>
        [DynamoDBProperty("Count", typeof(JsonIntSizeHintDdbConverter))]
        public int Count { get; set; }
        
        /// <summary>
        /// The number of items evaluated, before any <see cref="QueryRequest.FilterExpression"/> is applied. A high <see cref="ScannedCount"/> value with few, or no, <see cref="Count"/> results indicates an inefficient Query operation.<br/><br/>
        /// If you did not use a filter in the request, then <see cref="ScannedCount"/> is the same as <see cref="Count"/>.
        /// </summary>
        [DynamoDBProperty("ScannedCount")]
        public int ScannedCount { get; set; }

        /// <summary>
        /// An array of item attributes that match the query criteria. Each element in this array consists of an attribute name and the value for that attribute.
        /// </summary>
        [DynamoDBProperty("Items", typeof(JsonArrayDdbConverter<>))]
        public T[] Items { get; set; } = null!;
    }
}