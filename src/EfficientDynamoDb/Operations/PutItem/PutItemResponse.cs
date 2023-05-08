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

    /// <summary>
    /// PutItem response with entity.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity in response.</typeparam>
    public class PutItemEntityResponse<TEntity> : WriteEntityResponse where TEntity : class
    {
        /// <summary>
        /// The item as it appeared before the <c>PutItem</c> operation, but only if ReturnValues is specified as ALL_OLD in the request.
        /// </summary>
        [DynamoDbProperty("Attributes")]
        public TEntity? Item { get; set; }
    }
    
    internal sealed class PutItemEntityProjection<TEntity> where TEntity : class
    {
        /// <summary>
        /// The item as it appeared before the <c>PutItem</c> operation, but only if ReturnValues is specified as ALL_OLD in the request.
        /// </summary>
        [DynamoDbProperty("Attributes")]
        public TEntity? Item { get; set; }
    }
}