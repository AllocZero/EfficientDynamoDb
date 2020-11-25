using System.Collections.Generic;
using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.PutItem
{
    public class PutItemRequest : TableRequest
    {
        /// <summary>
        /// A map of attribute name/value pairs, one for each attribute. Only the primary key attributes are required; you can optionally provide other attribute name-value pairs for the item. <br/><br/>
        /// You must provide all of the attributes for the primary key. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide both values for both the partition key and the sort key. <br/><br/>
        /// If you specify any attributes that are part of an index key, then the data types for those attributes must match those of the schema in the table's attribute definition. <br/><br/>
        /// Empty String and Binary attribute values are allowed. Attribute values of type String and Binary must have a length greater than zero if the attribute is used as a key attribute for a table or index.
        /// </summary>
        public Document? Item { get; set; }

        /// <summary>
        /// A condition that must be satisfied in order for a conditional <c>PutItem</c> operation to succeed. <br/><br/>
        /// <list type="bullet">
        /// <listheader>
        /// <description>An expression can contain any of the following:</description>
        /// </listheader>
        /// <item>
        /// <description>
        /// Functions:<c> attribute_exists | attribute_not_exists | attribute_type | contains | begins_with | size</c> <br/>
        /// These function names are case-sensitive.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Comparison operators:<c> = | &lt;&gt; | &lt; | &gt; | &lt;= | &gt;= | BETWEEN | IN</c> <br/>
        /// These function names are case-sensitive.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Logical operators:<c> AND | OR | NOT </c>
        /// </description>
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
        /// Use the <c>#</c> character in a <see cref="ConditionExpression"/> to dereference an attribute name.<br/>
        /// <example> If your <see cref="ConditionExpression"/> is equal to <c>#pk = :pk</c>, then map <c>#pk</c> to the real attribute name in the <see cref="ExpressionAttributeNames"/> dictionary
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
        /// <example>If your <see cref="ConditionExpression"/> is equal to <c>#pk = :pk</c>, then map <c>:pk</c> to the real attribute value in the <see cref="ExpressionAttributeValues"/> dictionary:
        /// <code>AttributeValues = new Dictionary&lt;string, AttributeValue>
        /// {
        ///     [":pk"] = pkValue
        /// }
        /// </code>
        /// </example>
        /// </summary>
        public IReadOnlyDictionary<string, AttributeValue>? ExpressionAttributeValues { get; set; }

        /// <summary>
        /// Determines the level of detail about provisioned throughput consumption that is returned in the response. <br/><br/>
        /// </summary>
        public ReturnConsumedCapacity ReturnConsumedCapacity { get; set; }

        /// <summary>
        /// Determines whether item collection metrics are returned.
        /// </summary>
        public ReturnItemCollectionMetrics ReturnItemCollectionMetrics { get; set; }

        /// <summary>
        /// Use <c>ReturnValues</c> if you want to get the item attributes as they appeared before they were updated with the <c>PutItem</c> request.
        /// </summary>
        /// <remarks>
        /// The <c>ReturnValues</c> parameter is used by several DynamoDB operations; however, <c>PutItem</c> does not recognize any values other than <c>NONE</c> or <c>ALL_OLD</c>.
        /// <br/><br/>
        /// There is no additional cost associated with requesting a return value aside from the small network and processing overhead of receiving a larger response. No read capacity units are consumed.
        /// </remarks>
        public ReturnValues ReturnValues { get; set; }
    }
}