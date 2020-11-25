using System.Collections.Generic;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Context.Operations.Scan;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.Shared
{
    public abstract class IterableRequest : TableRequest
    {
        /// <summary>
        /// The name of an index to query. This index can be any local secondary index or global secondary index on the table. Note that if you use the <see cref="IndexName"/> parameter, you must also provide <see cref="TableName"/>.
        /// </summary>
        public string? IndexName { get; set; }
        
        /// <summary>
        /// Determines the read consistency model: If set to true, then the operation uses strongly consistent reads; otherwise, the operation uses eventually consistent reads. <br/><br/>
        /// Strongly consistent reads are not supported on global secondary indexes. If you query a global secondary index with <see cref="ConsistentRead"/> set to true, you will receive a validation exception.
        /// </summary>
        public bool ConsistentRead { get; set; }
        
        /// <summary>
        /// The primary key of the first item that this operation will evaluate. Use the value that was returned for <see cref="IterableResponse.LastEvaluatedKey"/> in the previous operation.<br/><br/>
        /// In a parallel scan, a Scan request that includes ExclusiveStartKey must specify the same segment whose previous Scan returned the corresponding value of <see cref="IterableResponse.LastEvaluatedKey"/>.
        /// </summary>
        public IReadOnlyDictionary<string, AttributeValue>? ExclusiveStartKey { get; set; }
        
        /// <summary>
        /// The maximum number of items to evaluate (not necessarily the number of matching items). If DynamoDB processes the number of items up to the limit while processing the results, it stops the operation and returns the matching values up to that point, and a key in <see cref="IterableResponse.LastEvaluatedKey"/> to apply in a subsequent operation, so that you can pick up where you left off. Also, if the processed dataset size exceeds 1 MB before DynamoDB reaches this limit, it stops the operation and returns the matching values up to the limit, and a key in <see cref="IterableResponse.LastEvaluatedKey"/>to apply in a subsequent operation to continue the operation.
        /// </summary>
        public int? Limit { get; set; }
        
        /// <summary>
        /// A collection of strings that identifies one or more attributes to retrieve from the table. These attributes can include scalars, sets, or elements of a JSON document. <br/><br/>
        /// If no attribute names are specified, then all attributes will be returned. If any of the requested attributes are not found, they will not appear in the result. 
        /// </summary>
        public IReadOnlyList<string>? ProjectionExpression { get; set; }

        /// <summary>
        /// Determines the level of detail about provisioned throughput consumption that is returned in the response.
        /// </summary>
        public ReturnConsumedCapacity ReturnConsumedCapacity { get; set; }

        /// <summary>
        /// The attributes to be returned in the result. You can retrieve all item attributes, specific item attributes, the count of matching items, or in the case of an index, some or all of the attributes projected into the index.
        /// </summary>
        public Select? Select { get; set; }
        
        /// <summary>
        /// One or more substitution tokens for attribute names in an expression. The following are some use cases for using ExpressionAttributeNames:
        /// <list type="bullet">
        /// <listheader>
        /// <description> Valid comparisons for the sort key condition are as follows:</description>
        /// </listheader>
        /// <item>
        /// <term>To access an attribute whose name conflicts with a DynamoDB reserved word.</term>
        /// </item>
        /// <item>
        /// <term>To create a placeholder for repeating occurrences of an attribute name in an expression.</term>
        /// </item>
        /// <item>
        /// <term>To prevent special characters in an attribute name from being misinterpreted in an expression.</term>
        /// </item>
        /// </list>
        /// Use the <c>#</c> character in a <see cref="QueryRequest.KeyConditionExpression"/> or query <see cref="QueryRequest.FilterExpression"/> or scan <see cref="ScanRequest.FilterExpression"/> to dereference an attribute name.<br/>
        /// <example> If your <see cref="QueryRequest.KeyConditionExpression"/> is equal to <c>#pk = :pk</c>, then map <c>#pk</c> to the real attribute name in the <see cref="ExpressionAttributeNames"/> dictionary
        /// <code>AttributeNames = new Dictionary&lt;string, string>
        /// {
        ///     ["#pk"] = "pk"
        /// }
        /// </code>
        /// </example>
        /// </summary>
        public IReadOnlyDictionary<string, string>? ExpressionAttributeNames { get; set; }
        
        /// <summary>
        /// One or more values that can be substituted in an expression. Use the <c>:</c> (colon) character in an expression to dereference an attribute value.<br/>
        /// <example>If your <see cref="QueryRequest.KeyConditionExpression"/> is equal to <c>#pk = :pk</c>, then map <c>:pk</c> to the real attribute value in the <see cref="ExpressionAttributeValues"/> dictionary:
        /// <code>AttributeValues = new Dictionary&lt;string, AttributeValue>
        /// {
        ///     [":pk"] = pkValue
        /// }
        /// </code>
        /// </example>
        /// </summary>
        public IReadOnlyDictionary<string, AttributeValue>? ExpressionAttributeValues { get; set; }
    }
}