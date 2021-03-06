using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.GetItem
{
    public class GetItemRequest : GetItemRequestBase
    {
        /// <summary>
        /// A map of attribute names to AttributeValue objects, representing the primary key of the item to retrieve. <br/><br/>
        /// For the primary key, you must provide all of the attributes. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide values for both the partition key and the sort key.
        /// </summary>
        public PrimaryKey? Key { get; set; }
    }
}