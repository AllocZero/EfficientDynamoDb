using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.PutItem
{
    public class PutItemResponse : WriteResponse
    {
        /// <summary>
        /// The attribute values as they appeared before the <c>PutItem</c> operation, but only if ReturnValues is specified as ALL_OLD in the request.
        /// Each element consists of an attribute name and an attribute value.
        /// </summary>
        public Document? Attributes { get; set; }
    }

    public class PutItemEntityResponse<TEntity> : WriteEntityResponse where TEntity : class
    {
        [DynamoDBProperty("Item")]
        public TEntity? Item { get; set; }
    }
    
    internal sealed class PutItemEntityProjection<TEntity> where TEntity : class
    {
        [DynamoDBProperty("Item")]
        public TEntity? Item { get; set; }
    }
}