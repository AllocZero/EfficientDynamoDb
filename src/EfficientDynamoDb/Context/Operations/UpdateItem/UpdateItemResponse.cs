using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Attributes;

namespace EfficientDynamoDb.Context.Operations.UpdateItem
{
    public class UpdateItemResponse : WriteResponse
    {
        /// <summary>
        /// <para>A map of attribute values as they appear before or after the <c>UpdateItem</c> operation, as determined by the <see cref="UpdateItemRequest.ReturnValues"/> parameter. Each element represents one attribute.</para>
        /// The Attributes map is only present if <see cref="UpdateItemRequest.ReturnValues"/> was specified as something other than <see cref="EfficientDynamoDb.DocumentModel.ReturnDataFlags.ReturnValues.None"/> in the request.
        /// </summary>
        public Document? Attributes { get; set; }
    }
    
    public class UpdateItemEntityResponse<TEntity> : WriteResponse where TEntity : class
    {
        [DynamoDBProperty("Attributes")]
        public TEntity? Item { get; set; }
    }
}