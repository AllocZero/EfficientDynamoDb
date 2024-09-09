using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Converters;
using EfficientDynamoDb.Internal.Converters.Json;
using EfficientDynamoDb.Operations.Shared.Capacity;
using EfficientDynamoDb.Operations.TransactGetItems;
using System.Collections.Generic;

namespace EfficientDynamoDb.Operations.ExecuteStatement
{
    public class ExecuteStatementResponse
    {
        /// <summary>
        /// Gets and sets the property Items. 
        /// <para>
        /// If a read operation was used, this property will contain the result of the read operation;
        /// a map of attribute names and their values. For the write operations this value will
        /// be empty.
        /// </para>
        /// </summary>
        public IReadOnlyList<Document>? Items { get; set; }

        /// <summary>
        /// Gets and sets the property LastEvaluatedKey. 
        /// <para>
        /// The primary key of the item where the operation stopped, inclusive of the previous
        /// result set. Use this value to start a new operation, excluding this value in the new
        /// request. If <c>LastEvaluatedKey</c> is empty, then the "last page" of results has
        /// been processed and there is no more data to be retrieved. If <c>LastEvaluatedKey</c>
        /// is not empty, it does not necessarily mean that there is more data in the result set.
        /// The only way to know when you have reached the end of the result set is when <c>LastEvaluatedKey</c>
        /// is empty. 
        /// </para>
        /// </summary>
        public IReadOnlyDictionary<string, AttributeValue>? LastEvaluatedKey { get; set; }

        /// <summary>
        /// Gets and sets the property NextToken. 
        /// <para>
        /// If the response of a read request exceeds the response payload limit DynamoDB will
        /// set this value in the response. If set, you can use that this value in the subsequent
        /// request to get the remaining results.
        /// </para>
        /// </summary>
        public string NextToken { get; set; } = string.Empty;

        /// <summary>
        /// The capacity units consumed by the entire <c>ExecuteStatement</c> operation. The values of the list are ordered according to the ordering of the <see cref="TransactWriteItemsRequest.TransactItems"/> request parameter.
        /// </summary>
        public FullConsumedCapacity? ConsumedCapacity { get; set; }
    }

    public class ExecuteStatementEntityResponse<TEntity>
    {
        /// <summary>
        /// An array of item attributes that match the query criteria. Each element in this array consists of an attribute name and the value for that attribute.
        /// </summary>
        [DynamoDbProperty("Items", typeof(JsonIReadOnlyListHintDdbConverter<>))]
        public IReadOnlyList<TEntity> Items { get; set; } = null!;

        /// <summary>
        /// The primary key of the item where the operation stopped, inclusive of the previous result set. Use this value to start a new operation, excluding this value in the new request.<br/><br/>
        /// If <see cref="LastEvaluatedKey"/> is null, then the "last page" of results has been processed and there is no more data to be retrieved.<br/><br/>
        /// If <see cref="LastEvaluatedKey"/> is not null, it does not necessarily mean that there is more data in the result set. The only way to know when you have reached the end of the result set is when <see cref="LastEvaluatedKey"/> is null.
        /// </summary>
        [DynamoDbProperty("LastEvaluatedKey", typeof(PaginationDdbConverter))]
        public string? LastEvaluatedKey { get; set; }

        /// <summary>
        /// Gets and sets the property NextToken. 
        /// <para>
        /// If the response of a read request exceeds the response payload limit DynamoDB will
        /// set this value in the response. If set, you can use that this value in the subsequent
        /// request to get the remaining results.
        /// </para>
        /// </summary>
        [DynamoDbProperty("NextToken")]
        public string NextToken { get; set; } = string.Empty;

        /// <summary>
        /// The capacity units consumed by the Query operation. The data returned includes the total provisioned throughput consumed, along with statistics for the table and any indexes involved in the operation. <see cref="ConsumedCapacity"/> is only returned if the <see cref="QueryRequest.ReturnConsumedCapacity"/> parameter was specified.
        /// </summary>
        [DynamoDbProperty("ConsumedCapacity", typeof(JsonObjectDdbConverter<FullConsumedCapacity>))]
        public FullConsumedCapacity? ConsumedCapacity { get; set; }
    }
}
