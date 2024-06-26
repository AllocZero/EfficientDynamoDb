﻿using System.Collections.Generic;
using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Converters.Json;
using EfficientDynamoDb.Operations.Shared.Capacity;

namespace EfficientDynamoDb.Operations.BatchGetItem
{
    public class BatchGetItemResponse
    {
        /// <summary>
        /// <para>The read capacity units consumed by the entire <c>BatchGetItem</c> operation.</para>
        /// <list type="bullet">
        /// <listheader>
        /// Each element consists of:
        /// </listheader>
        /// <item>
        /// <c>TableName</c> - The table that consumed the provisioned throughput.
        /// </item>
        /// <item>
        /// <c>CapacityUnits</c> - The total number of capacity units consumed.
        /// </item>
        /// </list>
        /// </summary>
        public IReadOnlyList<TableConsumedCapacity>? ConsumedCapacity { get; }

        /// <summary>
        /// A map of table name to a list of items. Each object in Responses consists of a table name, along with a list of <see cref="Document"/>.
        /// </summary>
        public IReadOnlyDictionary<string, IReadOnlyList<Document>>? Responses { get; }

        /// <summary>
        /// A map of tables and their respective keys that were not processed with the current response.
        /// The <c>UnprocessedKeys</c> value is in the same form as <see cref="BatchGetItemRequest.RequestItems"/>, so the value can be provided directly to a subsequent BatchGetItem operation.
        /// </summary>
        public IReadOnlyDictionary<string, TableBatchGetItemRequest>? UnprocessedKeys { get; }

        public BatchGetItemResponse(IReadOnlyList<TableConsumedCapacity>? consumedCapacity, IReadOnlyDictionary<string, IReadOnlyList<Document>>? responses,
            IReadOnlyDictionary<string, TableBatchGetItemRequest>? unprocessedKeys)
        {
            ConsumedCapacity = consumedCapacity;
            Responses = responses;
            UnprocessedKeys = unprocessedKeys;
        }
    }

    public class BatchGetItemResponse<TEntity>
    {
        /// <summary>
        /// <para>The read capacity units consumed by the entire <c>BatchGetItem</c> operation.</para>
        /// <list type="bullet">
        /// <listheader>
        /// Each element consists of:
        /// </listheader>
        /// <item>
        /// <c>TableName</c> - The table that consumed the provisioned throughput.
        /// </item>
        /// <item>
        /// <c>CapacityUnits</c> - The total number of capacity units consumed.
        /// </item>
        /// </list>
        /// </summary>
        public IReadOnlyList<TableConsumedCapacity>? ConsumedCapacity { get; }

        /// <summary>
        /// A list of successfully retrieved items.
        /// </summary>
        public IReadOnlyList<TEntity> Items { get; }

        /// <summary>
        /// A map of tables and their respective keys that were not processed with the current response.
        /// The <c>UnprocessedKeys</c> value is in the same form as <see cref="BatchGetItemRequest.RequestItems"/>, so the value can be provided directly to a subsequent BatchGetItem operation.
        /// </summary>
        public IReadOnlyDictionary<string, TableBatchGetItemRequest>? UnprocessedKeys { get; }

        public BatchGetItemResponse(IReadOnlyList<TableConsumedCapacity>? consumedCapacity, IReadOnlyList<TEntity> items,
            IReadOnlyDictionary<string, TableBatchGetItemRequest>? unprocessedKeys)
        {
            ConsumedCapacity = consumedCapacity;
            Items = items;
            UnprocessedKeys = unprocessedKeys;
        }
    }

    internal sealed class BatchGetItemEntityResponse<TEntity> where TEntity : class
    {
        /// <summary>
        /// A map of table name to a list of items. Each object in Responses consists of a table name, along with a list of <see cref="Document"/>.
        /// </summary>
        [DynamoDbProperty("Responses", typeof(JsonBatchGetItemResponsesConverter<,>))]
        public IReadOnlyDictionary<string, List<TEntity>>? Responses { get; set; }

        /// <summary>
        /// A map of tables and their respective keys that were not processed with the current response.
        /// The <c>UnprocessedKeys</c> value is in the same form as <see cref="BatchGetItemRequest.RequestItems"/>, so the value can be provided directly to a subsequent BatchGetItem operation.
        /// 
        /// </summary>
        [DynamoDbProperty("UnprocessedKeys", typeof(JsonBatchGetItemUnprocessedKeysConverter))]
        public IReadOnlyDictionary<string, TableBatchGetItemRequest>? UnprocessedKeys { get; set; }
        
        /// <summary>
        /// <para>The read capacity units consumed by the entire <c>BatchGetItem</c> operation.</para>
        /// <list type="bullet">
        /// <listheader>
        /// Each element consists of:
        /// </listheader>
        /// <item>
        /// <c>TableName</c> - The table that consumed the provisioned throughput.
        /// </item>
        /// <item>
        /// <c>CapacityUnits</c> - The total number of capacity units consumed.
        /// </item>
        /// </list>
        /// </summary>
        [DynamoDbProperty("ConsumedCapacity", typeof(JsonListDdbConverter<>))]
        public List<TableConsumedCapacity>? ConsumedCapacity { get; set; }
    }

    internal sealed class JsonBatchGetItemResponsesConverter<TKey, TValue> : JsonIReadOnlyDictionaryDdbConverter<TKey, TValue> where TKey : notnull
    {
        public JsonBatchGetItemResponsesConverter(DynamoDbContextMetadata metadata) : base(metadata)
        {
            var entityType = typeof(TValue).GenericTypeArguments[0];

            ValueConverter = (DdbConverter<TValue>) metadata.GetOrAddConverter(typeof(TValue), typeof(JsonListDdbConverter<>).MakeGenericType(entityType));
        }
    }

    internal sealed class JsonBatchGetItemUnprocessedKeysConverter : JsonIReadOnlyDictionaryDdbConverter<string, TableBatchGetItemRequest>
    {
        public JsonBatchGetItemUnprocessedKeysConverter(DynamoDbContextMetadata metadata) : base(metadata)
        {
            ValueConverter = (DdbConverter<TableBatchGetItemRequest>) metadata.GetOrAddConverter(typeof(TableBatchGetItemRequest),
                typeof(JsonObjectDdbConverter<TableBatchGetItemRequest>));
        }
    }
}