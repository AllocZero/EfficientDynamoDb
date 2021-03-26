namespace EfficientDynamoDb.Operations.Shared
{
    public class UpdateRequest : WriteRequest
    {
        /// <summary>
        /// A map of attribute names to AttributeValue objects, representing the primary key of the item to retrieve. <br/><br/>
        /// For the primary key, you must provide all of the attributes. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide values for both the partition key and the sort key.
        /// </summary>
        public PrimaryKey? Key { get; set; }
        
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