using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Capacity;

namespace EfficientDynamoDb.Context.Operations.TransactGetItems
{
    public class TransactGetItemsResponse
    {
        public IReadOnlyList<TableConsumedCapacity>? ConsumedCapacity { get; set; }

        public IReadOnlyList<Document> Items { get; set; } = null!;
    }
}