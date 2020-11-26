using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Context.Operations.BatchWriteItem
{
    public class BatchWriteDeleteRequest
    {
        /// <summary>
        /// <para>A map of primary key attribute values that uniquely identify the item. Each entry in this map consists of an attribute name and an attribute value.</para>
        /// <para>
        /// For each primary key, you must provide all of the key attributes. For example, with a simple primary key, you only need to provide a value for the partition key.
        /// For a composite primary key, you must provide values for both the partition key and the sort key.
        /// </para>
        /// </summary>
        public IReadOnlyDictionary<string, AttributeValue> Key { get; }

        public BatchWriteDeleteRequest(IReadOnlyDictionary<string, AttributeValue> key) => Key = key;
    }
}