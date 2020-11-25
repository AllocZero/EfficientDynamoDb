using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Context.Operations.BatchGetItem
{
    public class TableBatchGetItemRequest
    {
        /// <summary>
        /// Determines the read consistency model: If set to true, then the operation uses strongly consistent reads; otherwise, the operation uses eventually consistent reads. <br/><br/>
        /// </summary>
        public bool ConsistentRead { get; set; }

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
        /// Use the <c>#</c> character in a <see cref="ProjectionExpression"/> to dereference an attribute name.<br/>
        /// <example> If your <see cref="ProjectionExpression"/> is equal to <c>#pk = :pk</c>, then map <c>#pk</c> to the real attribute name in the <see cref="ExpressionAttributeNames"/> dictionary
        /// <code>AttributeNames = new Dictionary&lt;string, string>
        /// {
        ///     ["#pk"] = "pk"
        /// }
        /// </code>
        /// </example>
        /// </summary>
        public IReadOnlyDictionary<string, string>? ExpressionAttributeNames { get; set; }

        /// <summary>
        /// <para>An array of primary key attribute values that define specific items in the table.</para>
        /// For each primary key, you must provide all of the key attributes. For example, with a simple primary key, you only need to provide the partition key value. For a composite key, you must provide both the partition key value and the sort key value.
        /// </summary>
        public IReadOnlyList<IReadOnlyDictionary<string, AttributeValue>>? Keys { get; set; }

        /// <summary>
        /// A collection of strings that identifies one or more attributes to retrieve from the table. These attributes can include scalars, sets, or elements of a JSON document. <br/><br/>
        /// If no attribute names are specified, then all attributes will be returned. If any of the requested attributes are not found, they will not appear in the result. 
        /// </summary>
        public IReadOnlyList<string>? ProjectionExpression { get; set; }
    }
}