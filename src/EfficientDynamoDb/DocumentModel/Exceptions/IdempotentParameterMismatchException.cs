using System;
using System.Runtime.Serialization;

namespace EfficientDynamoDb.DocumentModel.Exceptions
{
    /// <summary>
    /// DynamoDB rejected the request because you retried a request with a different payload but with an idempotent token that was already used.
    /// </summary>
    public class IdempotentParameterMismatchException : DdbException
    {
        public IdempotentParameterMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public IdempotentParameterMismatchException(string message) : base(message)
        {
        }

        public IdempotentParameterMismatchException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}