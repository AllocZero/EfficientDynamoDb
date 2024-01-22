using System;
using System.Collections.Generic;

namespace EfficientDynamoDb.Operations
{
    public class ListStreamsResponse
    {
        /// <summary>
        /// The stream ARN of the item where the operation stopped, inclusive of the previous result set.
        /// Use this value to start a new operation, excluding this value in the new request.
        /// </summary>
        /// <remarks>
        /// If <see cref="LastEvaluatedStreamArn"/> is empty, then the "last page" of results has been processed and there is no more data to be retrieved.
        /// If <see cref="LastEvaluatedStreamArn"/> is not empty, it does not necessarily mean that there is more data in the result set.
        /// The only way to know when you have reached the end of the result set is when <see cref="LastEvaluatedStreamArn"/> is empty.
        /// </remarks>
        public string? LastEvaluatedStreamArn { get; set; }

        /// <summary>
        /// A list of stream descriptors.
        /// </summary>
        public IReadOnlyList<StreamInfo> Streams { get; set; } = Array.Empty<StreamInfo>();
    }
}