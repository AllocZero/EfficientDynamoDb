---
id: op-result
title: OpResult
slug: ../../api-reference/error-handling/op-result
---

## Summary

`OpResult` and `OpResult<T>` are used to encapsulate the result of an operation when `SuppressThrowing` builder method is used for any Data Plane operation.

## `OpResult` {#op-result}

A read-only struct that contains the status of an operation that doesn't return a value.

```csharp
[StructLayout(LayoutKind.Auto)]
public readonly struct OpResult
```

### Properties {#op-result-properties}

| Property    | Data Type     | Description                                                 |
|-------------|---------------|-------------------------------------------------------------|
| `ErrorType` | [`OpErrorType`](op-error-type.md) | Contains the type of error that occurred during the operation. |
| `Exception` | `DdbException?` | Contains the exception that occurred during the operation. `null` if `IsSuccess` is `true`.      |
| `IsSuccess` | `bool`        | Indicates whether the operation was successful.             |

### Methods {#op-result-methods}

| Method | Description |
|--------|-------------|
| `EnsureSuccess()` | Throws an exception if the operation failed. |

There are also extension methods to convert the `OpResult` to a specific exception type. See [Exception conversion methods](#exception-conversion-methods) below.

## `OpResult<T>` {#op-result-t}

A read-only struct that contains the result of an operation that returns a value.

```csharp
[StructLayout(LayoutKind.Auto)]
public readonly struct OpResult<T>
```

### Properties {#op-result-t-properties}

| Property    | Data Type     | Description                                                 |
|-------------|---------------|-------------------------------------------------------------|
| `ErrorType` | [`OpErrorType`](op-error-type.md) | Contains the type of error that occurred during the operation. |
| `Exception` | `DdbException?` | Contains the exception that occurred during the operation. `null` if `IsSuccess` is `true`.      |
| `IsSuccess` | `bool`        | Indicates whether the operation was successful.             |
| `Value` | `T?` | Contains the result of the operation if `IsSuccess` is `true`, otherwise `default(T)`. |

### Methods {#op-result-t-methods}

| Method | Description |
|--------|-------------|
| `EnsureSuccess()` | Throws an exception if the operation failed. |

Similar to [`OpResult` methods](#op-result-methods), there are also extension methods to convert the `OpResult<T>` to a specific exception type. See [Exception conversion methods](#exception-conversion-methods) below.

## Exception conversion methods {#exception-conversion-methods}

These extension methods are used to convert the `OpResult` and `OpResult<T>` to a specific exception type.

:::note
These methods will throw `InvalidOperationException` if an actual exception type doesn't match the called method.
Use `ErrorType` to check the type of the error before calling these methods.
:::

| Method | Description |
|--------|-------------|
| `AsServiceUnavailableException()` | Returns a `ServiceUnavailableException` if the operation failed. |
| `AsInternalServerErrorException()` | Returns a `InternalServerErrorException` if the operation failed. |
| `AsTransactionCanceledException()` | Returns a `TransactionCanceledException` if the operation failed. |
| `AsConditionalCheckFailedException()` | Returns a `ConditionalCheckFailedException` if the operation failed. |
| `AsProvisionedThroughputExceededException()` | Returns a `ProvisionedThroughputExceededException` if the operation failed. |
| `AsAccessDeniedException()` | Returns a `AccessDeniedException` if the operation failed. |
| `AsIncompleteSignatureException()` | Returns a `IncompleteSignatureException` if the operation failed. |
| `AsItemCollectionSizeLimitExceededException()` | Returns a `ItemCollectionSizeLimitExceededException` if the operation failed. |
| `AsLimitExceededException()` | Returns a `LimitExceededException` if the operation failed. |
| `AsMissingAuthenticationTokenException()` | Returns a `MissingAuthenticationTokenException` if the operation failed. |
| `AsRequestLimitExceededException()` | Returns a `RequestLimitExceededException` if the operation failed. |
| `AsResourceInUseException()` | Returns a `ResourceInUseException` if the operation failed. |
| `AsResourceNotFoundException()` | Returns a `ResourceNotFoundException` if the operation failed. |
| `AsThrottlingException()` | Returns a `ThrottlingException` if the operation failed. |
| `AsUnrecognizedClientException()` | Returns a `UnrecognizedClientException` if the operation failed. |
| `AsValidationException()` | Returns a `ValidationException` if the operation failed. |
| `AsIdempotentParameterMismatchException()` | Returns a `IdempotentParameterMismatchException` if the operation failed. |
| `AsTransactionInProgressException()` | Returns a `TransactionInProgressException` if the operation failed. |