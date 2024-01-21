using System;
using System.Collections.Generic;

namespace EfficientDynamoDb.Operations
{
    public class GetRecordsResponse
    {
        /// <summary>
        /// The next position in the shard from which to start sequentially reading stream records.
        /// If set to <c>null</c>, the shard has been closed and the requested iterator will not return any more data.
        /// </summary>
        public string? NextShardIterator { get; }
        
        /// <summary>
        /// The stream records from the shard, which were retrieved using the shard iterator.
        /// </summary>
        public IReadOnlyList<Record> Records { get; }
        
        public GetRecordsResponse(string? nextShardIterator, IReadOnlyList<Record>? records)
        {
            NextShardIterator = nextShardIterator;
            Records = records ?? Array.Empty<Record>();
        }   
    }
}