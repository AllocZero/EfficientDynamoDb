using System;
using System.Linq.Expressions;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.FluentCondition.Factories;
using EfficientDynamoDb.FluentCondition.Operators.Update;
using EfficientDynamoDb.FluentCondition.Operators.Update.AssignConcat;
using EfficientDynamoDb.FluentCondition.Operators.Update.AssignMath;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.FluentCondition
{
    // SET #a = if_not_exists(#a, :val)
    // SET #a = #a + :val


    // Update.On(x => x.A).Insert(x => x.B)

    // Joiner.Combine(Update.On(x => x.A).Assign(x => x.B), Update.On(x => x.A).Assign(x => x.B))

    // context.Update().When(condition)
    // context.Update().Property(x => x.A).Assign(x => x.B)
    //                 .On(x => x.A).AssignSum(x => x.B, x => x.C)

    // Update.On(x => x.A).Assign(x => x.B)
    //       .On(x => x.A).AssignSum(x => x.B, x => x.C)

    // Update.On(x => x.A).Assign(x => x.B)
    // Update.On(x => x.A).AssignSum(x => x.B, x => x.C)
    // Update.On(x => x.A).AssignSum(x => x.B, 5)
    // Update.On(x => x.A).AssignSubtraction(x => x.B, x => x.C)

    // Update.On(x => x.A).Set(x => x.B)
    // Update.On(x => x.A).SetSum(x => x.B)
    // Update.On(x => x.A).SetSubtraction(x => x.B)

    // Update.On(x => x.A).Append(x => x.B)
    // Update.On(x => x.A).Prepend(x => x.B)
    // Update.On(x => x.A).AssignConcat(x => x.A, x => x.B)
    // Update.On(x => x.A).AssignConcat(x => x.B, x => x.A)
    // Update.On(x => x.A).AssignConcat(x => x.B, x => x.C)

    public interface IUpdateItemBuilder<out TUpdateRequestBuilder> : ITableBuilder<TUpdateRequestBuilder> 
        where TUpdateRequestBuilder : ITableBuilder<TUpdateRequestBuilder>
    {
        internal TUpdateRequestBuilder Create(UpdateBase update, BuilderNodeType nodeType);
    }
    
    public interface IAttributeUpdate<out TUpdateItemBuilder, TEntity, TProperty> where TEntity : class where TUpdateItemBuilder : IUpdateItemBuilder<TUpdateItemBuilder>
    {
        /// <summary>
        /// Assigns a specific value to a target DynamoDB property.
        /// </summary>
        /// <param name="value">The value to be set.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder Assign(TProperty value);

        /// <summary>
        /// Assigns the value of a specified DynamoDB property to target DynamoDB property.
        /// </summary>
        /// <param name="property">The DynamoDB property whose value is to be used.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the source property does not exist in DynamoDB, the operation will fail.
        /// To prevent this, use the <see cref="Assign(Expression{Func{TEntity,TProperty}},TProperty)"/> overload.
        /// </remarks>
        public TUpdateItemBuilder Assign(Expression<Func<TEntity, TProperty>> property);

        /// <summary>
        /// Assigns the value of a specified DynamoDB property to another DynamoDB property.
        /// If the source property does not exist in DynamoDB, the specified fallback value is used instead.
        /// </summary>
        /// <param name="property">The DynamoDB property whose value is to be used.</param>
        /// <param name="fallbackValue">The fallback value to be used if the source property doesn't exist in DynamoDB.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder Assign(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue);


        /// <summary>
        /// Assigns the sum of the specified DynamoDB properties to a target DynamoDB property.
        /// </summary>
        /// <param name="left">A property to use as a left operand in sum operation.</param>
        /// <param name="right">A property to use as a right operand in sum operation.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If any of the source properties does not exist in DynamoDB, the operation will fail.
        /// Use overloads with fallback values to prevent this.
        /// </remarks>
        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right);
        
        /// <summary>
        /// Assigns the sum of the specified DynamoDB properties to a target DynamoDB property.
        /// If property specified as left operand does not exist in DynamoDB, the specified fallback value is used instead.
        /// </summary>
        /// <param name="left">A property to use as a left operand in sum operation.</param>
        /// <param name="leftFallbackValue">Fallback value to be used if the left operand property doesn't exist in DynamoDB.</param>
        /// <param name="right">A property to use as a right operand in sum operation.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If property specified as right operand does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right);
        
        /// <summary>
        /// Assigns the sum of the specified DynamoDB properties to a target DynamoDB property.
        /// If property specified as right operand does not exist in DynamoDB, the specified fallback value is used instead.
        /// </summary>
        /// <param name="left">A property to use as a left operand in sum operation.</param>
        /// <param name="right">A property to use as a right operand in sum operation.</param>
        /// <param name="rightFallbackValue">Fallback value to be used if the right operand property doesn't exist in DynamoDB.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If property specified as left operand does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);
        
        /// <summary>
        /// Assigns the sum of the specified DynamoDB properties to a target DynamoDB property.
        /// If any of the source properties does not exist in DynamoDB, the specified fallback values are used instead.
        /// </summary>
        /// <param name="left">A property to use as a left operand in sum operation.</param>
        /// <param name="leftFallbackValue">Fallback value to be used if the left operand property doesn't exist in DynamoDB.</param>
        /// <param name="right">A property to use as a right operand in sum operation.</param>
        /// <param name="rightFallbackValue">Fallback value to be used if the right operand property doesn't exist in DynamoDB.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);

        /// <summary>
        /// Assigns the sum of the specified DynamoDB property and a constant value to a target DynamoDB property.
        /// </summary>
        /// <param name="left">A property to use as a left operand in sum operation.</param>
        /// <param name="right">A value to use as a right operand in sum operation.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If property specified as left operand does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty right);
        
        /// <summary>
        /// Assigns the sum of the specified DynamoDB property and a constant value to a target DynamoDB property.
        /// If the property specified as left operand does not exist in DynamoDB, the specified fallback value is used instead.
        /// </summary>
        /// <param name="left">A property to use as a left operand in sum operation.</param>
        /// <param name="leftFallbackValue">Fallback value to be used if the left operand property doesn't exist in DynamoDB.</param>
        /// <param name="right">A value to use as a right operand in sum operation.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, TProperty right);

        /// <summary>
        /// Assigns the sum of the constant value and the specified DynamoDB property to a target DynamoDB property.
        /// </summary>
        /// <param name="left">A value to use as a left operand in sum operation.</param>
        /// <param name="right">A property to use as a right operand in sum operation.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If property specified as right operand does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder AssignSum(TProperty left, Expression<Func<TEntity, TProperty>> right);
        
        /// <summary>
        /// Assigns the sum of the constant value and the specified DynamoDB property to a target DynamoDB property.
        /// </summary>
        /// <param name="left">A value to use as a left operand in sum operation.</param>
        /// <param name="right">A property to use as a right operand in sum operation.</param>
        /// <param name="rightFallbackValue">Fallback value to be used if the right operand property doesn't exist in DynamoDB.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder AssignSum(TProperty left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);
        
        /// <summary>
        /// Increments the target DynamoDB property by the value of the specified property.
        /// </summary>
        /// <param name="right">A property to use as an increment value.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the target property or property specified as increment value does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder Increment(Expression<Func<TEntity, TProperty>> right);
        
        /// <summary>
        /// Increments the target DynamoDB property by the value of the specified property.
        /// </summary>
        /// <param name="leftFallbackValue">Fallback value to use if the target DynamoDB property doesn't exist in DynamoDB.</param>
        /// <param name="right">A property to use as an increment value.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the property specified as increment value does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder Increment(TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right);
        
        /// <summary>
        /// Increments the target DynamoDB property by the value of the specified property.
        /// </summary>
        /// <param name="leftFallbackValue">Fallback value to use if the target DynamoDB property doesn't exist in DynamoDB.</param>
        /// <param name="right">A property to use as an increment value.</param>
        /// <param name="rightFallbackValue">Fallback value to use if the property used as an increment value doesn't exist in DynamoDB.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder Increment(TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);
        
        /// <summary>
        /// Increments the target DynamoDB property by the specified value.
        /// </summary>
        /// <param name="right">An increment value.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the target property does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder Increment(TProperty right);
        
        /// <summary>
        /// Increments the target DynamoDB property by the specified value.
        /// </summary>
        /// <param name="leftFallbackValue">Fallback value to use if the target DynamoDB property doesn't exist in DynamoDB.</param>
        /// <param name="right">An increment value.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder Increment(TProperty leftFallbackValue, TProperty right);

        /// <summary>
        /// Subtracts the value of the right property from the left property and assigns the result to the target property.
        /// </summary>
        /// <param name="left">The property whose value will be subtracted from.</param>
        /// <param name="right">The property whose value will be subtracted.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If any of the properties specified as operands does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right);
        
        /// <summary>
        /// Subtracts the value of the right property from the left property and assigns the result to the target property.
        /// If the left property doesn't exist, uses the provided fallback value instead.
        /// </summary>
        /// <param name="left">The property whose value will be subtracted from.</param>
        /// <param name="leftFallbackValue">The fallback value if the left property doesn't exist.</param>
        /// <param name="right">The property whose value will be subtracted.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the property specified as right operand does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right);

        /// <summary>
        /// Subtracts the value of the right property from the left property and assigns the result to the target property.
        /// If the right property doesn't exist, uses the provided fallback value instead.
        /// </summary>
        /// <param name="left">The property whose value will be subtracted from.</param>
        /// <param name="right">The property whose value will be subtracted.</param>
        /// <param name="rightFallbackValue">The fallback value if the right property doesn't exist.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the property specified as left operand does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);
        
        /// <summary>
        /// Subtracts the value of the right property from the left property and assigns the result to the target property.
        /// If either property doesn't exist, uses the corresponding fallback value instead.
        /// </summary>
        /// <param name="left">The property whose value will be subtracted from.</param>
        /// <param name="leftFallbackValue">The fallback value if the left property doesn't exist.</param>
        /// <param name="right">The property whose value will be subtracted.</param>
        /// <param name="rightFallbackValue">The fallback value if the right property doesn't exist.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);

        /// <summary>
        /// Subtracts the provided value from the left property and assigns the result to the target property.
        /// </summary>
        /// <param name="left">The property whose value will be subtracted from.</param>
        /// <param name="right">The value to subtract.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the property specified as left operand does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty right);
        
        /// <summary>
        /// Subtracts the provided value from the left property and assigns the result to the target property.
        /// If the left property doesn't exist, uses the provided fallback value instead.
        /// </summary>
        /// <param name="left">The property whose value will be subtracted from.</param>
        /// <param name="leftFallbackValue">The fallback value if the left property doesn't exist.</param>
        /// <param name="right">The value to subtract.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, TProperty right);

        /// <summary>
        /// Subtracts the value of the right property from the provided value and assigns the result to the target property.
        /// </summary>
        /// <param name="left">The value to be subtracted from.</param>
        /// <param name="right">The property whose value will be subtracted.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the property specified as right operand does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder AssignSubtraction(TProperty left, Expression<Func<TEntity, TProperty>> right);
        
        /// <summary>
        /// Subtracts the value of the right property from the provided value and assigns the result to the target property.
        /// If the right property doesn't exist, uses the provided fallback value instead.
        /// </summary>
        /// <param name="left">The value to be subtracted from.</param>
        /// <param name="right">The property whose value will be subtracted.</param>
        /// <param name="rightFallbackValue">The fallback value if the right property doesn't exist.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder AssignSubtraction(TProperty left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);
        
        /// <summary>
        /// Decrements the target DynamoDB property by the value of the specified property.
        /// </summary>
        /// <param name="right">A property to use as a decrement value.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the target property or property specified as decrement value does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder Decrement(Expression<Func<TEntity, TProperty>> right);
        
        /// <summary>
        /// Decrements the target DynamoDB property by the value of the specified property.
        /// </summary>
        /// <param name="leftFallbackValue">Fallback value to use if the target DynamoDB property doesn't exist in DynamoDB.</param>
        /// <param name="right">A property to use as a decrement value.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the property specified as decrement value does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder Decrement(TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right);
        
        /// <summary>
        /// Decrements the target DynamoDB property by the value of the specified property.
        /// </summary>
        /// <param name="leftFallbackValue">Fallback value to use if the target DynamoDB property doesn't exist in DynamoDB.</param>
        /// <param name="right">A property to use as a decrement value.</param>
        /// <param name="rightFallbackValue">Fallback value to use if the property used as a decrement value doesn't exist in DynamoDB.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder Decrement(TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);
        
        /// <summary>
        /// Decrements the target DynamoDB property by the specified value.
        /// </summary>
        /// <param name="right">A decrement value.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the target property does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder Decrement(TProperty right);
        
        /// <summary>
        /// Decrements the target DynamoDB property by the specified value.
        /// </summary>
        /// <param name="leftFallbackValue">Fallback value to use if the target DynamoDB property doesn't exist in DynamoDB.</param>
        /// <param name="right">A decrement value.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder Decrement(TProperty leftFallbackValue, TProperty right);

        /// <summary>
        /// Appends value of the specified property to the target list property.
        /// </summary>
        /// <param name="property">A DynamoDB property whose value will be appended.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the target property or property specified as a value to append does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder Append(Expression<Func<TEntity, TProperty>> property);

        /// <summary>
        /// Appends value of the specified property to the target list property.
        /// </summary>
        /// <param name="targetFallbackValue">The fallback value to be used if the target property doesn't exist in DynamoDB.</param>
        /// <param name="property">A DynamoDB property whose value will be appended.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the property specified as a value to append does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder Append(TProperty targetFallbackValue, Expression<Func<TEntity, TProperty>> property);
        
        /// <summary>
        /// Appends value of the specified property to the target list property.
        /// </summary>
        /// <param name="property">A DynamoDB property whose value will be appended.</param>
        /// <param name="fallbackValue">The fallback value to be used if the specified property doesn't exist in DynamoDB.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the target property does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder Append(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue); // SET #a = list_append(#a, if_not_exists(#b, :fallbackVal))
        
        /// <summary>
        /// Appends value of the specified property to the target list property.
        /// </summary>
        /// <param name="targetFallbackValue">The fallback value to be used if the target property doesn't exist in DynamoDB.</param>
        /// <param name="property">A DynamoDB property whose value will be appended.</param>
        /// <param name="fallbackValue">The fallback value to be used if the specified property doesn't exist in DynamoDB.</param>
        /// <returns>
        /// An UpdateItem operation builder.
        /// </returns>
        public TUpdateItemBuilder Append(TProperty targetFallbackValue, Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue);
        
        /// <summary>
        /// Appends the specified value to the target list property.
        /// </summary>
        /// <param name="value">A value to be appended.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the target property does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder Append(TProperty value);
        
        /// <summary>
        /// Appends the specified value to the target list property.
        /// </summary>
        /// <param name="targetFallbackValue">The fallback value to be used if the target property doesn't exist in DynamoDB.</param>
        /// <param name="right">A value to be appended.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder Append(TProperty targetFallbackValue, TProperty right);

        /// <summary>
        /// Prepends value of the specified property to the target list property.
        /// </summary>
        /// <param name="property">A DynamoDB property whose value will be prepended.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the target property or property specified as a value to prepend does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder Prepend(Expression<Func<TEntity, TProperty>> property);
        
        /// <summary>
        /// Prepends value of the specified property to the target list property.
        /// </summary>
        /// <param name="targetFallbackValue">The fallback value to be used if the target property doesn't exist in DynamoDB.</param>
        /// <param name="property">A DynamoDB property whose value will be prepended.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the property specified as a value to prepend does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder Prepend(TProperty targetFallbackValue, Expression<Func<TEntity, TProperty>> property);
        
        /// <summary>
        /// Prepends value of the specified property to the target list property.
        /// </summary>
        /// <param name="property">A DynamoDB property whose value will be prepended.</param>
        /// <param name="fallbackValue">The fallback value to be used if the specified property doesn't exist in DynamoDB.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the target property does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder Prepend(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue); // SET #a = list_append(if_not_exists(#b, :fallbackVal), #a)
        
        /// <summary>
        /// Prepends value of the specified property to the target list property.
        /// </summary>
        /// <param name="targetFallbackValue">The fallback value to be used if the target property doesn't exist in DynamoDB.</param>
        /// <param name="property">A DynamoDB property whose value will be prepended.</param>
        /// <param name="fallbackValue">The fallback value to be used if the specified property doesn't exist in DynamoDB.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder Prepend(TProperty targetFallbackValue, Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue);
        
        /// <summary>
        /// Prepends the specified value to the target list property.
        /// </summary>
        /// <param name="value">A value to be prepended.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the target property does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder Prepend(TProperty value);
        
        /// <summary>
        /// Prepends the specified value to the target list property.
        /// </summary>
        /// <param name="targetFallbackValue">The fallback value to be used if the target property doesn't exist in DynamoDB.</param>
        /// <param name="value">A value to be prepended.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder Prepend(TProperty targetFallbackValue, TProperty value);
        
        /// <summary>
        /// Assigns the concatenation of specified list properties to the target list property.
        /// </summary>
        /// <param name="left">A DynamoDB property whose value will be used as a left operand in concatenation.</param>
        /// <param name="right">A DynamoDB property whose value will be used as a right operand in concatenation.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If any of the specified properties does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right);
        
        /// <summary>
        /// Assigns the concatenation of specified list properties to the target list property.
        /// </summary>
        /// <param name="left">A DynamoDB property whose value will be used as a left operand in concatenation.</param>
        /// <param name="leftFallbackValue">The fallback value to be used if the specified left property doesn't exist in DynamoDB.</param>
        /// <param name="right">A DynamoDB property whose value will be used as a right operand in concatenation.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the right property does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right);
        
        /// <summary>
        /// Assigns the concatenation of specified list properties to the target list property.
        /// </summary>
        /// <param name="left">A DynamoDB property whose value will be used as a left operand in concatenation.</param>
        /// <param name="right">A DynamoDB property whose value will be used as a right operand in concatenation.</param>
        /// <param name="rightFallbackValue">The fallback value to be used if the specified right property doesn't exist in DynamoDB.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the left property does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);
        
        /// <summary>
        /// Assigns the concatenation of specified list properties to the target list property.
        /// </summary>
        /// <param name="left">A DynamoDB property whose value will be used as a left operand in concatenation.</param>
        /// <param name="leftFallbackValue">The fallback value to be used if the specified left property doesn't exist in DynamoDB.</param>
        /// <param name="right">A DynamoDB property whose value will be used as a right operand in concatenation.</param>
        /// <param name="rightFallbackValue">The fallback value to be used if the specified right property doesn't exist in DynamoDB.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);

        /// <summary>
        /// Assigns the concatenation of the specified property and value to the target list property.
        /// </summary>
        /// <param name="left">A DynamoDB property whose value will be used as a left operand in concatenation.</param>
        /// <param name="right">A value to be used as a right operand in concatenation.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the left property does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty right);

        /// <summary>
        /// Assigns the concatenation of the specified property and value to the target list property.
        /// </summary>
        /// <param name="left">A DynamoDB property whose value will be used as a left operand in concatenation.</param>
        /// <param name="leftFallbackValue">The fallback value to be used if the specified left property doesn't exist in DynamoDB.</param>
        /// <param name="right">A value to be used as a right operand in concatenation.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, TProperty right);

        /// <summary>
        /// Assigns the concatenation of the specified value and property to the target list property.
        /// </summary>
        /// <param name="left">A value to be used as a left operand in concatenation.</param>
        /// <param name="right">A DynamoDB property whose value will be used as a right operand in concatenation.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the right property does not exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder AssignConcat(TProperty left, Expression<Func<TEntity, TProperty>> right);

        /// <summary>
        /// Assigns the concatenation of the specified property and value to the target list property.
        /// </summary>
        /// <param name="left">A value to be used as a left operand in concatenation.</param>
        /// <param name="right">A DynamoDB property whose value will be used as a right operand in concatenation.</param>
        /// <param name="rightFallbackValue">The fallback value to be used if the specified right property doesn't exist in DynamoDB.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder AssignConcat(TProperty left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);

        /// <summary>
        /// Inserts the specified value into the target DynamoDB Set.
        /// </summary>
        /// <param name="value">A value to be inserted into the target DynamoDB Set property.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder Insert(TProperty value);
        
        /// <summary>
        /// Inserts the value of the specified property into the target DynamoDB Set.
        /// </summary>
        /// <param name="property">A DynamoDB property whose value will be inserted into the target DynamoDB Set.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder Insert(Expression<Func<TEntity, TProperty>> property);
        
        /// <summary>
        /// Inserts the value of the specified property into the target DynamoDB Set.
        /// </summary>
        /// <param name="property">A DynamoDB property whose value will be inserted into the target DynamoDB Set.</param>
        /// <param name="fallbackValue">The fallback value to be used if the specified property doesn't exist in DynamoDB.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder Insert(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue); // ADD #a if_not_exists(#b, :fallbackVal)

        /// <summary>
        /// Removes the target DynamoDB property.
        /// </summary>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder Remove(); // REMOVE #a
        // public TUpdateItemBuilder RemoveAt(int index); // REMOVE #a[0]

        /// <summary>
        /// Removes the specified value from the target DynamoDB Set.
        /// </summary>
        /// <param name="value">A value to be removed from the target DynamoDB Set.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder Remove(TProperty value); // DELETE #set_a :val
        
        /// <summary>
        /// Removes the value of the specified property from the target DynamoDB Set.
        /// </summary>
        /// <param name="property">A DynamoDB property whose value will be removed from the target DynamoDB Set.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        /// <remarks>
        /// If the specified property doesn't exist in DynamoDB, the operation will fail.
        /// </remarks>
        public TUpdateItemBuilder Remove(Expression<Func<TEntity, TProperty>> property); // DELETE #set_a #b
        
        /// <summary>
        /// Removes the value of the specified property from the target DynamoDB Set.
        /// </summary>
        /// <param name="property">A DynamoDB property whose value will be removed from the target DynamoDB Set.</param>
        /// <param name="fallbackValue">The fallback value to be used if the specified property doesn't exist in DynamoDB.</param>
        /// <returns>An UpdateItem operation builder.</returns>
        public TUpdateItemBuilder Remove(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue); // DELETE #set_a if_not_exists(#b, :fallbackVal)
    }

    internal class AttributeUpdate<TUpdateItemBuilder, TEntity, TProperty> : IAttributeUpdate<TUpdateItemBuilder, TEntity, TProperty> where TEntity : class where TUpdateItemBuilder : IUpdateItemBuilder<TUpdateItemBuilder>
    {
        private readonly TUpdateItemBuilder _requestBuilder;
        private readonly Expression<Func<TEntity, TProperty>> _expression;

        internal AttributeUpdate(TUpdateItemBuilder requestBuilder, Expression<Func<TEntity, TProperty>> expression)
        {
            _expression = expression;
            _requestBuilder = requestBuilder;
        }

        public TUpdateItemBuilder Assign(TProperty value) => _requestBuilder.Create(new UpdateAssign<TEntity, TProperty>(_expression, value), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder Assign(Expression<Func<TEntity, TProperty>> property) =>
            _requestBuilder.Create(new UpdateAssign<TEntity>(_expression, property), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder Assign(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue) =>
            _requestBuilder.Create(new UpdateAssignFallback<TEntity, TProperty>(_expression, property, fallbackValue), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignAttributesMath<TEntity>(_expression, AssignMathOperator.Plus, left, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue,
            Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignAttributesMathLeftFallback<TEntity, TProperty>(_expression, AssignMathOperator.Plus, left, leftFallbackValue, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right,
            TProperty rightFallbackValue) =>
            _requestBuilder.Create(new UpdateAssignAttributesMathRightFallback<TEntity, TProperty>(_expression, AssignMathOperator.Plus, left, right, rightFallbackValue), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue,
            Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue) => _requestBuilder.Create(
            new UpdateAssignAttributesMathFallback<TEntity, TProperty>(_expression, AssignMathOperator.Plus, left, leftFallbackValue, right, rightFallbackValue), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty right) =>
            _requestBuilder.Create(new UpdateAssignRightValueMath<TEntity, TProperty>(_expression, AssignMathOperator.Plus, left, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, TProperty right) =>
            _requestBuilder.Create(new UpdateAssignRightValueMathFallback<TEntity, TProperty>(_expression, AssignMathOperator.Plus, left, leftFallbackValue, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSum(TProperty left, Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignLeftValueMath<TEntity, TProperty>(_expression, AssignMathOperator.Plus, left, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSum(TProperty left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue) =>
            _requestBuilder.Create(new UpdateAssignLeftValueMathFallback<TEntity, TProperty>(_expression, AssignMathOperator.Plus, left, right, rightFallbackValue), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder Increment(Expression<Func<TEntity, TProperty>> right) => AssignSum(_expression, right);

        public TUpdateItemBuilder Increment(TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right) =>
            AssignSum(_expression, leftFallbackValue, right);

        public TUpdateItemBuilder Increment(TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue) =>
            AssignSum(_expression, leftFallbackValue, right, rightFallbackValue);

        public TUpdateItemBuilder Increment(TProperty right) => AssignSum(_expression, right);

        public TUpdateItemBuilder Increment(TProperty leftFallbackValue, TProperty right) => AssignSum(_expression, leftFallbackValue, right);

        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignAttributesMath<TEntity>(_expression, AssignMathOperator.Minus, left, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue,
            Expression<Func<TEntity, TProperty>> right) => _requestBuilder.Create(new UpdateAssignAttributesMathLeftFallback<TEntity, TProperty>(_expression, AssignMathOperator.Minus, left, leftFallbackValue, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right,
            TProperty rightFallbackValue) => _requestBuilder.Create(new UpdateAssignAttributesMathRightFallback<TEntity, TProperty>(_expression, AssignMathOperator.Minus, left, right, rightFallbackValue), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue,
            Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue) => _requestBuilder.Create(
            new UpdateAssignAttributesMathFallback<TEntity, TProperty>(_expression, AssignMathOperator.Minus, left, leftFallbackValue, right,
                rightFallbackValue), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty right) =>
            _requestBuilder.Create(new UpdateAssignRightValueMath<TEntity, TProperty>(_expression, AssignMathOperator.Minus, left, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, TProperty right) =>
            _requestBuilder.Create(new UpdateAssignRightValueMathFallback<TEntity, TProperty>(_expression, AssignMathOperator.Minus, left, leftFallbackValue, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSubtraction(TProperty left, Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignLeftValueMath<TEntity, TProperty>(_expression, AssignMathOperator.Minus, left, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSubtraction(TProperty left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue) =>
            _requestBuilder.Create(new UpdateAssignLeftValueMathFallback<TEntity, TProperty>(_expression, AssignMathOperator.Minus, left, right, rightFallbackValue), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder Decrement(Expression<Func<TEntity, TProperty>> right) => AssignSubtraction(_expression, right);

        public TUpdateItemBuilder Decrement(TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right) =>
            AssignSubtraction(_expression, leftFallbackValue, right);

        public TUpdateItemBuilder Decrement(TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue) =>
            AssignSubtraction(_expression, leftFallbackValue, right, rightFallbackValue);

        public TUpdateItemBuilder Decrement(TProperty right) => AssignSubtraction(_expression, right);

        public TUpdateItemBuilder Decrement(TProperty leftFallbackValue, TProperty right) => AssignSubtraction(_expression, leftFallbackValue, right);

        public TUpdateItemBuilder Append(Expression<Func<TEntity, TProperty>> property) => AssignConcat(_expression, property);

        public TUpdateItemBuilder Append(TProperty targetFallbackValue, Expression<Func<TEntity, TProperty>> property) =>
            AssignConcat(_expression, targetFallbackValue, property);

        public TUpdateItemBuilder Append(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue) =>
            AssignConcat(_expression, property, fallbackValue);

        public TUpdateItemBuilder Append(TProperty targetFallbackValue, Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue) =>
            AssignConcat(_expression, targetFallbackValue, property, fallbackValue);

        public TUpdateItemBuilder Append(TProperty value) => AssignConcat(_expression, value);
        public TUpdateItemBuilder Append(TProperty targetFallbackValue, TProperty right) => AssignConcat(_expression, targetFallbackValue, right);

        public TUpdateItemBuilder Prepend(Expression<Func<TEntity, TProperty>> property) => AssignConcat(property, _expression);

        public TUpdateItemBuilder Prepend(TProperty targetFallbackValue, Expression<Func<TEntity, TProperty>> property) =>
            AssignConcat(property, _expression, targetFallbackValue);

        public TUpdateItemBuilder Prepend(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue) =>
            AssignConcat(property, fallbackValue, _expression);

        public TUpdateItemBuilder Prepend(TProperty targetFallbackValue, Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue) =>
            AssignConcat(property, fallbackValue, _expression, targetFallbackValue);

        public TUpdateItemBuilder Prepend(TProperty value) => AssignConcat(value, _expression);
        public TUpdateItemBuilder Prepend(TProperty targetFallbackValue, TProperty value) => AssignConcat(value, _expression, targetFallbackValue);

        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignConcatAttributes<TEntity>(_expression, left, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue,
            Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignConcatAttributesLeftFallback<TEntity, TProperty>(_expression, left, leftFallbackValue, right),
                BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right,
            TProperty rightFallbackValue) =>
            _requestBuilder.Create(new UpdateAssignConcatAttributesRightFallback<TEntity, TProperty>(_expression, left, right, rightFallbackValue),
                BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue,
            Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue) => _requestBuilder.Create(
            new UpdateAssignConcatAttributesFallback<TEntity, TProperty>(_expression, left, leftFallbackValue, right, rightFallbackValue),
            BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty right) =>
            _requestBuilder.Create(new UpdateAssignConcatRightValue<TEntity, TProperty>(_expression, left, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, TProperty right) =>
            _requestBuilder.Create(new UpdateAssignConcatRightValueFallback<TEntity, TProperty>(_expression, left, leftFallbackValue, right),
                BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignConcat(TProperty left, Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignConcatLeftValue<TEntity, TProperty>(_expression, left, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignConcat(TProperty left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue) =>
            _requestBuilder.Create(new UpdateAssignConcatLeftValueFallback<TEntity, TProperty>(_expression, left, right, rightFallbackValue),
                BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder Insert(TProperty value) => _requestBuilder.Create(new UpdateInsert<TEntity, TProperty>(_expression, value), BuilderNodeType.AddUpdate);

        public TUpdateItemBuilder Insert(Expression<Func<TEntity, TProperty>> property) =>
            _requestBuilder.Create(new UpdateInsert<TEntity>(_expression, property), BuilderNodeType.AddUpdate);

        public TUpdateItemBuilder Insert(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue) =>
            _requestBuilder.Create(new UpdateInsertFallback<TEntity, TProperty>(_expression, property, fallbackValue), BuilderNodeType.AddUpdate);

        public TUpdateItemBuilder Remove() => _requestBuilder.Create(new UpdateRemove<TEntity>(_expression), BuilderNodeType.RemoveUpdate);

        // public TUpdateItemBuilder RemoveAt(int index) => _requestBuilder.Create(new UpdateRemoveAt<TEntity>(_expression, index), BuilderNodeType.RemoveUpdate);

        public TUpdateItemBuilder Remove(TProperty value) =>
            _requestBuilder.Create(new UpdateRemoveFromSet<TEntity, TProperty>(_expression, value), BuilderNodeType.DeleteUpdate);

        public TUpdateItemBuilder Remove(Expression<Func<TEntity, TProperty>> property) =>
            _requestBuilder.Create(new UpdateRemoveFromSet<TEntity>(_expression, property), BuilderNodeType.DeleteUpdate);

        public TUpdateItemBuilder Remove(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue) =>
            _requestBuilder.Create(new UpdateRemoveFromSetFallback<TEntity, TProperty>(_expression, property, fallbackValue), BuilderNodeType.DeleteUpdate);
    }

    internal abstract class UpdateBase
    {
        protected readonly Expression Expression;

        public UpdateBase(Expression expression)
        {
            Expression = expression;
        }

        // TODO: Get rid of hashset
        internal abstract void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount,
            DdbExpressionVisitor visitor);

        internal abstract void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor);

        // public static UpdateBase operator &(UpdateBase left, UpdateBase right) => Joiner.And(left, right);

        // public static UpdateBase operator |(UpdateBase left, UpdateBase right) => Joiner.Or(left, right);

        protected DdbConverter<TProperty> GetPropertyConverter<TProperty>(DdbExpressionVisitor visitor)
        {
            return (DdbConverter<TProperty>) visitor.ClassInfo.ConverterBase;

            // var propertyName = visitor.CachedAttributeNames[^1];
            // if (!visitor.ClassInfo.PropertiesMap.TryGetValue(propertyName, out var propertyInfo))
            //     throw new DdbException(
            //         $"Property {propertyName} does not exist in entity {visitor.ClassInfo.Type.Name} or it's not marked by {nameof(DynamoDbPropertyAttribute)} attribute");
            //
            // return ((DdbPropertyInfo<TProperty>) propertyInfo).Converter;
        }

        protected static void WriteIfNotExistsBlock<TEntity>(ref NoAllocStringBuilder builder, DdbExpressionVisitor visitor, Expression property,
            ref int valuesCount)
        {
            builder.Append("if_not_exists(");

            visitor.Visit<TEntity>(property);
            builder.Append(visitor.Builder);

            builder.Append(",:v");
            builder.Append(valuesCount++);
            builder.Append(')');
        }

        protected void WriteAttributeValue<TEntity, TProperty>(ref NoAllocStringBuilder builder, in DdbWriter writer, ref TProperty value,
            DdbExpressionVisitor visitor, ref int valuesCount)
        {
            visitor.Visit<TEntity>(Expression);
            var converter = GetPropertyConverter<TProperty>(visitor);

            builder.Append(":v");
            builder.Append(valuesCount++);
            writer.JsonWriter.WritePropertyName(builder.GetBuffer());
            converter.Write(in writer, ref value);
        }
    }
}