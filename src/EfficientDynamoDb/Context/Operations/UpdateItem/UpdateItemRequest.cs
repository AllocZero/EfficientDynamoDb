using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.UpdateItem
{
    public class UpdateItemRequest
    {
        /// <summary>
        /// A map of attribute names to AttributeValue objects, representing the primary key of the item to retrieve. <br/><br/>
        /// For the primary key, you must provide all of the attributes. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide values for both the partition key and the sort key.
        /// </summary>
        public PrimaryKey? Key { get; set; }
        
        /// <summary>
        /// The name of the table containing the requested item.
        /// </summary>
        public string TableName { get; set; } = string.Empty;
        
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
        /// Use <c>ReturnValues</c> if you want to get the item attributes as they appeared before they were updated with the <c>UpdateItem</c> request.
        /// </summary>
        /// <remarks>
        /// There is no additional cost associated with requesting a return value aside from the small network and processing overhead of receiving a larger response. No read capacity units are consumed.
        /// <br/><br/>
        /// The values returned are strongly consistent.
        /// </remarks>
        public ReturnValues ReturnValues { get; set; }
        
        /// <summary>
        /// An expression that defines one or more attributes to be updated, the action to be performed on them, and new values for them.
        /// <list type="bullet">
        /// <listheader>
        /// <description> The following action values are available for <c>UpdateExpression</c>.</description>
        /// </listheader>
        /// <item>
        /// <description>
        /// <c>SET</c> - Adds one or more attributes and values to an item. If any of these attributes already exist, they are replaced by the new values. You can also use <c>SET</c> to add or subtract from an attribute that is of type Number. For example: <c>SET myNum = myNum + :val</c>
        /// <br/><br/>
        /// <c>SET</c> supports the following functions:<br/>
        /// &bull; <c>if_not_exists (path, operand)</c> - if the item does not contain an attribute at the specified path, then <c>if_not_exists</c> evaluates to operand; otherwise, it evaluates to path. You can use this function to avoid overwriting an attribute that may already be present in the item.<br/>
        /// &bull; <c>listappend (operand, operand)</c> - evaluates to a list with a new element added to it. You can append the new element to the start or the end of the list by reversing the order of the operands.<br/>
        /// These function names are case-sensitive.
        /// <br/><br/>
        /// </description>
        /// </item>
        /// <item>
        /// <description><c>REMOVE</c> - Removes one or more attributes from an item.<br/><br/></description>
        /// </item>
        /// <item>
        /// <description><c>ADD</c> - Adds the specified value to the item, if the attribute does not already exist. If the attribute does exist, then the behavior of ADD depends on the data type of the attribute.<br/><br/></description>
        /// </item>
        /// <item>
        /// <description>
        /// <c>DELETE</c> - Deletes an element from a set. <br/>
        /// If a set of values is specified, then those values are subtracted from the old set. For example, if the attribute value was the set <c>[a,b,c]</c> and the <c>DELETE</c> action specifies <c>[a,c]</c>, then the final attribute value is <c>[b]</c>. Specifying an empty set is an error.
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        public string? UpdateExpression { get; set; }
    }
}