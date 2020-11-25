using System.Collections.Generic;
using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.PutItem
{
    public class PutItemRequest : WriteRequest
    {
        /// <summary>
        /// A map of attribute name/value pairs, one for each attribute. Only the primary key attributes are required; you can optionally provide other attribute name-value pairs for the item. <br/><br/>
        /// You must provide all of the attributes for the primary key. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide both values for both the partition key and the sort key. <br/><br/>
        /// If you specify any attributes that are part of an index key, then the data types for those attributes must match those of the schema in the table's attribute definition. <br/><br/>
        /// Empty String and Binary attribute values are allowed. Attribute values of type String and Binary must have a length greater than zero if the attribute is used as a key attribute for a table or index.
        /// </summary>
        public Document? Item { get; set; }
        
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