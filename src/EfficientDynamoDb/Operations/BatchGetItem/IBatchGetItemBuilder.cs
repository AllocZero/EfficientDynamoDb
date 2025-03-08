using System;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Operations.BatchGetItem
{
    /// <summary>
    /// Defines the contract for building a GetItem request in a batch.
    /// </summary>
    public interface IBatchGetItemBuilder
    {
        internal PrimaryKeyNodeBase GetPrimaryKeyNode() => throw new NotImplementedException(
            $"All internal implementations of {nameof(IBatchGetItemBuilder)} must implement the {nameof(GetPrimaryKeyNode)} method.");

        internal Type GetEntityType() => throw new NotImplementedException(
            $"All internal implementations of {nameof(IBatchGetItemBuilder)} must implement the {nameof(GetEntityType)} method.");
        
        internal string? TableName => throw new NotImplementedException(
            $"All internal implementations of {nameof(IBatchGetItemBuilder)} must implement the {nameof(TableName)} property.");

        internal IBatchGetItemBuilder WithTableName(string tableName) => throw new NotImplementedException(
            $"All internal implementations of {nameof(IBatchGetItemBuilder)} must implement the {nameof(WithTableName)} method.");
        
        /// <summary>
        /// Specifies the partition key of the item to get in batch.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <returns>BatchGetItem operation builder.</returns>
        /// <remarks>
        /// This method should be used only if the table has only partition key.
        /// If the table has both partition and sort keys, use <see cref="WithPrimaryKey{TPk,TSk}"/> instead.
        /// </remarks>
        IBatchGetItemBuilder WithPrimaryKey<TPk>(TPk pk);
        
        /// <summary>
        /// Specifies partition and sort keys of the item to get in batch.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <param name="sk">Sort key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <typeparam name="TSk">Type of the sort key.</typeparam>
        /// <returns>BatchGetItem operation builder.</returns>
        /// <remarks>
        /// This method should be used only if the table has both partition and sort keys.
        /// If the table has only partition key, use <see cref="WithPrimaryKey{TPk}"/> instead.
        /// </remarks>
        IBatchGetItemBuilder WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
    }
}