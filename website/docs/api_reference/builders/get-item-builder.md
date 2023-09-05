---
id: get-item-builder
title: GetItem Request Builder
sidebar_label: GetItem
slug: ../../api-reference/builders/get-item
---

## Overview

This section describes the EfficientDynamoDb API for building the `GetItem` request.

The `IGetItemEntityRequestBuilder<TEntity>` interface provides a builder pattern for constructing a `GetItem` operation in DynamoDB.
It is designed to work with a database entity of type `TEntity`.

Use the `DynamoDbContext.GetItem<TEntity>()` method to get the builder:

```csharp
var builder = ddbContext.GetItem<EntityClass>();
```

For the `GetItem` request you only required to specify primary key using the `WithPrimaryKey` method.
All other builder methods are optional and can be omitted.
In this case, DynamoDB will use the default behavior.

## GetItem Configuration

### WithPrimaryKey (with only Partition key) {#withprimarykey-pk}

Specifies the partition key of the item to get.

```csharp
IGetItemEntityRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);
```

:::info
It is required to specify the primary key for every GetItem request.
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

Specifies the partition and sort keys of the item to get.

```csharp
IGetItemEntityRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
```

:::info
It is required to specify the primary key for every GetItem request.
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

### WithConsistentRead {#withconsistentread}

Specifies whether to use a consistent read in the GetItem operation.

```csharp
IGetItemEntityRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead);
```

If not specified, DynamoDB will use eventually consistent read.

#### Parameters {#withconsistentread-parameters}

- `useConsistentRead`: Set this to `true` if you want a consistent read.
Otherwise, set it to `false`.
Setting it to `false` is equivalent to not using the `WithConsistentRead()` method at all.

#### Example {#withconsistentread-example}

```csharp
builder = builder.WithConsistentRead(true);
```

### ReturnConsumedCapacity {#returnconsumedcapacity}

Specifies the consumed capacity details to include in the response.

```csharp
IGetItemEntityRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);
```

If not specified, no consumed capacity info is returned in the response.

#### Parameters {#returnconsumedcapacity-parameters}

- `consumedCapacityMode`: The type of consumed capacity information to return.
Setting it to `ReturnConsumedCapacity.None` is equivalent to not using the `ReturnConsumedCapacity()` method at all.

#### Example {#returnconsumedcapacity-example}

```csharp
builder = builder.ReturnConsumedCapacity(ReturnConsumedCapacity.Total);
```

### AsProjection (With Type) {#asprojection-type}

Projects the retrieved item to the specified type.
Only properties present in `TProjection` will be retrieved.

This method returns a different type of the builder to preserve the projection type.
In case of chained calls and/or using `var` to save builder to a variable, the change of returned type may be unnoticeable.
This is by design and you should be able to mix regular and projected builders.

```csharp
IGetItemEntityRequestBuilder<TEntity, TProjection> AsProjection<TProjection>() where TProjection : class;
```

#### Example {#asprojection-type-example}

Since the change of returned builder type, it can't be assigned to the same variable.

```csharp
var projectedBuilder = builder.AsProjection<ProjectedEntity>();
```

### AsProjection (With Attributes) {#asprojection-type-attributes}

Projects the retrieved item to the specified type, but only retrieves the properties specified in `properties` parameter.
Other properties will have default values.

Similarly to [`AsProjection<TProjection>()`](#asprojection-type), this method returns a different type of the builder to preserve the projection type with all the previously explained consequences.

```csharp
IGetItemEntityRequestBuilder<TEntity, TProjection> AsProjection<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
```

#### Parameters {#asprojection-type-attributes-parameters}

- `properties`: The attributes to project.

#### Example {#asprojection-type-attributes-example}

```csharp
var projectedBuilder = builder.AsProjection<ProjectedEntity>(
    x => x.SomeProperty,
    x => x.AnotherProperty
);
```

After execution, this `GetItem` request will return instance of `ProjectedEntity` with only `SomeProperty` and `AnotherProperty` set.
All other properties will have default values.

### WithProjectedAttributes {#withprojectedattributes}

Specifies the attributes to project in the retrieved item.
Only properties specified in `properties` will be retrieved. Other properties will have default values.

Contrary to `AsProjection` methods, `WithProjectedAttributes` doesn't change the type of returned entity and builder.

```csharp
IGetItemEntityRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties);
```

#### Parameters {#withprojectedattributes-parameters}

- `properties`: The attributes to project.

#### Example {#withprojectedattributes-example}

```csharp
builder = builder.WithProjectedAttributes(
    x => x.SomeProperty,
    x => x.AnotherProperty
);
```

After execution, this `GetItem` request will return the original entity of the builder (in this example it's `EntityClass`) with only `SomeProperty` and `AnotherProperty` set.
All other properties will have default values.

### AsDocument {#asdocument}

Represents the returned item as a `Document`.

Similarly to [`AsProjection<TProjection>()`](#asprojection-type), this operation returns different type of builder with all the previously explained consequences.

```csharp
IGetItemDocumentRequestBuilder<TEntity> AsDocument();
```

#### Example {#asdocument-example}

```csharp
var documentBuilder = builder.AsDocument();
```

After execution, this `GetItem` request will return the `Document` instead of the original entity of the builder.

## GetItem Execution

There are 3 versions of every GetItem execution method: regular, projected, and document.
All versions have same parameters, the only difference is entity type returned value:

- In most cases, the original entity `TEntity` is returned.
- If `AsProjection<TProjection>()` was used during the configuration, the execution method will contain the entity type of `TProjection`.
- If `AsDocument()` was used, the execution method will contain the entity type of `Document`.

In all cases, the result will be `null` if the item does not exist.

For simplicity, this document covers only regular version of execution methods.

### ToItemAsync {#toitem}

Executes the GetItem operation asynchronously and returns the item.

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

Executes the GetItem operation asynchronously and returns the deserialized response.
Besides the item, this response contains `ConsumedCapacity` property if [`ReturnConsumedCapacity`](#returnconsumedcapacity) was used.

```csharp
Task<GetItemEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default);
```

#### Example {#toresponse-example}

```csharp
var response = await builder.ToResponseAsync();
var item = response.Item;
var consumedCapacity = response.ConsumedCapacity;
```
