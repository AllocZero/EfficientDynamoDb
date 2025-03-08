using System;

namespace EfficientDynamoDb.Operations.BatchWriteItem
{
    /// <summary>
    /// Provides functionality to build a DeleteItem operation in batch.
    /// </summary>
    public interface IBatchDeleteItemBuilder : IBatchWriteBuilder
    {
        /// <summary>
        /// Specifies partition and sort keys of the item to delete in batch.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <param name="sk">Sort key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <typeparam name="TSk">Type of the sort key.</typeparam>
        /// <returns>BatchDeleteItem operation builder.</returns>
        /// <remarks>
        /// This method should be used only if the table has both partition and sort keys.
        /// If the table has only partition key, use <see cref="WithPrimaryKey{TPk}"/> instead.
        /// </remarks>
        IBatchWriteBuilder WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        /// <summary>
        /// Specifies the partition key of the item to delete in batch.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <returns>BatchDeleteItem operation builder.</returns>
        /// <remarks>
        /// This method should be used only if the table has only partition key.
        /// If the table has both partition and sort keys, use <see cref="WithPrimaryKey{TPk,TSk}"/> instead.
        /// </remarks>
        IBatchWriteBuilder WithPrimaryKey<TPk>(TPk pk);

        internal IBatchDeleteItemBuilder WithTableName(string tableName) => throw new NotImplementedException(
            $"All internal implementations of {nameof(IBatchDeleteItemBuilder)} must implement the {nameof(WithTableName)} method.");
    }
}