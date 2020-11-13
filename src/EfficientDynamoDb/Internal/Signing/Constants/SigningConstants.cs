using System.Text;

namespace EfficientDynamoDb.Internal.Signing.Constants
{
    public static class SigningConstants
    {
        public const string Aws4AlgorithmTag = "AWS4-HMAC-SHA256";
        
        public const string AwsSignTerminator = "aws4_request";
        
        public static readonly byte[] AwsSignTerminatorBytes = Encoding.UTF8.GetBytes("aws4_request");
        
        /// <summary>
        /// Pre-computed SHA256 hash for empty body. Used for performance reasons.
        /// </summary>
        public const string EmptyBodySha256 = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
    }
}