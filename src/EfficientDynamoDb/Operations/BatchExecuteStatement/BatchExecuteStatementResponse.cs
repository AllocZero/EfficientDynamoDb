using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Converters.Json;
using EfficientDynamoDb.Operations.Shared.Capacity;
using System.Collections.Generic;

namespace EfficientDynamoDb.Operations.BatchExecuteStatement
{
    public class BatchExecuteStatementResponse
    {
        /// <summary>
        /// Gets and sets the property Responses. 
        /// <para>
        /// The response to each PartiQL statement in the batch. The values of the list are ordered
        /// according to the ordering of the request statements.
        /// </para>
        /// </summary>
        public IReadOnlyList<Document>? Responses { get; set; }

        /// <summary>
        /// Gets and sets the property ConsumedCapacity. 
        /// <para>
        /// The capacity units consumed by the entire operation. The values of the list are ordered
        /// according to the ordering of the statements.
        /// </para>
        /// </summary>
        public List<FullConsumedCapacity>? ConsumedCapacity { get; set; }
    }
}
