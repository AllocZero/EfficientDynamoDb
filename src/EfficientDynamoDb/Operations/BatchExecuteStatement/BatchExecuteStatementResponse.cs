using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Converters.Json;
using EfficientDynamoDb.Operations.Shared.Capacity;
using System.Collections.Generic;

namespace EfficientDynamoDb.Operations.BatchExecuteStatement
{
    public class BatchExecuteStatementResponse
    {
        /// <summary>
        /// Gets and sets the property Responses. 
        /// <para>
        /// The response to each PartiQL statement in the batch. The values of the list are ordered
        /// according to the ordering of the request statements.
        /// </para>
        /// </summary>
        public IReadOnlyList<Document>? Responses { get; set; }

        /// <summary>
        /// Gets and sets the property ConsumedCapacity. 
        /// <para>
        /// The capacity units consumed by the entire operation. The values of the list are ordered
        /// according to the ordering of the statements.
        /// </para>
        /// </summary>
        public List<FullConsumedCapacity>? ConsumedCapacity { get; set; }
    }

    public class BatchExecuteStatementEntityResponse<TEntity> where TEntity : class
    {
        /// <summary>
        /// The capacity units consumed by the entire operation. The values of the list are ordered according to the ordering of the statements.
        /// </summary>
        [DynamoDbProperty("ConsumedCapacity", typeof(JsonListDdbConverter<>))]
        public List<TableConsumedCapacity>? ConsumedCapacity { get; set; }

        /// <summary>
        /// The response to each PartiQL statement in the batch. The values of the list are ordered according to the ordering of the request statements.
        /// </summary>
        [DynamoDbProperty("Responses", typeof(BatchExecuteStatementItemsResponsesConverter<>))]
        public IReadOnlyList<BatchExecuteStatementItemResponse<TEntity>> Responses { get; set; } = null!;
    }

    public class BatchExecuteStatementItemResponse<TEntity> where TEntity : class
    {
        [DynamoDbProperty("Item")]
        public TEntity? Item { get; set; } = null!;
    }

    internal sealed class BatchExecuteStatementItemsResponsesConverter<TValue> : JsonIReadOnlyListDdbConverter<TValue>
    {
        public BatchExecuteStatementItemsResponsesConverter(DynamoDbContextMetadata metadata) : base(CreateValueConverter(metadata))
        {
        }

        private static DdbConverter<TValue> CreateValueConverter(DynamoDbContextMetadata metadata)
        {
            var transactResponseType = typeof(TValue);
            return (DdbConverter<TValue>)metadata.GetOrAddConverter(transactResponseType, typeof(JsonObjectDdbConverter<>).MakeGenericType(transactResponseType));
        }
    }
}
