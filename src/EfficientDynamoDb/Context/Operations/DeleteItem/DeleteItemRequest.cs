using System.Collections.Generic;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Context.Operations.Scan;
using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.DeleteItem
{
    public class DeleteItemRequest : TableRequest
    {
        /// <summary>
        /// A map of attribute names to AttributeValue objects, representing the primary key of the item to delete. <br/><br/>
        /// For the primary key, you must provide all of the attributes. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide values for both the partition key and the sort key.
        /// </summary>
        public PrimaryKey? Key { get; set; }
        
        /// <summary>
        /// A condition that must be satisfied in order for a conditional delete to succeed.<br/><br/>
        /// <list type="bullet">
        /// <listheader>
        /// <description> An expression can contain any of the following:</description>
        /// </listheader>
        /// <item>
        /// <term>Functions: <c>attribute_exists | attribute_not_exists | attribute_type | contains | begins_with | size</c> <br/>These function names are case-sensitive.</term>
        /// </item>
        /// <item>
        /// <term>Comparison operators: <c>= | &lt;> | &lt; | > | &lt;= | >= | BETWEEN | IN</c></term>
        /// </item>
        /// <item>
        /// <term>Logical operators: <c>AND | OR | NOT</c></term>
        /// </item>
        /// </list>
        /// </summary>
        public string? ConditionExpression { get; set; }
        
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
        
        /// <summary>
        /// Determines the level of detail about provisioned throughput consumption that is returned in the response.
        /// </summary>
        public ReturnConsumedCapacity ReturnConsumedCapacity { get; set; }
        
        /// <summary>
        /// Determines whether item collection metrics are returned.
        /// </summary>
        public ReturnItemCollectionMetrics ReturnItemCollectionMetrics { get; set; }
        
        /// <summary>
        /// Use ReturnValues if you want to get the item attributes as they appeared before they were deleted.
        /// <list type="bullet">
        /// <listheader>
        /// <description>For delete, the valid values are:</description>
        /// </listheader>
        /// <item>
        /// <term><see cref="EfficientDynamoDb.DocumentModel.ReturnDataFlags.ReturnValues.None"/> - If <see cref="ReturnValues"/> is not specified, or if its value is <see cref="EfficientDynamoDb.DocumentModel.ReturnDataFlags.ReturnValues.None"/>, then nothing is returned. (This setting is the default for <see cref="ReturnValues"/>.)</term>
        /// </item>
        /// <item>
        /// <term><see cref="DocumentModel.ReturnDataFlags.ReturnValues.AllOld"/> - The content of the old item is returned.</term>
        /// </item>
        /// </list>
        /// </summary>
        public ReturnValues ReturnValues { get; set; }
    }
}