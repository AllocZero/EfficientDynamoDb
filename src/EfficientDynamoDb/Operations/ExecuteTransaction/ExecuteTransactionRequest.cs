using EfficientDynamoDb.Operations.Shared;
using System;
using System.Collections.Generic;

namespace EfficientDynamoDb.Operations.ExecuteTransaction
{
    /// <summary>
    /// This operation allows you to perform batch reads or writes on data stored in DynamoDB, using PartiQL.
    /// Each read statement in a BatchExecuteStatement must specify an equality condition on all key attributes.
    /// This enforces that each SELECT statement in a batch returns at most a single item.
    /// </summary>
    public class ExecuteTransactionRequest
    {
        /// <summary>
        /// Gets and sets the property TransactStatements. 
        /// <para>
        /// The list of PartiQL statements representing the transaction to run.
        /// </para>
        /// </summary>
        public IReadOnlyList<ParameterizedStatement> TransactStatements { get; set; } = Array.Empty<ParameterizedStatement>();

        /// <summary>
        /// Gets and sets the property ClientRequestToken. 
        /// <para>
        /// Set this value to get remaining results, if <c>NextToken</c> was returned in the statement
        /// response.
        /// </para>
        /// </summary>
        public string? ClientRequestToken { get; set; }

        /// <summary>
        /// Gets and sets the property ReturnConsumedCapacity. 
        /// <para>
        /// Determines the level of detail about either provisioned or on-demand throughput consumption
        /// that is returned in the response. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/APIReference/API_TransactGetItems.html">TransactGetItems</a>
        /// and <a href="https://docs.aws.amazon.com/amazondynamodb/latest/APIReference/API_TransactWriteItems.html">TransactWriteItems</a>.
        /// </para>
        /// </summary>
        public ReturnConsumedCapacity ReturnConsumedCapacity { get; set; }
    }
}
