using System;
using System.Collections.Generic;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Context.Operations.Scan
{
    public class ScanRequest : IterableRequest
    {
        /// <summary>
        /// A string that contains conditions that DynamoDB applies after the Scan operation, but before the data is returned to you. Items that do not satisfy the <see cref="FilterExpression"/> criteria are not returned.<br/><br/>
        /// A <see cref="FilterExpression"/> is applied after the items have already been read; the process of filtering does not consume any additional read capacity units.
        /// </summary>
        public string? FilterExpression { get; set; }
        
        /// <summary>
        /// For a parallel Scan request, <see cref="Segment"/> identifies an individual segment to be scanned by an application worker.<br/><br/>
        /// <see cref="Segment"/> IDs are zero-based, so the first segment is always 0. For example, if you want to use four application threads to scan a table or an index, then the first thread specifies a <see cref="Segment"/> value of 0, the second thread specifies 1, and so on.<br/><br/>
        /// The value of <see cref="IterableResponse.LastEvaluatedKey"/> returned from a parallel Scan request must be used as <see cref="IterableRequest.ExclusiveStartKey"/> with the same segment ID in a subsequent Scan operation.<br/><br/>
        /// The value for <see cref="Segment"/> must be greater than or equal to 0, and less than the value provided for TotalSegments.<br/><br/>
        /// If you provide <see cref="Segment"/>, you must also provide TotalSegments.
        /// </summary>
        public int? Segment { get; set; }
        
        /// <summary>
        /// For a parallel Scan request, <see cref="TotalSegments"/> represents the total number of segments into which the Scan operation will be divided. The value of <see cref="TotalSegments"/> corresponds to the number of application workers that will perform the parallel scan. For example, if you want to use four application threads to scan a table or an index, specify a <see cref="TotalSegments"/> value of 4.<br/><br/>
        /// The value for <see cref="TotalSegments"/> must be greater than or equal to 1, and less than or equal to 1000000. If you specify a <see cref="TotalSegments"/> value of 1, the Scan operation will be sequential rather than parallel.<br/><br/>
        /// If you specify <see cref="TotalSegments"/>, you must also specify <see cref="Segment"/>.
        /// </summary>
        public int? TotalSegments { get; set; }
        
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