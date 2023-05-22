using System;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.TransactWriteItems.Builders
{
    /// <summary>
    /// Provides functionality to build a TransactPutItem operation, which inserts an item in a transaction based on given conditions.
    /// This interface enables configuration of the conditions for insertion, and handling return values if the operation fails.
    /// </summary>
    /// <typeparam name="TEntity">Type of the DB entity.</typeparam>
    public interface ITransactPutItemBuilder<TEntity> : ITransactWriteItemBuilder, ITableBuilder<ITransactPutItemBuilder<TEntity>> where TEntity : class
    {
        /// <summary>
        /// Specifies condition for the PutItem operation in transaction.
        /// </summary>
        /// <param name="condition">Condition to set.</param>
        /// <returns>TransactPutItem operation builder.</returns>
        /// <remarks>
        /// This condition is used to determine whether the PutItem operation should succeed or fail in transaction.
        /// </remarks>
        ITransactPutItemBuilder<TEntity> WithCondition(FilterBase condition);

        /// <summary>
        /// Specifies the condition function for the PutItem operation in transaction.
        /// </summary>
        /// <param name="conditionSetup">The condition function to set.</param>
        /// <returns>TransactPutItem operation builder.</returns>
        /// <remarks>
        /// This condition is used to determine whether the PutItem operation should succeed or fail in transaction.
        /// </remarks>
        ITransactPutItemBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);
        
        /// <summary>
        /// Specifies how to handle return values if the operation fails.
        /// </summary>
        /// <param name="returnValuesOnConditionCheckFailure">Option for handling return values on condition check failure.</param>
        /// <returns>TransactPutItem operation builder.</returns>
        ITransactPutItemBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure);        
    }
}