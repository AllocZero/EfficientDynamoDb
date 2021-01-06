using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.Operations.Shared;

namespace EfficientDynamoDb.Context.Operations.Query
{
    public class QueryHighLevelRequest : IterableRequest
    {
        /// <summary>
        /// Specifies the order for index traversal: If true (default), the traversal is performed in ascending order; if false, the traversal is performed in descending order.<br/><br/>
        /// Items with the same partition key value are stored in sorted order by sort key. If the sort key data type is Number, the results are stored in numeric order. For type String, the results are stored in order of UTF-8 bytes. For type Binary, DynamoDB treats each byte of the binary data as unsigned.<br/><br/>
        /// If <see cref="ScanIndexForward"/> is true, DynamoDB returns the results in the order in which they are stored (by sort key value). This is the default behavior. If <see cref="ScanIndexForward"/> is false, DynamoDB reads the results in reverse order by sort key value, and then returns the results to the client.
        /// </summary>
        public bool ScanIndexForward { get; set; } = true;
        
        public IFilter? KeyExpression { get; set; }
        
        public IFilter? FilterExpression { get; set; }
    }
}