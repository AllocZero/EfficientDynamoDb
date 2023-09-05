---
id: delete-item-builder
title: DeleteItem Request Builder
sidebar_label: DeleteItem
slug: ../../api-reference/builders/delete-item
---

## Overview

This section describes the EfficientDynamoDb API for building the `DeleteItem` request.

The `IDeleteItemEntityRequestBuilder<TEntity>` interface provides a builder pattern for constructing a `DeleteItem` operation in DynamoDB.
It is designed to work with a database entity of type `TEntity`.

Use the `DynamoDbContext.DeleteItem()` method to get the builder:

```csharp
var builder = ddbContext.DeleteItem();
```

For the `DeleteItem` request you only required to specify primary key using the `WithPrimaryKey` method.
All other builder methods are optional and can be omitted.
In this case, DynamoDB will use the default behavior.

## DeleteItem Configuration

### WithPrimaryKey (with only Partition key) {#withprimarykey-pk}

Specifies the partition key of the item to delete.

```csharp
IDeleteItemEntityRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);
```

:::info
It is required to specify the primary key for every DeleteItem request.
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

Specifies the partition and sort keys of the item to delete.

```csharp
IDeleteItemEntityRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
```

:::info
It is required to specify the primary key for every DeleteItem request.
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

### WithCondition (Explicit condition) {#withcondition}

Specifies condition for the DeleteItem operation.

```csharp
IDeleteItemEntityRequestBuilder<TEntity> WithCondition(FilterBase condition);
```

#### Parameters {#withcondition-parameters}

- `condition`: The condition that is used to determine whether the DeleteItem operation should succeed or fail.
Refer to our [condition expression building guide](../../dev_guide/high_level/conditions.md) to learn how to build a condition in EfficientDynamoDb.

#### Example {#withcondition-example}

```csharp
var expr = Condition<EntityClass>.On(item => item.FirstName).EqualTo("John");
builder = builder.WithCondition(expr);
```

### WithCondition (Function condition) {#withcondition-func}

Specifies the condition function for the DeleteItem operation.

```csharp
IDeleteItemEntityRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);
```

#### Parameters {#withcondition-func-parameters}

- `conditionSetup`: The condition function that is used to determine whether the DeleteItem operation should succeed or fail.
Refer to our [condition expression building guide](../../dev_guide/high_level/conditions.md) to learn how to build a condition in EfficientDynamoDb.

#### Example {#withcondition-func-example}

```csharp
builder = builder.WithCondition(
    cond => cond.On(item => item.FirstName).EqualTo("John")
);
```

### WithReturnValues {#withreturnvalues}

Specifies the attributes to include in the response.

```csharp
IDeleteItemEntityRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues);
```

If not specified, no values are returned in the response.

#### Parameters {#withreturnvalues-parameters}

- `returnValues`: [ReturnValues](../options/return-values.md) enum option.
Only `ReturnValues.None` and `ReturnValues.AllOld` are valid for DeleteItem operation.
Passing `ReturnValues.None` is equivalent to not using the `WithReturnValues` method at all.

#### Example {#withreturnvalues-example}

```csharp
builder = builder.WithReturnValues(ReturnValues.AllOld);
```

### WithReturnValuesOnConditionCheckFailure {#withreturnvaluesonconditioncheckfailure}

Specifies how to handle return values if the operation fails.

```csharp
IDeleteItemEntityRequestBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure option)
```

If not specified, no values are returned in case of condition check failure.

#### Parameters {#withreturnvaluesonconditioncheckfailure-parameters}

- `option`: Option for handling return values on condition check failure.

#### Example {#withreturnvaluesonconditioncheckfailure-example}

```csharp
builder = builder.WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure.AllOld);
```

### WithReturnConsumedCapacity {#withreturnconsumedcapacity}

Specifies the consumed capacity details to include in the response.

```csharp
IDeleteItemEntityRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
```

If not specified, no consumed capacity info is returned in the response.

#### Parameters {#withreturnconsumedcapacity-parameters}

- `returnConsumedCapacity`: The type of consumed capacity information to return.
Setting it to `ReturnConsumedCapacity.None` is equivalent to not using the `ReturnConsumedCapacity()` method at all.

#### Example {#withreturnconsumedcapacity-example}

```csharp
builder = builder.WithReturnConsumedCapacity(ReturnConsumedCapacity.Total);
```

### AsDocument {#asdocument}

Represents the returned item as a `Document`.

This method changes the type of the builder.

```csharp
IDeleteItemDocumentRequestBuilder<TEntity> AsDocument();
```

#### Example {#asdocument-example}

```csharp
var documentBuilder = builder.AsDocument();
```

After execution, this `DeleteItem` request will return the `Document` instead of the original entity of the builder.

## DeleteItem Execution

There are 2 versions of every query execution method: regular and document.
All versions have same parameters, the only difference is entity type returned value:

- In most cases, the original entity `TEntity` is returned.
- If `AsDocuments()` was used, the execution method will contain the entity type of `Document`.

For simplicity, this document covers only regular version of execution methods.

### ExecuteAsync {#execute}

Executes the DeleteItem operation.

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

Executes the DeleteItem operation and returns the item before the deletion.

The item is returned as it appeared before the DeleteItem operation, but only if [`WithReturnValues`](#withreturnvalues) with `ReturnValues.AllOld` was specified in the request chain.
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

Executes the DeleteItem operation and returns the deserialized response.
Besides the item, this response contains `ConsumedCapacity` property if [`WithReturnConsumedCapacity`](#withreturnconsumedcapacity) was used.

```csharp
Task<DeleteItemEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default);
```

#### Example {#toresponse-example}

```csharp
var response = await builder.ToResponseAsync();
var item = response.Item;
var consumedCapacity = response.ConsumedCapacity;
```
