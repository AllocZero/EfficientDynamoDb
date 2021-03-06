using System.Collections.Generic;
using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Converters;
using EfficientDynamoDb.Internal.Converters.Json;
using EfficientDynamoDb.Internal.Converters.Primitives.Numbers;
using EfficientDynamoDb.Operations.Shared.Capacity;

namespace EfficientDynamoDb.Operations.Query
{
    public abstract class IterableResponse
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
        public FullConsumedCapacity? ConsumedCapacity { get; set; }
        
        /// <summary>
        /// The number of items in the response.<br/><br/>
        /// If you used a <see cref="QueryRequest.FilterExpression"/> in the request, then <see cref="Count"/> is the number of items returned after the filter was applied, and <see cref="ScannedCount"/> is the number of matching items before the filter was applied.<br/><br/>
        ///If you did not use a filter in the request, then <see cref="Count"/> and <see cref="ScannedCount"/> are the same.
        /// </summary>
        public int Count { get; set; }
        
        /// <summary>
        /// The number of items evaluated, before any <see cref="QueryRequest.FilterExpression"/> is applied. A high <see cref="ScannedCount"/> value with few, or no, <see cref="Count"/> results indicates an inefficient Query operation.<br/><br/>
        /// If you did not use a filter in the request, then <see cref="ScannedCount"/> is the same as <see cref="Count"/>.
        /// </summary>
        public int ScannedCount { get; set; }

        /// <summary>
        /// An array of item attributes that match the query criteria. Each element in this array consists of an attribute name and the value for that attribute.
        /// </summary>
        public IReadOnlyList<Document> Items { get; set; } = null!;
    }
    
    public abstract class IterableEntityResponse<TEntity> where TEntity : class
    {
        /// <summary>
        /// The primary key of the item where the operation stopped, inclusive of the previous result set. Use this value to start a new operation, excluding this value in the new request.<br/><br/>
        /// If <see cref="LastEvaluatedKey"/> is null, then the "last page" of results has been processed and there is no more data to be retrieved.<br/><br/>
        /// If <see cref="LastEvaluatedKey"/> is not null, it does not necessarily mean that there is more data in the result set. The only way to know when you have reached the end of the result set is when <see cref="LastEvaluatedKey"/> is null.
        /// </summary>
        [DynamoDbProperty("LastEvaluatedKey", typeof(PaginationDdbConverter))]
        public string? LastEvaluatedKey { get; set; }
        
        /// <summary>
        /// The capacity units consumed by the Query operation. The data returned includes the total provisioned throughput consumed, along with statistics for the table and any indexes involved in the operation. <see cref="ConsumedCapacity"/> is only returned if the <see cref="QueryRequest.ReturnConsumedCapacity"/> parameter was specified.
        /// </summary>
        [DynamoDbProperty("ConsumedCapacity", typeof(JsonObjectDdbConverter<FullConsumedCapacity>))]
        public FullConsumedCapacity? ConsumedCapacity { get; set; }
        
        /// <summary>
        /// The number of items in the response.<br/><br/>
        /// If you used a <see cref="QueryRequest.FilterExpression"/> in the request, then <see cref="Count"/> is the number of items returned after the filter was applied, and <see cref="ScannedCount"/> is the number of matching items before the filter was applied.<br/><br/>
        ///If you did not use a filter in the request, then <see cref="Count"/> and <see cref="ScannedCount"/> are the same.
        /// </summary>
        [DynamoDbProperty("Count", typeof(JsonIntSizeHintDdbConverter))]
        public int Count { get; set; }
        
        /// <summary>
        /// The number of items evaluated, before any <see cref="QueryRequest.FilterExpression"/> is applied. A high <see cref="ScannedCount"/> value with few, or no, <see cref="Count"/> results indicates an inefficient Query operation.<br/><br/>
        /// If you did not use a filter in the request, then <see cref="ScannedCount"/> is the same as <see cref="Count"/>.
        /// </summary>
        [DynamoDbProperty("ScannedCount", typeof(IntDdbConverter))]
        public int ScannedCount { get; set; }

        /// <summary>
        /// An array of item attributes that match the query criteria. Each element in this array consists of an attribute name and the value for that attribute.
        /// </summary>
        [DynamoDbProperty("Items", typeof(JsonIReadOnlyListHintDdbConverter<>))]
        public IReadOnlyList<TEntity> Items { get; set; } = null!;
    }
}