using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Operations.Shared;
using System.Collections.Generic;

namespace EfficientDynamoDb.Operations.BatchExecuteStatement
{
    public class BatchStatementRequest
    {
        /// <summary>
        /// Gets and sets the property Statement. 
        /// <para>
        ///  A valid PartiQL statement. 
        /// </para>
        /// </summary>
        public string Statement { get; set; } = string.Empty;

        /// <summary>
        /// Gets and sets the property ConsistentRead. 
        /// <para>
        ///  The read consistency of the PartiQL batch request. 
        /// </para>
        /// </summary>
        public bool ConsistentRead { get; set; }

        /// <summary>
        /// Gets and sets the property Parameters. 
        /// <para>
        ///  The parameters associated with a PartiQL statement in the batch request. 
        /// </para>
        /// </summary>
        public List<AttributeValue> Parameters { get; set; } = new List<AttributeValue>();

        /// <summary>
        /// Gets and sets the property ReturnValuesOnConditionCheckFailure. 
        /// <para>
        /// An optional parameter that returns the item attributes for a PartiQL batch request
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
