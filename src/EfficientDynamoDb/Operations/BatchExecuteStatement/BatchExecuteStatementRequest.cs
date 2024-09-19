using EfficientDynamoDb.Operations.Shared;
using System;
using System.Collections.Generic;

namespace EfficientDynamoDb.Operations.BatchExecuteStatement
{
    /// <summary>
    /// This operation allows you to perform batch reads or writes on data stored in DynamoDB, using PartiQL. Each read statement in a BatchExecuteStatement must specify an equality condition on all key attributes. This enforces that each SELECT statement in a batch returns at most a single item.
    /// </summary>
    public class BatchExecuteStatementRequest
    {
        /// <summary>
        /// Gets and sets the property Statements. 
        /// <para>
        /// The list of PartiQL statements representing the batch to run.
        /// </para>
        /// </summary>
        public IReadOnlyList<BatchStatementRequest> Statements { get; set; } = Array.Empty<BatchStatementRequest>();

        /// <summary>
        /// Determines the level of detail about provisioned throughput consumption that is returned in the response.
        /// </summary>
        public ReturnConsumedCapacity ReturnConsumedCapacity { get; set; }
    }
}
