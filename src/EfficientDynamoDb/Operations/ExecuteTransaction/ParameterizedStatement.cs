using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Operations.Shared;
using System.Collections.Generic;

namespace EfficientDynamoDb.Operations.ExecuteTransaction
{
    public class ParameterizedStatement
    {
        /// <summary>
        /// Gets and sets the property Statement. 
        /// <para>
        ///  A PartiQL statement that uses parameters. 
        /// </para>
        /// </summary>
        public string Statement { get; set; } = string.Empty;

        /// <summary>
        /// Gets and sets the property Parameters. 
        /// <para>
        ///  The parameter values. 
        /// </para>
        /// </summary>
        //[AWSProperty(Min = 1)] // TODO check if this is needed
        public List<AttributeValue> Parameters { get; set; } = new List<AttributeValue>();

        /// <summary>
        /// Use <see cref="ReturnValuesOnConditionCheckFailure"/> to get the item attributes if the <c>Delete</c> condition fails. For <see cref="ReturnValuesOnConditionCheckFailure"/>, the valid values are: NONE and ALL_OLD.
        /// </summary>
        public ReturnValuesOnConditionCheckFailure ReturnValuesOnConditionCheckFailure { get; set; }
    }
}
