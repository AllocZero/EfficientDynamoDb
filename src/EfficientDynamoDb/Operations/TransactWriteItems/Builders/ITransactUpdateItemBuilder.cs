using System;
using System.Linq.Expressions;
using EfficientDynamoDb.FluentCondition;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.TransactWriteItems.Builders
{
    /// <summary>
    /// Defines the contract for building a TransactUpdateItem operation in a transaction. 
    /// This interface allows for the specification of partition keys and sort keys for the item to be updated,
    /// as well as the conditions that must be met for the update to be successful.
    /// It also provides methods to handle return values if the operation fails,
    /// and to specify the attribute to be updated in the DynamoDB item.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity that this builder will configure updates for.</typeparam>
    public interface ITransactUpdateItemBuilder<TEntity> : ITransactWriteItemBuilder, IUpdateItemBuilder<ITransactUpdateItemBuilder<TEntity>> where TEntity : class
    {
        /// <summary>
        /// Specifies the partition key of the item to update in transaction.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <returns>TransactUpdateItem operation builder.</returns>
        /// <remarks>
        /// This method should be used only if the table has only partition key.
        /// If the table has both partition and sort keys, use <see cref="WithPrimaryKey{TPk,TSk}"/> instead.
        /// </remarks>
        ITransactUpdateItemBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);

        /// <summary>
        /// Specifies partition and sort keys of the item to update in transaction.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <param name="sk">Sort key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <typeparam name="TSk">Type of the sort key.</typeparam>
        /// <returns>TransactUpdateItem operation builder.</returns>
        /// <remarks>
        /// This method should be used only if the table has both partition and sort keys.
        /// If the table has only partition key, use <see cref="WithPrimaryKey{TPk}"/> instead.
        /// </remarks>
        ITransactUpdateItemBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        /// <summary>
        /// Specifies condition for the UpdateItem operation in transaction.
        /// </summary>
        /// <param name="condition">Condition to set.</param>
        /// <returns>TransactUpdateItem operation builder.</returns>
        /// <remarks>
        /// This condition is used to determine whether the UpdateItem operation should succeed or fail in transaction.
        /// </remarks>
        ITransactUpdateItemBuilder<TEntity> WithCondition(FilterBase condition);

        /// <summary>
        /// Specifies the condition function for the UpdateItem operation in transaction.
        /// </summary>
        /// <param name="conditionSetup">The condition function to set.</param>
        /// <returns>TransactUpdateItem operation builder.</returns>
        /// <remarks>
        /// This condition is used to determine whether the UpdateItem operation should succeed or fail in transaction.
        /// </remarks>
        ITransactUpdateItemBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);
        
        /// <summary>
        /// Specifies how to handle return values if the operation fails.
        /// </summary>
        /// <param name="returnValuesOnConditionCheckFailure">Option for handling return values on condition check failure.</param>
        /// <returns>TransactDeleteItem operation builder.</returns>
        ITransactUpdateItemBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure);
        
        /// <summary>
        /// Specifies the attribute to be updated in the DynamoDB item. in transaction.
        /// </summary>
        /// <param name="expression">An expression identifying the attribute to be updated.</param>
        /// <typeparam name="TProperty">The type of the attribute to be updated.</typeparam>
        /// <returns>UpdateItem operation builder.</returns>
        /// <remarks>
        /// To update multiple attributes, call this method multiple times.
        /// For a detailed walkthrough and examples, refer to the developer guide: https://alloczero.github.io/EfficientDynamoDb/docs/dev_guide/dev-guide/high-level/update-expression
        /// </remarks>
        IAttributeUpdate<ITransactUpdateItemBuilder<TEntity>, TEntity, TProperty> On<TProperty>(Expression<Func<TEntity, TProperty>> expression);
    }
}