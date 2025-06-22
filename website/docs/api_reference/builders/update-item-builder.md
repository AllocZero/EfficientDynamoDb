---
id: update-item-builder
title: UpdateItem Request Builder
sidebar_label: UpdateItem
slug: ../../api-reference/builders/update-item
---

## Overview

This section describes the EfficientDynamoDb API for building the `UpdateItem` request.

The `UpdateEntityRequestBuilder<TEntity>` interface provides a builder pattern for constructing a `UpdateItem` operation in DynamoDB.

Use the `DynamoDbContext.UpdateItem()` method to get the builder:

```csharp
var builder = ddbContext.UpdateItem();
```

For the `UpdateItem` request you only required to specify primary key using the `WithPrimaryKey` method.
All other builder methods are optional and can be omitted.
In this case, DynamoDB will use the default behavior.

## UpdateItem Configuration

### WithPrimaryKey (with only Partition key) {#withprimarykey-pk}

Specifies the partition key of the item to update.

```csharp
IUpdateEntityRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);
```

:::info
It is required to specify the primary key for every UpdateItem request.
:::

:::caution
Use this method if the table has only a partition key. If the table has both partition and sort keys, use [`WithPrimaryKey<TPk, TSk>`](#withprimarykey-pk-sk) instead.
:::

#### Parameters {#withprimarykey-pk-parameters}

- `pk`: The partition key of the item.

#### Example {#withprimarykey-pk-example}

```csharp
builder = builder.WithPrimaryKey("partitionKey");
```

### WithPrimaryKey (with both Partition and Sort keys) {#withprimarykey-pk-sk}

Specifies the partition and sort keys of the item to update.

```csharp
IDeleteItemEntityRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
```

:::info
It is required to specify the primary key for every UpdateItem request.
:::

:::caution
Use this method only if the table has both partition and sort keys.
If the table only has a partition key, use [`WithPrimaryKey<TPk>`](#withprimarykey-pk) instead.
:::

#### Parameters {#withprimarykey-pk-sk-parameters}

- `pk`: The partition key of the item.
- `sk`: The sort key of the item.

#### Example {#withprimarykey-pk-sk-example}

```csharp
builder = builder.WithPrimaryKey("partitionKey", "sortKey");
```

### WithReturnValues {#withreturnvalues}

Specifies the attributes to include in the response.

```csharp
UpdateEntityRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues);
```

If not specified, no values are returned in the response.

#### Parameters {#withreturnvalues-parameters}

- `returnValues`: [ReturnValues](../options/return-values.md) enum option.
All values of `ReturnValues` enum are valid for UpdateItem operation.
Passing `ReturnValues.None` is equivalent to not using the `WithReturnValues` method at all.

#### Example {#withreturnvalues-example}

```csharp
builder = builder.WithReturnValues(ReturnValues.UpdatedOld);
```

### WithReturnConsumedCapacity {#withreturnconsumedcapacity}

Specifies the consumed capacity details to include in the response.

```csharp
UpdateEntityRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
```

If not specified, no consumed capacity info is returned in the response.

#### Parameters {#withreturnconsumedcapacity-parameters}

- `returnConsumedCapacity`: The type of consumed capacity information to return.
Setting it to `ReturnConsumedCapacity.None` is equivalent to not using the `ReturnConsumedCapacity()` method at all.

#### Example {#withreturnconsumedcapacity-example}

```csharp
builder = builder.WithReturnConsumedCapacity(ReturnConsumedCapacity.Total);
```

### WithReturnValuesOnConditionCheckFailure {#withreturnvaluesonconditioncheckfailure}

Specifies how to handle return values if the operation fails.

```csharp
UpdateEntityRequestBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure option);
```

If not specified, no values are returned in case of condition check failure.

#### Parameters {#withreturnvaluesonconditioncheckfailure-parameters}

- `option`: Option for handling return values on condition check failure.

#### Example {#withreturnvaluesonconditioncheckfailure-example}

```csharp
builder = builder.WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure.AllOld);
```

### WithCondition (Explicit condition) {#withcondition}

Specifies condition for the UpdateItem operation.

```csharp
UpdateEntityRequestBuilder<TEntity> WithCondition(FilterBase condition);
```

#### Parameters {#withcondition-parameters}

- `condition`: The condition that is used to determine whether the UpdateItem operation should succeed or fail.
Refer to our [condition expression building guide](../../dev_guide/high_level/conditions.md) to learn how to build a condition in EfficientDynamoDb.

#### Example {#withcondition-example}

```csharp
var expr = Condition<EntityClass>.On(item => item.Pk).Exists();
builder = builder.WithCondition(expr);
```

### WithCondition (Function condition) {#withcondition-func}

Specifies the condition function for the UpdateItem operation.

```csharp
IUpdateEntityRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> filterSetup);
```

#### Parameters {#withcondition-func-parameters}

- `filterSetup`: The condition function that is used to determine whether the UpdateItem operation should succeed or fail.
Refer to our [condition expression building guide](../../dev_guide/high_level/conditions.md) to learn how to build a condition in EfficientDynamoDb.

#### Example {#withcondition-func-example}

```csharp
builder = builder.WithCondition(
    cond => cond.On(item => item.FirstName).EqualTo("John")
);
```

### AsDocument {#asdocument}

Represents the returned item as a `Document`.

This method changes the type of the builder.

```csharp
IUpdateItemDocumentRequestBuilder<TEntity> AsDocument();
```

#### Example {#asdocument-example}

```csharp
var documentBuilder = builder.AsDocument();
```

After execution, this `UpdateItem` request will return the `Document` instead of the original entity of the builder.

### SuppressThrowing {#suppressthrowing}

Prevents the `UpdateItem` operation from throwing an exception in case of any failure. Instead, the execution methods will return an `OpResult<T>` that encapsulates either a successful result or an error.

This method returns a different type of the builder to indicate that exception suppression is active.

```csharp
ISuppressedUpdateItemEntityRequestBuilder<TEntity> SuppressThrowing();
```

#### Example {#suppressthrowing-example}

```csharp
var result = await builder.SuppressThrowing().ToItemAsync();
if (result.IsSuccess)
{
    var item = result.Value;
    // process item
}
else
{
    var exception = result.Exception;
    // handle error
}
```

### On {#on}

Specifies the attribute to be updated in the DynamoDB item.

To update multiple attributes, call this method multiple times.
For a detailed walkthrough and examples, refer to the [update expression developer guide](../../dev_guide/high_level/update-expression.md).

```csharp
IAttributeUpdate<IUpdateEntityRequestBuilder<TEntity>, TEntity, TProperty> On<TProperty>(Expression<Func<TEntity, TProperty>> expression);
```

#### Parameters {#on-parameters}

- `expression`: An expression identifying the attribute to be updated.

#### Example {#on-example}

```csharp
builder = builder.On(item => item.FirstName).Assign("John")
                 .On(item => item.UpdatesCount).Increment(1);
```

## UpdateItem Execution

There are 2 versions of every query execution method: regular and document.
All versions have same parameters, the only difference is entity type returned value:

- In most cases, the original entity `TEntity` is returned.
- If `AsDocuments()` was used, the execution method will contain the entity type of `Document`.
- If `SuppressThrowing()` was used, the execution method will return an `OpResult<T>` where `T` is one of the types above.

For simplicity, this document covers only regular version of execution methods.

### ExecuteAsync {#execute}

Executes the UpdateItem operation.

```csharp
Task ExecuteAsync(CancellationToken cancellationToken = default);
```

#### Parameters {#execute-parameters}

- `cancellationToken`: Token that can be used to cancel the task.

#### Example {#execute-example}

```csharp
await builder.ExecuteAsync();
```

### ToItemAsync {#toitem}

Executes the UpdateItem operation and returns the item before the update.

The item is returned as it appeared before the UpdateItem operation, but only if [`WithReturnValues`](#withreturnvalues) was specified in the request chain.
Otherwise, `null` is returned.

```csharp
Task<TEntity?> ToItemAsync(CancellationToken cancellationToken = default);
```

#### Parameters {#toitem-parameters}

- `cancellationToken`: Token that can be used to cancel the task.

#### Example {#toitem-example}

```csharp
var item = await builder.ToItemAsync();
```

### ToResponseAsync {#toresponse}

Executes the UpdateItem operation and returns the deserialized response.
Besides the item, this response contains `ConsumedCapacity` property if [`WithReturnConsumedCapacity`](#withreturnconsumedcapacity) was used.

```csharp
Task<UpdateItemEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default);
```

#### Example {#toresponse-example}

```csharp
var response = await builder.ToResponseAsync();
var item = response.Item;
var consumedCapacity = response.ConsumedCapacity;
```
