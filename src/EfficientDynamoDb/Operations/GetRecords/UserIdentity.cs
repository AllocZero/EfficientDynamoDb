namespace EfficientDynamoDb.Operations
{
    /// <summary>
    /// Contains details about the type of identity that made the request.
    /// </summary>
    public class UserIdentity
    {
        /// <summary>
        /// A unique identifier for the entity that made the call.
        /// For Time To Live, the principalId is "dynamodb.amazonaws.com".
        /// </summary>
        public string PrincipalId { get; set; } = "";

        /// <summary>
        /// The type of the identity.
        /// For Time To Live, the type is "Service".
        /// </summary>
        public string Type { get; set; } = "";
    }
}