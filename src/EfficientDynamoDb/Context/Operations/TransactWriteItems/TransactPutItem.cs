using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems
{
    public class TransactPutItem : WriteRequest
    {
        /// <summary>
        /// A map of attribute name/value pairs, one for each attribute. Only the primary key attributes are required; you can optionally provide other attribute name-value pairs for the item. <br/><br/>
        /// You must provide all of the attributes for the primary key. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide both values for both the partition key and the sort key. <br/><br/>
        /// If you specify any attributes that are part of an index key, then the data types for those attributes must match those of the schema in the table's attribute definition. <br/><br/>
        /// Empty String and Binary attribute values are allowed. Attribute values of type String and Binary must have a length greater than zero if the attribute is used as a key attribute for a table or index.
        /// </summary>
        public Document? Item { get; set; }
        
        /// <summary>
        /// Use <see cref="ReturnValuesOnConditionCheckFailure"/> to get the item attributes if the <c>Put</c> condition fails. For <see cref="ReturnValuesOnConditionCheckFailure"/>, the valid values are: NONE and ALL_OLD.
        /// </summary>
        public ReturnValuesOnConditionCheckFailure ReturnValuesOnConditionCheckFailure { get; set; }
    }
}