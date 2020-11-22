using EfficientDynamoDb.Context.Requests.Query;

namespace EfficientDynamoDb.Context.Requests
{
    public enum Select : byte
    {
        /// <summary>
        /// Returns all of the item attributes from the specified table or index. If you query a local secondary index, then for each matching item in the index, DynamoDB fetches the entire item from the parent table. If the index is configured to project all item attributes, then all of the data can be obtained from the local secondary index, and no fetching is required.
        /// </summary>
        AllAttributes = 0,
        /// <summary>
        /// Allowed only when querying an index. Retrieves all attributes that have been projected into the index. If the index is configured to project all attributes, this return value is equivalent to specifying <see cref="AllAttributes"/>.
        /// </summary>
        AllProjectedAttributes = 1,
        /// <summary>
        /// Returns the number of matching items, rather than the matching items themselves.
        /// </summary>
        Count = 2,
        /// <summary>
        /// Returns only the attributes listed in <see cref="QueryRequest.ProjectionExpression"/>. This return value is equivalent to specifying <see cref="QueryRequest.ProjectionExpression"/> without specifying any value for <see cref="Select"/>.<br/><br/>
        /// If you query or scan a local secondary index and request only attributes that are projected into that index, the operation will read only the index and not the table. If any of the requested attributes are not projected into the local secondary index, DynamoDB fetches each of these attributes from the parent table. This extra fetching incurs additional throughput cost and latency. <br/><br/>
        /// If you query or scan a global secondary index, you can only request attributes that are projected into the index. Global secondary index queries cannot fetch attributes from the parent table.
        /// </summary>
        SpecificAttributes = 3
    }
}