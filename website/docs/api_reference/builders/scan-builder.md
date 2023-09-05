---
id: scan-builder
title: Scan Request Builder
sidebar_label: Scan
slug: ../../api-reference/builders/scan
---

## Overview

This section describes the EfficientDynamoDb API for building the `Scan` request.

The `IScanEntityRequestBuilder<TEntity>` interface provides a builder pattern for constructing a `Scan` operation in DynamoDB.
It is designed to work with a database entity of type `TEntity`.

Use the `DynamoDbContext.Scan<TEntity>()` method to get the builder:

```csharp
var builder = ddbContext.Scan<EntityClass>();
```

There are no required configuration methods for the `Scan` request.
All builder methods are optional and can be omitted.
In this case, DynamoDB will use the default behavior.

## Scan Configuration

### FromIndex {#fromindex}

Specifies the index name to use for the Scan operation.
Can be used for both GSI and LSI.

```csharp
IScanEntityRequestBuilder<TEntity> FromIndex(string indexName);
```

#### Parameters {#fromindex-parameters}

- `indexName`: Name of the index.

#### Example {#fromindex-example}

```csharp
builder = builder.FromIndex("indexName");
```

### WithConsistentRead {#withconsistentread}

Specifies whether to use a consistent read in the Scan operation.

```csharp
IScanEntityRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead);
```

If not specified, DynamoDB will use eventually consistent read.

:::caution
Consistent reads are not supported for Scan requests against GSIs.
:::

#### Parameters {#withconsistentread-parameters}

- `useConsistentRead`: Set this to `true` if you want a consistent read.
Otherwise, set it to `false`.
Setting it to `false` is equivalent to not using the `WithConsistentRead()` method at all.

#### Example {#withconsistentread-example}

```csharp
builder = builder.WithConsistentRead(true);
```

### WithLimit {#withlimit}

Specifies the maximum number of items to query.

```csharp
IScanEntityRequestBuilder<TEntity> WithLimit(int limit);
```

:::info
The actual number of items returned may be less than specified when filter expression is present or if the scan operation exceeds the 1 MB limit or retrieved data.
Refer to [AWS developer guide](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Scan.html) for more information.
:::

#### Parameters {#withlimit-parameters}

- `limit`: Maximum number of items to query.

#### Example {#withlimit-parameters}

```csharp
builder = builder.WithLimit(50);
```

### ReturnConsumedCapacity {#returnconsumedcapacity}

Specifies the consumed capacity details to include in the response.

```csharp
IScanEntityRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);
```

If not specified, no consumed capacity info is returned in the response.

#### Parameters {#returnconsumedcapacity-parameters}

- `consumedCapacityMode`: The type of consumed capacity information to return.
Setting it to `ReturnConsumedCapacity.None` is equivalent to not using the `ReturnConsumedCapacity()` method at all.

#### Example {#returnconsumedcapacity-example}

```csharp
builder = builder.ReturnConsumedCapacity(ReturnConsumedCapacity.Total);
```

### WithSelectMode {#withselectmode}

Specify the select mode for the Scan operation.
It affects what data will be returned in response.

```csharp
IScanEntityRequestBuilder<TEntity> WithSelectMode(Select selectMode);
```

#### Parameters {#withselectmode-parameters}

- `selectMode`: Select mode to use for the query operation.
Learn more about possible modes [here](../options/select-mode).

:::tip
Use one of the projection methods instead of specifying `SpecificAttributes` mode:

- [WithProjectedAttributes](#withprojectedattributes)
- [`AsProjections` with type](#asprojections-type)
- [`AsProjections` with attributes](#asprojections-type-attributes)
:::

#### Example {#withselectmode-example}

```csharp
builder = builder.WithSelectMode(Select.Count);
```

### BackwardSearch {#backwardsearch}

Specifies if backward search should be used.

```csharp
IScanEntityRequestBuilder<TEntity> BackwardSearch(bool useBackwardSearch);
```

#### Parameters {#backwardsearch-parameters}

- `useBackwardSearch`: `true`, if backward search should be used. Otherwise, `false`.

#### Example {#backwardsearch-example}

```csharp
builder = builder.BackwardSearch(true);
```

### WithFilterExpression (Explicit condition) {#withfilterexpression}

Specifies the filter expression for the Scan operation.

```csharp
IScanEntityRequestBuilder<TEntity> WithFilterExpression(FilterBase filterExpressionBuilder);
```

#### Parameters {#withfilterexpression-parameters}

- `filterExpressionBuilder`: Filter expression for query. Refer to our [condition expression building guide](../../dev_guide/high_level/conditions.md) to learn how to build key expression in EfficientDynamoDb.

#### Example {#withfilterexpression-example}

```csharp
var expr = Condition<EntityClass>.On(item => item.FirstName).EqualTo("John");
builder = builder.WithFilterExpression(expr);
```

### WithFilterExpression (Function condition) {#withfilterexpression-func}

Specifies the filter expression function for the Scan operation.

```csharp
IScanEntityRequestBuilder<TEntity> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup);
```

#### Parameters {#withfilterexpression-func-parameters}

- `filterSetup`: Filter expression function for query. Refer to our [condition expression building guide](../../dev_guide/high_level/conditions.md) to learn how to build key expression in EfficientDynamoDb.

#### Example {#withfilterexpression-func-example}

```csharp
builder = builder.WithFilterExpression(
    cond => cond.On(item => item.FirstName).EqualTo("John")
);
```

### WithPaginationToken {#withpaginationtoken}

Specifies the pagination token for the Scan operation.

```csharp
IScanEntityRequestBuilder<TEntity> WithPaginationToken(string? paginationToken);
```

#### Parameters {#withpaginationtoken-parameters}

- `paginationToken`: The pagination token to use.
Passing `null` results in the same behavior as not specifying the pagination token at all.

#### Example {#withpaginationtoken-example}

```csharp
builder = builder.WithPaginationToken("yourToken");
```

### AsProjections (With type) {#asprojections-type}

Projects the retrieved items to the specified type.
Only properties present in `TProjection` will be retrieved.

This method returns a different type of the builder to preserve the projection type.
In case of chained calls and/or using `var` to save builder to a variable, the change of returned type may be unnoticeable.
This is by design and you should be able to mix regular and projected builders.

```csharp
IScanEntityRequestBuilder<TEntity, TProjection> AsProjections<TProjection>() where TProjection : class;
```

#### Example {#asprojections-type-example}

Since the change of returned builder type, it can't be assigned to the same variable.

```csharp
var projectedBuilder = builder.AsProjections<ProjectedEntity>();
```

### AsProjections (With attributes) {#asprojections-type-attributes}

Projects the retrieved items to the specified type, but only retrieves the properties specified in `properties` parameter.
Other properties will have default values.

Similarly to [`AsProjection<TProjection>()`](#asprojections-type), this method returns a different type of the builder to preserve the projection type with all the previously explained consequences.

```csharp
IScanEntityRequestBuilder<TEntity, TProjection> AsProjections<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
```

#### Parameters {#asprojections-type-attributes-parameters}

- `properties`: The attributes to project.

#### Example {#asprojections-type-attributes-example}

```csharp
var projectedBuilder = builder.AsProjections<ProjectedEntity>(
    x => x.SomeProperty,
    x => x.AnotherProperty
);
```

After execution, this `Scan` request will return instance of `ProjectedEntity` with only `SomeProperty` and `AnotherProperty` set.
All other properties will have default values.

### WithProjectedAttributes {#withprojectedattributes}

Specifies the attributes to project in the retrieved item.
Only properties specified in `properties` will be retrieved. Other properties will have default values.

Contrary to `AsProjection` methods, `WithProjectedAttributes` doesn't change the type of returned entity and builder.

```csharp
IScanEntityRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties);
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

After execution, this `Scan` request will return the original entity of the builder (in this example it's `EntityClass`) with only `SomeProperty` and `AnotherProperty` set.
All other properties will have default values.

### AsDocumets {#asdocuments}

Represents the returned items as `Document`.

Similarly to [`AsProjection<TProjection>()`](#asprojection-type), this operation returns different type of builder with all the previously explained consequences.

```csharp
IScanDocumentRequestBuilder<TEntity> AsDocuments();
```

#### Example {#asdocuments-example}

```csharp
var documentBuilder = builder.AsDocuments();
```

## Scan Execution

There are 3 versions of every query execution method: regular, projected, and document.
All versions have same parameters, the only difference is entity type returned value:

- In most cases, the original entity `TEntity` is returned.
- If `AsProjection<TProjection>()` was used during the configuration, the execution method will contain the entity type of `TProjection`.
- If `AsDocuments()` was used, the execution method will contain the entity type of `Document`.

For simplicity, this document covers only regular version of execution methods.

### ToPageAsync {#topage}

Executes the Scan operation and returns the page of data with pagination token.

```csharp
Task<PagedResult<TEntity>> ToPageAsync(CancellationToken cancellationToken = default);
```

#### Parameters {#topage-parameters}

- `cancellationToken`: Token that can be used to cancel the task.

#### Example {#topage-example}

```csharp
var page = await builder.ToPageAsync();
var items = page.Items;
var paginationToken = page.PaginationToken;
```

### ToResponseAsync {#toresponse}

Executes the Scan operation and returns the deserialized response.

```csharp
Task<ScanEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default);
```

#### Parameters {#toresponse-parameters}

- `cancellationToken`: Token that can be used to cancel the task.

#### Example {#toresponse-example}

```csharp
var response = await builder.ToResponseAsync();
```

### ToAsyncEnumerable {#toasyncenumerable}

Executes the Scan operation and returns the result as an async enumerable, with each item in the sequence representing a single retrieved item.

```csharp
IAsyncEnumerable<TEntity> ToAsyncEnumerable();
```

#### Example {#toasyncenumerable-example}

```csharp
await foreach(var item in builder.ToAsyncEnumerable())
{
    // Do something.
}
```

### ToPagedAsyncEnumerable {#topagedasyncenumerable}

Executes the Scan operation and returns the result as an async enumerable, with each item in the sequence representing a page of DynamoDB items.

```csharp
IAsyncEnumerable<IReadOnlyList<TEntity>> ToPagedAsyncEnumerable();
```

#### Example {#topagedasyncenumerable-example}

```csharp
await foreach(var page in builder.ToAsyncEnumerable())
{
    var items = page.Items;
    var paginationToken = page.PaginationToken;

    // Do something.
}
```
