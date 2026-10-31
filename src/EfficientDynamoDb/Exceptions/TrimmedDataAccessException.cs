using System;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// The requested stream records are beyond the trim horizon: they exceeded the 24 hour retention of the stream.<br/>
    /// OK to retry? No. Request a new shard iterator from a position that is still retained instead.
    /// </summary>
    public class TrimmedDataAccessException : DdbException
    {
        internal override OpErrorType OpErrorType => ErrorType;
        
        internal static OpErrorType ErrorType => OpErrorType.TrimmedDataAccess;
        
        public TrimmedDataAccessException(string message) : base(message)
        {
        }

        public TrimmedDataAccessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}