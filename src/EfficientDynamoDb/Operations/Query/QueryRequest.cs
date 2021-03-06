using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.Query
{
    public class QueryRequest : IterableRequest
    {
        /// <summary>
        /// Specifies the order for index traversal: If true (default), the traversal is performed in ascending order; if false, the traversal is performed in descending order.<br/><br/>
        /// Items with the same partition key value are stored in sorted order by sort key. If the sort key data type is Number, the results are stored in numeric order. For type String, the results are stored in order of UTF-8 bytes. For type Binary, DynamoDB treats each byte of the binary data as unsigned.<br/><br/>
        /// If <see cref="ScanIndexForward"/> is true, DynamoDB returns the results in the order in which they are stored (by sort key value). This is the default behavior. If <see cref="ScanIndexForward"/> is false, DynamoDB reads the results in reverse order by sort key value, and then returns the results to the client.
        /// </summary>
        public bool ScanIndexForward { get; set; } = true;
        
        /// <summary>
        /// The condition that specifies the key values for items to be retrieved by the Query action. <br/><br/>
        /// The condition must perform an equality test on a single partition key value. The condition can optionally perform one of several comparison tests on a single sort key value. This allows Query to retrieve one item with a given partition key value and sort key value, or several items that have the same partition key value but different sort key values.<br/><br/>
        /// The partition key equality test is required, and must be specified in the following format:
        /// <code>partitionKeyName = :partitionkeyval</code>
        /// If you also want to provide a condition for the sort key, it must be combined using AND with the condition for the sort key. Following is an example, using the = comparison operator for the sort key:
        /// <code>partitionKeyName = :partitionkeyval AND sortKeyName = :sortkeyval</code>
        /// <list type="bullet">
        /// <listheader>
        /// <description> Valid comparisons for the sort key condition are as follows:</description>
        /// </listheader>
        /// <item>
        /// <term><c>sortKeyName = :sortkeyval</c></term>
        /// <description> - <c>true</c> if the sort key value is equal to <c>:sortkeyval</c>.</description>
        /// </item>
        /// <item>
        /// <term><c>sortKeyName &lt; :sortkeyval</c></term>
        /// <description> - <c>true</c> if the sort key value is less than <c>:sortkeyval</c>.</description>
        /// </item>
        /// <item>
        /// <term><c>sortKeyName &lt;= :sortkeyval</c></term>
        /// <description> - <c>true</c> if the sort key value is less than or equal to <c>:sortkeyval</c>.</description>
        /// </item>
        /// <item>
        /// <term><c>sortKeyName > :sortkeyval</c></term>
        /// <description> - <c>true</c> if the sort key value is greater than <c>:sortkeyval</c>.</description>
        /// </item>
        /// <item>
        /// <term><c>sortKeyName >= :sortkeyval</c></term>
        /// <description> - <c>true</c> if the sort key value is greater than or equal to <c>:sortkeyval</c>.</description>
        /// </item>
        /// <item>
        /// <term><c>sortKeyName BETWEEN :sortkeyval1 AND :sortkeyval2</c></term>
        /// <description> - <c>true</c> if the sort key value is greater than or equal to <c>:sortkeyval1</c>, and less than or equal to <c>:sortkeyval2</c>.</description>
        /// </item>
        /// <item>
        /// <term><c>begins_with ( sortKeyName, :sortkeyval )</c></term>
        /// <description> - <c>true</c> if the sort key value begins with a particular operand. (You cannot use this function with a sort key that is of type Number.) Note that the function name begins_with is case-sensitive.</description>
        /// </item>
        /// </list>
        /// </summary>
        public string? KeyConditionExpression { get; set; }

        /// <summary>
        /// A string that contains conditions that DynamoDB applies after the Query operation, but before the data is returned to you. Items that do not satisfy the FilterExpression criteria are not returned. <br/><br/>
        /// A <see cref="FilterExpression"/> does not allow key attributes. You cannot define a filter expression based on a partition key or a sort key. <br/>
        /// A <see cref="FilterExpression"/> is applied after the items have already been read; the process of filtering does not consume any additional read capacity units.
        /// </summary>
        public string? FilterExpression { get; set; }

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
    }

    
}