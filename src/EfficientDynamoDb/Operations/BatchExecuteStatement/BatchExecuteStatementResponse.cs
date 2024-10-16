using EfficientDynamoDb.DocumentModel;
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
        public IReadOnlyList<BatchStatementResponse> Responses { get; set; } = null!;

        /// <summary>
        /// Gets and sets the property ConsumedCapacity. 
        /// <para>
        /// The capacity units consumed by the entire operation. The values of the list are ordered
        /// according to the ordering of the statements.
        /// </para>
        /// </summary>
        public IReadOnlyList<FullConsumedCapacity>? ConsumedCapacity { get; set; }
    }

    public class BatchStatementResponse
    {
        public BatchStatementError? Error { get; set; }

        public Document? Item { get; set; }

        public string? TableName { get; set; }
    }

    public class BatchStatementError
    {
        public string? Code { get; set; }

        public string? Message { get; set; }

        public Document? Item { get; set; }
    }
}
