---
id: op-error-type
title: OpErrorType
slug: ../../api-reference/error-handling/op-error-type
---

`OpErrorType` is an enum that represents the type of error that occurred during an operation.
Used in [`OpResult`](op-result.md#op-result) and [`OpResult<T>`](op-result.md#op-result-t) to indicate the type of error that occurred during an operation.

The possible values are:
- `None`
- `Unknown`
- `ServiceUnavailable`
- `InternalServerError`
- `TransactionCanceled`
- `ConditionalCheckFailed`
- `ProvisionedThroughputExceeded`
- `AccessDenied`
- `IncompleteSignature`
- `ItemCollectionSizeLimitExceeded`
- `LimitExceeded`
- `MissingAuthenticationToken`
- `RequestLimitExceeded`
- `ResourceInUse`
- `ResourceNotFound`
- `Throttling`
- `UnrecognizedClient`
- `Validation`
- `IdempotentParameterMismatch`
- `TransactionInProgress`