---
id: retry-strategies
title: Retry Strategies
slug: ../dev-guide/retry-strategies
---

## Why do you need retries

When working with DynamoDB, you may face errors that can be solved by retrying the operation.
Rate limiting happens often and is considered a _normal (to some degree)_ behavior that should be handled appropriately.

## Retries in EfficientDynamoDb

**EfficientDynamoDb** aims to simplify retries so that you don't have to handle them in your business layer.
You can specify a desired `RetryStrategy` for every retriable issue that may happen while interacting with DynamoDB.

EfficientDynamoDb supports retries for the following errors:

1. `InternalServerErrorStrategy` - Internal server error happened on AWS.
1. `LimitExceededStrategy` - There are too many concurrent control plane operations.
The cumulative number of tables and indexes in the `CREATING`, `DELETING`, or `UPDATING` state cannot exceed 50.
1. `ProvisionedThroughputExceededStrategy` - Maximum allowed provisioned throughput for a table or for one or more global secondary indexes was exceeded.
1. `RequestLimitExceededStrategy` - Throughput exceeds the current throughput limit for the account.
1. `ServiceUnavailableStrategy` - DynamoDB is currently unavailable. (This should be a temporary state.)
1. `ThrottlingStrategy` - Control plane API operations are performed too rapidly.

You're free to select any predefined strategy for every error or create your behavior for retries.
More info about creating own retry policies is in [this section](#implementing-custom-retry-strategy)

## Predefined retry strategies

**EfficientDynamoDb** package contains predefined retry strategies that are most common and suit most DynamoDB users. They can be created via [RetryStrategyFactory](https://github.com/AllocZero/EfficientDynamoDb/blob/main/src/EfficientDynamoDb/Configs/Retries/RetryStrategyFactory.cs).

### LinearRetryStrategy

The most simple retry policy. It retries 5 times with 50ms delays between attempts. Delay and max retries count are configurable.

Example:

```csharp
var strategy = RetryStrategyFactory.Linear(maxRetriesCount: 10, delayMs: 25);
```

[Sources](https://github.com/AllocZero/EfficientDynamoDb/blob/main/src/EfficientDynamoDb/Configs/Retries/LinearRetryStrategy.cs)

### JitterRetryStrategy

Exponential backoff strategy with jitter. Designed to spread out the load to an approximately constant rate. Useful for handling DDB throttling exceptions and similar cases.

Represents `FullJitter` retry strategy from this [AWS article](https://aws.amazon.com/blogs/architecture/exponential-backoff-and-jitter/).

Example:

```csharp
var strategy = RetryStrategyFactory.Jitter(maxRetriesCount: 10, baseDelayMs: 25, maxDelayMs: 400);
```

[Sources](https://github.com/AllocZero/EfficientDynamoDb/blob/main/src/EfficientDynamoDb/Configs/Retries/JitterRetryStrategy.cs)

## Implementing custom retry strategy

All retry strategies must implement the [IRetryStrategy](https://github.com/AllocZero/EfficientDynamoDb/blob/main/src/EfficientDynamoDb/Configs/Retries/IRetryStrategy.cs) interface.
It contains a single `bool TryGetRetryDelay(int attempt, out TimeSpan delay)` method to calculate the actual delay required for the specified retry attempt.

**Parameters:**

* `int attempt` - 0-based index of the retry attempt.
E.g., for the first retry value of `attempt` is `0`, and for the 3rd retry, it is `2`.
* `out TimeSpan delay` - out parameter that contains a calculated delay for the specified attempt.

**Returns:**

`false` if retry shouldn't happen, e.g., when the maximum number of retries is reached. Otherwise, return `true`.

### Best practices

1. Make sure your strategy implementation is thread-safe if you share it across different retriable issues.
1. Try to keep the strategy simple to avoid performance degradations due to complex calculations combined with frequent retries.
