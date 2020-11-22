using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Context.Requests.Query
{
    public class Expression
    {
        /// <summary>
        /// In case of <see cref="QueryRequest.KeyExpression"/> - the condition that specifies the key values for items to be retrieved by the Query action. <br/>
        /// The condition must perform an equality test on a single partition key value. The condition can optionally perform one of several comparison tests on a single sort key value. This allows Query to retrieve one item with a given partition key value and sort key value, or several items that have the same partition key value but different sort key values.<br/><br/>
        /// In case of <see cref="QueryRequest.FilterExpression"/> - a string that contains conditions that DynamoDB applies after the Query operation, but before the data is returned to you. Items that do not satisfy the FilterExpression criteria are not returned. <br/>
        /// A filter expression does not allow key attributes. You cannot define a filter expression based on a partition key or a sort key. <br/>
        /// A filter expression is applied after the items have already been read; the process of filtering does not consume any additional read capacity units.
        /// </summary>
        public string? ConditionExpression { get; set; }
        
    }
}