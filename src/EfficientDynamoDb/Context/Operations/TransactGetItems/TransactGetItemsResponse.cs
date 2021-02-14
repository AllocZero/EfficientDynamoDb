using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.DocumentModel.Capacity;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Converters;
using EfficientDynamoDb.Internal.Converters.Json;

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

    public class TransactGetItemResponse<TEntity> where TEntity : class
    {
        [DynamoDBProperty("Item")] 
        public TEntity? Item { get; set; } = null!;
    }
    
    public class TransactGetItemsEntityResponse<TEntity> where TEntity : class
    {
        [DynamoDBProperty("ConsumedCapacity")]
        public IReadOnlyList<TableConsumedCapacity>? ConsumedCapacity { get; set; }

        [DynamoDBProperty("Responses", typeof(TransactGetItemsResponsesConverter<>))] 
        public IReadOnlyList<TransactGetItemResponse<TEntity>> Responses { get; set; } = null!;
    }
    
    internal sealed class TransactGetItemsEntityProjection<TEntity> where TEntity : class
    {
        [DynamoDBProperty("Responses", typeof(TransactGetItemsResponsesConverter<>))] 
        public IReadOnlyList<TransactGetItemResponse<TEntity>> Responses { get; set; } = null!;
    }
    
    internal sealed class TransactGetItemsResponsesConverter<TValue> : JsonIReadOnlyListDdbConverter<TValue>
    {
        public TransactGetItemsResponsesConverter(DynamoDbContextMetadata metadata) : base(CreateValueConverter(metadata))
        {
        }

        private static DdbConverter<TValue> CreateValueConverter(DynamoDbContextMetadata metadata)
        {
            var transactResponseType = typeof(TValue);
            return (DdbConverter<TValue>) metadata.GetOrAddConverter(transactResponseType, typeof(JsonObjectDdbConverter<>).MakeGenericType(transactResponseType));
        }
    }
}