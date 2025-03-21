﻿using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Operations.BatchExecuteStatement;
using EfficientDynamoDb.Operations.Shared.Capacity;
using System.Collections.Generic;

namespace EfficientDynamoDb.Operations.ExecuteTransaction
{
    public class ExecuteTransactionResponse
    {
        /// <summary>
        /// Gets and sets the property Responses. 
        /// <para>
        /// The response to each PartiQL statement in the batch. The values of the list are ordered
        /// according to the ordering of the request statements.
        /// </para>
        /// </summary>
        public IReadOnlyList<ItemResponse> Responses { get; set; } = null!;

        /// <summary>
        /// Gets and sets the property ConsumedCapacity. 
        /// <para>
        /// The capacity units consumed by the entire operation. The values of the list are ordered
        /// according to the ordering of the statements.
        /// </para>
        /// </summary>
        public IReadOnlyList<FullConsumedCapacity>? ConsumedCapacity { get; set; }
    }

    public class ItemResponse
    {
        public Document? Item { get; set; }
    }
}
