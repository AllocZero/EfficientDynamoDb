using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Context.Operations.TransactGetItems
{
    public class TransactGetRequest : GetRequest
    {
        /// <summary>
        /// A map of attribute names to AttributeValue objects, representing the primary key of the item to retrieve. <br/><br/>
        /// For the primary key, you must provide all of the attributes. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide values for both the partition key and the sort key.
        /// </summary>
        public PrimaryKey? Key { get; set; }
    }
}