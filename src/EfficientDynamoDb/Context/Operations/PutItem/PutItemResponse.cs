using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.DocumentModel.Capacity;
using EfficientDynamoDb.Internal.Converters.Json;

namespace EfficientDynamoDb.Context.Operations.PutItem
{
    public class PutItemResponse : WriteResponse
    {
        /// <summary>
        /// The attribute values as they appeared before the <c>PutItem</c> operation, but only if ReturnValues is specified as ALL_OLD in the request.
        /// Each element consists of an attribute name and an attribute value.
        /// </summary>
        public Document? Attributes { get; set; }
    }

    public class PutItemEntityResponse<TEntity> : WriteResponse where TEntity : class
    {
        [DynamoDBProperty("Item")]
        public TEntity? Item { get; set; }
    }
}