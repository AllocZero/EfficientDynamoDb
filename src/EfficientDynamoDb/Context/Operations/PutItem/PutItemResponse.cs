using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel;

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
}