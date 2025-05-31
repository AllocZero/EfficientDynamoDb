using System.Runtime.Serialization;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// Dynamodb x-amz-crc32 header value does not match the CRC32 value of the response body.
    /// </summary>
    public class ChecksumMismatchException : DdbException
    {
        public ChecksumMismatchException() : base("Dynamodb x-amz-crc32 header value does not match the CRC32 value of the response body.")
        {
        }
    }
}