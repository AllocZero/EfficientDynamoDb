---
id: error-handling
title: Error handling
slug: ../../dev-guide/high-level/error-handling
---

# Error handling {#error-handling}

EfficientDynamoDb supports two ways of handling DynamoDB errors:
- Throwing exceptions (default behavior)
- Suppressing exceptions and returning [`OpResult`](../../api_reference/error_handling/op-result.md#op-result) or [`OpResult<T>`](../../api_reference/error_handling/op-result.md#op-result-t).

:::note
Both approaches utilize [retry strategies](../configuration/retry-strategies.md) before throwing an exception or returning an error result.
:::

## Throwing exceptions {#throwing-exceptions}

Throwing exceptions is the default behavior.
After encountering an error and after all retries are exhausted, an `DdbException` is thrown.

## Returning errors as results {#returning-errors-as-results}

This behavior can be useful when some error is expected to happen, e.g. when table is throttled.
To enable this behavior, use `SuppressThrowing()` method with a builder API for any data plane operation:
- [GetItem](../../api_reference/builders/get-item-builder.md#suppressthrowing)
- [PutItem](../../api_reference/builders/put-item-builder.md#suppressthrowing)
- [UpdateItem](../../api_reference/builders/update-item-builder.md#suppressthrowing)
- [DeleteItem](../../api_reference/builders/delete-item-builder.md#suppressthrowing)
- BatchGetItem
- BatchWriteItem
- [Query](../../api_reference/builders/query-builder.md#suppressthrowing)
- [Scan](../../api_reference/builders/scan-builder.md#suppressthrowing)
- TransactGetItems
- TransactWriteItems

### `OpResult` vs `OpResult<T>`

After applying `SuppressThrowing()`, executing operations will return [`OpResult`](../../api_reference/error_handling/op-result.md#op-result) or [`OpResult<T>`](../../api_reference/error_handling/op-result.md#op-result-t) instead of throwing an exception.
The only difference between these two types is that `OpResult<T>` contains the result of the operation, while `OpResult` doesn't:
- `ExecuteAsync()` always returns `OpResult`.
- `OpResult<T>` is returned when `ToItemAsync()`, `ToResponseAsync()`, `ToListAsync()` or similar methods are called.

### Handling error results {#handling-error-results}

There are two main ways to handle error results.

#### Checking `ErrorType` and using `As` methods

```csharp
var result = await context.GetItem()
    .WithPrimaryKey("pk", "sk")
    .SuppressThrowing()
    .ToItemAsync();

if (result.IsSuccess)
    return result.Value;

if (result.ErrorType == OpErrorType.ProvisionedThroughputExceeded)
{
    var provisionedException = result.AsProvisionedThroughputExceededException();
    // Handle provisioned throughput exceeded error
}
```

#### Using pattern matching

```csharp
var result = await context.GetItem()
    .WithPrimaryKey("pk", "sk")
    .SuppressThrowing()
    .ToItemAsync();

if (result.IsSuccess)
    return result.Value;

if (result.Exception is ProvisionedThroughputExceededException provisionedException)
{
    // Handle provisioned throughput exceeded error
}
```

#### Using `EnsureSuccess()` method

There is a convenience method called `EnsureSuccess()` that can be used to throw an exception if the operation was not successful.

With `OpResult<T>`:
```csharp
var result = await context.GetItem()
    .WithPrimaryKey("pk", "sk")
    .SuppressThrowing()
    .ToItemAsync();

var actualItem = result.EnsureSuccess();
```

With `OpResult`:
```csharp
var result = await context.GetItem()
    .WithPrimaryKey("pk", "sk")
    .SuppressThrowing()
    .ExecuteAsync();

result.EnsureSuccess();
```