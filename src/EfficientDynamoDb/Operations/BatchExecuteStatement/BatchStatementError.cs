using EfficientDynamoDb.DocumentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EfficientDynamoDb.Operations.BatchExecuteStatement
{
    public class BatchStatementError
    {
        /// <summary>
        /// Gets and sets the property Code. 
        /// <para>
        ///  The error code associated with the failed PartiQL batch statement. 
        /// </para>
        /// </summary>
        public BatchStatementErrorCodeEnum Code { get; set; }

        /// <summary>
        /// Gets and sets the property Item. 
        /// <para>
        /// The item which caused the condition check to fail. This will be set if ReturnValuesOnConditionCheckFailure
        /// is specified as <c>ALL_OLD</c>.
        /// </para>
        /// </summary>
        public Dictionary<string, AttributeValue> Item { get; set; } = new Dictionary<string, AttributeValue>();

        /// <summary>
        /// Gets and sets the property Message. 
        /// <para>
        ///  The error message associated with the PartiQL batch response. 
        /// </para>
        /// </summary>
        public string Message { get; set; } = string.Empty;


    }
}
