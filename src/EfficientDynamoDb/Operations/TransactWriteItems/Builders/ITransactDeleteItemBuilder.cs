using System;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.TransactWriteItems.Builders
{
    /// <summary>
    /// Provides functionality to build a DeleteItem operation in transaction, which deletes an item in a transaction based on given conditions.
    /// This interface enables configuration of the partition key, optional sort key, conditions for deletion, and handling return values if the operation fails.
    /// </summary>
    /// <typeparam name="TEntity">Type of the DB entity.</typeparam>
    public interface ITransactDeleteItemBuilder<TEntity> : ITransactWriteItemBuilder, ITableBuilder<ITransactDeleteItemBuilder<TEntity>> where TEntity : class
    {
        /// <summary>
        /// Specifies the partition key of the item to delete in transaction.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <returns>TransactDeleteItem operation builder.</returns>
        /// <remarks>
        /// This method should be used only if the table has only partition key.
        /// If the table has both partition and sort keys, use <see cref="WithPrimaryKey{TPk,TSk}"/> instead.
        /// </remarks>
        ITransactDeleteItemBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);

        /// <summary>
        /// Specifies partition and sort keys of the item to delete in transaction.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <param name="sk">Sort key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <typeparam name="TSk">Type of the sort key.</typeparam>
        /// <returns>TransactDeleteItem operation builder.</returns>
        /// <remarks>
        /// This method should be used only if the table has both partition and sort keys.
        /// If the table has only partition key, use <see cref="WithPrimaryKey{TPk}"/> instead.
        /// </remarks>
        ITransactDeleteItemBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        /// <summary>
        /// Specifies condition for the DeleteItem operation in transaction.
        /// </summary>
        /// <param name="condition">Condition to set.</param>
        /// <returns>TransactDeleteItem operation builder.</returns>
        /// <remarks>
        /// This condition is used to determine whether the DeleteItem operation should succeed or fail in transaction.
        /// </remarks>
        ITransactDeleteItemBuilder<TEntity> WithCondition(FilterBase condition);

        /// <summary>
        /// Specifies the condition function for the DeleteItem operation in transaction.
        /// </summary>
        /// <param name="conditionSetup">The condition function to set.</param>
        /// <returns>TransactDeleteItem operation builder.</returns>
        /// <remarks>
        /// This condition is used to determine whether the DeleteItem operation should succeed or fail in transaction.
        /// </remarks>
        ITransactDeleteItemBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);
        
        /// <summary>
        /// Specifies how to handle return values if the operation fails.
        /// </summary>
        /// <param name="returnValuesOnConditionCheckFailure">Option for handling return values on condition check failure.</param>
        /// <returns>TransactDeleteItem operation builder.</returns>
        ITransactDeleteItemBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure);
    }
}