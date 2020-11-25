using System;
using System.Collections.Generic;
using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactGetItems
{
    public class TransactGetItemsRequest
    {
        /// <summary>
        /// A value of TOTAL causes consumed capacity information to be returned, and a value of NONE prevents that information from being returned. No other value is valid.
        /// </summary>
        public ReturnConsumedCapacity ReturnConsumedCapacity { get; set; }
        
        /// <summary>
        /// An ordered array of up to 25 <see cref="GetRequest"/> objects.
        /// </summary>
        public IReadOnlyCollection<TransactGetRequest> TransactItems { get; set; } = ArraySegment<TransactGetRequest>.Empty;
    }
}