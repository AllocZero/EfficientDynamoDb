using System;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.TransactWriteItems.Builders
{
    /// <summary>
    /// Defines a contract for constructing a condition check operation in a DynamoDB transaction.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity for the condition check.</typeparam>
    /// <remarks>
    /// The interface provides methods to specify the primary key(s) of the item to perform the condition check on,
    /// to define the condition for the check, and to handle return values if the condition check fails.
    /// </remarks>
    public interface ITransactConditionCheckBuilder<TEntity> : ITransactWriteItemBuilder, ITableBuilder<ITransactConditionCheckBuilder<TEntity>> where TEntity : class
    {
        /// <summary>
        /// Specifies the partition key of the of the target item for the condition check.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <returns>TransactConditionCheck operation builder.</returns>t
        /// <remarks>
        /// This method should be used only if the table has only partition key.
        /// If the table has both partition and sort keys, use <see cref="WithPrimaryKey{TPk,TSk}"/> instead.
        /// </remarks>
        ITransactConditionCheckBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);

        /// <summary>
        /// Specifies partition and sort keys of the of the target item for the condition check.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <param name="sk">Sort key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <typeparam name="TSk">Type of the sort key.</typeparam>
        /// <returns>TransactConditionCheck operation builder.</returns>
        /// <remarks>
        /// This method should be used only if the table has both partition and sort keys.
        /// If the table has only partition key, use <see cref="WithPrimaryKey{TPk}"/> instead.
        /// </remarks>
        ITransactConditionCheckBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        /// <summary>
        /// Specifies a condition to be checked during the transaction.
        /// </summary>
        /// <param name="condition">The condition to be checked.</param>
        /// <returns>TransactConditionCheck operation builder.</returns>
        /// <remarks>
        /// This condition is used to determine whether the transaction should succeed or fail.
        /// </remarks>
        ITransactConditionCheckBuilder<TEntity> WithCondition(FilterBase condition);

        /// <summary>
        /// Specifies a condition to be checked during the transaction.
        /// </summary>
        /// <param name="conditionSetup">Function to set up the condition.</param>
        /// <returns>TransactConditionCheck operation builder.</returns>
        /// <remarks>
        /// This condition is used to determine whether the transaction should succeed or fail.
        /// </remarks>
        ITransactConditionCheckBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);

        /// <summary>
        /// Specifies how to handle return values if the condition check fails.
        /// </summary>
        /// <param name="returnValuesOnConditionCheckFailure">Option for handling return values on condition check failure.</param>
        /// <returns>TransactConditionCheck operation builder.</returns>
        ITransactConditionCheckBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure);
    }
}