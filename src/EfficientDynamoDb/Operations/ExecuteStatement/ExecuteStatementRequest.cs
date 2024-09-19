using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Operations.Shared;
using System;
using System.Collections.Generic;

namespace EfficientDynamoDb.Operations.ExecuteStatement
{
    public class ExecuteStatementRequest
    {
        /// <summary>
        /// Gets and sets the property Statement. 
        /// <para>
        /// The PartiQL statement representing the operation to run.
        /// </para>
        /// </summary>
        public string Statement { get; set; } = string.Empty;

        /// <summary>
        /// Gets and sets the property Parameters. 
        /// <para>
        /// The parameters for the PartiQL statement, if any.
        /// </para>
        /// </summary>
        public IReadOnlyList<AttributeValue> Parameters { get; set; } = Array.Empty<AttributeValue>();

        /// <summary>
        /// Gets and sets the property ConsistentRead. 
        /// <para>
        /// The consistency of a read operation. If set to <c>true</c>, then a strongly consistent
        /// read is used; otherwise, an eventually consistent read is used.
        /// </para>
        /// </summary>
        public bool ConsistentRead { get; set; }

        /// <summary>
        /// Gets and sets the property Limit. 
        /// <para>
        /// The maximum number of items to evaluate (not necessarily the number of matching items).
        /// If DynamoDB processes the number of items up to the limit while processing the results,
        /// it stops the operation and returns the matching values up to that point, along with
        /// a key in <c>LastEvaluatedKey</c> to apply in a subsequent operation so you can pick
        /// up where you left off. Also, if the processed dataset size exceeds 1 MB before DynamoDB
        /// reaches this limit, it stops the operation and returns the matching values up to the
        /// limit, and a key in <c>LastEvaluatedKey</c> to apply in a subsequent operation to
        /// continue the operation. 
        /// </para>
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Gets and sets the property NextToken. 
        /// <para>
        /// Set this value to get remaining results, if <c>NextToken</c> was returned in the statement
        /// response.
        /// </para>
        /// </summary>
        public string? NextToken { get; set; }

        /// <summary>
        /// Determines the level of detail about provisioned throughput consumption that is returned in the response.
        /// </summary>
        public ReturnConsumedCapacity ReturnConsumedCapacity { get; set; }

        /// <summary>
        /// Determines whether item collection metrics are returned.
        /// </summary>
        public ReturnItemCollectionMetrics ReturnItemCollectionMetrics { get; set; }

        /// <summary>
        /// Gets and sets the property ReturnValuesOnConditionCheckFailure. 
        /// <para>
        /// An optional parameter that returns the item attributes for an <c>ExecuteStatement</c>
        /// operation that failed a condition check.
        /// </para>
        ///  
        /// <para>
        /// There is no additional cost associated with requesting a return value aside from the
        /// small network and processing overhead of receiving a larger response. No read capacity
        /// units are consumed.
        /// </para>
        /// </summary>
        public ReturnValuesOnConditionCheckFailure ReturnValuesOnConditionCheckFailure { get; set; }
    }
}
