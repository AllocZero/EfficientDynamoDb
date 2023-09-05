---
id: put-item-builder
title: PutItem Request Builder
sidebar_label: PutItem
slug: ../../api-reference/builders/put-item
---

## Overview

This section describes the EfficientDynamoDb API for building the `PutItem` request.

The `IPutItemRequestBuilder` interface provides a builder pattern for constructing a `PutItem` operation in DynamoDB.

Use the `DynamoDbContext.PutItem()` method to get the builder:

```csharp
var builder = ddbContext.PutItem();
```

For the `PutItem` request you only required to item to save using the `WithItem` method.
All other builder methods are optional and can be omitted.
In this case, DynamoDB will use the default behavior.

## PutItem General Configuration

The following configuration methods are available at every step of the PutItem request configuration, even before specifying the item to save.

### WithReturnValues {#withreturnvalues}

Specifies the attributes to include in the response.

```csharp
IPutItemRequestBuilder WithReturnValues(ReturnValues returnValues);
```

If not specified, no values are returned in the response.

#### Parameters {#withreturnvalues-parameters}

- `returnValues`: [ReturnValues](../options/return-values.md) enum option.
Only `ReturnValues.None` and `ReturnValues.AllOld` are valid for PutItem operation.
Passing `ReturnValues.None` is equivalent to not using the `WithReturnValues` method at all.

#### Example {#withreturnvalues-example}

```csharp
builder = builder.WithReturnValues(ReturnValues.AllOld);
```

### WithReturnConsumedCapacity {#withreturnconsumedcapacity}

Specifies the consumed capacity details to include in the response.

```csharp
IPutItemRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
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
IPutItemRequestBuilder WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure option);
```

If not specified, no values are returned in case of condition check failure.

#### Parameters {#withreturnvaluesonconditioncheckfailure-parameters}

- `option`: Option for handling return values on condition check failure.

#### Example {#withreturnvaluesonconditioncheckfailure-example}

```csharp
builder = builder.WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure.AllOld);
```

### WithCondition (Explicit condition) {#withcondition}

Specifies condition for the PutItem operation.

```csharp
IPutItemRequestBuilder WithCondition(FilterBase condition);
```

#### Parameters {#withcondition-parameters}

- `condition`: The condition that is used to determine whether the PutItem operation should succeed or fail.
Refer to our [condition expression building guide](../../dev_guide/high_level/conditions.md) to learn how to build a condition in EfficientDynamoDb.

#### Example {#withcondition-example}

```csharp
var expr = Condition<EntityClass>.On(item => item.Pk).NotExists();
builder = builder.WithCondition(expr);
```

### WithItem {#withitem}

Specifies the item to save.

This method returns a different type of the builder ([entity builder](#putitem-entity-configuration) covered below) to preserve the projection type.
Additionally, this entity builder provides more configuration methods that require knowledge about the item type.

```csharp
IPutItemEntityRequestBuilder<TEntity> WithItem<TEntity>(TEntity item) where TEntity : class;
```

#### Parameters {#withitem-parameters}

- `item`: The item to save.

#### Example {#withitem-example}

```csharp
var entityBuilder = builder.WithItem(new EntityClass());
```

## PutItem Entity Configuration {#putitem-entity-configuration}

This section covers all the additional configuration methods provided in `IPutItemEntityRequestBuilder<TEntity>` that you get after specifying the item to save using `WithItem` method.

### WithCondition (Function condition) {#withcondition-func}

Specifies the condition function for the PutItem operation.

```csharp
IPutItemEntityRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);
```

#### Parameters {#withcondition-func-parameters}

- `conditionSetup`: The condition function that is used to determine whether the PutItem operation should succeed or fail.
Refer to our [condition expression building guide](../../dev_guide/high_level/conditions.md) to learn how to build a condition in EfficientDynamoDb.

#### Example {#withcondition-func-example}

```csharp
builder = builder.WithCondition(
    cond => cond.On(item => item.Pk).NotExists()
);
```

### AsDocument {#asdocument}

Represents the returned item as a `Document`.

This method changes the type of the builder.

```csharp
IPutItemDocumentRequestBuilder<TEntity> AsDocument();
```

#### Example {#asdocument-example}

```csharp
var documentBuilder = builder.AsDocument();
```

After execution, this `PutItem` request will return the `Document` instead of the original entity of the builder.

## PutItem Execution

There are 2 versions of every query execution method: regular and document.
All versions have same parameters, the only difference is entity type returned value:

- In most cases, the original entity `TEntity` is returned.
- If `AsDocuments()` was used, the execution method will contain the entity type of `Document`.

For simplicity, this document covers only regular version of execution methods.

### ExecuteAsync {#execute}

Executes the PutItem operation.

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

Executes the PutItem operation and returns the item before the update.

The item is returned as it appeared before the PutItem operation, but only if [`WithReturnValues`](#withreturnvalues) with `ReturnValues.AllOld` was specified in the request chain.
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

Executes the PutItem operation and returns the deserialized response.
Besides the item, this response contains `ConsumedCapacity` property if [`WithReturnConsumedCapacity`](#withreturnconsumedcapacity) was used.

```csharp
Task<PutItemEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default);
```

#### Example {#toresponse-example}

```csharp
var response = await builder.ToResponseAsync();
var item = response.Item;
var consumedCapacity = response.ConsumedCapacity;
```
