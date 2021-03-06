using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Operations.Shared
{
    public abstract class DeleteRequest : TableRequest
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
    }
}