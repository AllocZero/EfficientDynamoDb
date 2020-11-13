namespace EfficientDynamoDb.Internal.Signing.Crypto
{
    /// <summary>
    /// The valid hashing algorithm supported by the sdk for request signing.
    /// </summary>
    public enum SigningAlgorithm
    {
        HmacSHA1,
        HmacSHA256,
    }
}