using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Capacity;

namespace EfficientDynamoDb.Context.Operations.TransactGetItems
{
    public class TransactGetItemsResponse
    {
        public IReadOnlyList<TableConsumedCapacity>? ConsumedCapacity { get; }

        public IReadOnlyList<Document> Items { get; }

        public TransactGetItemsResponse(IReadOnlyList<Document> items, IReadOnlyList<TableConsumedCapacity>? consumedCapacity)
        {
            ConsumedCapacity = consumedCapacity;
            Items = items;
        }
    }
}