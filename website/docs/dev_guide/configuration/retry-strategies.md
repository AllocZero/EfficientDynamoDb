---
id: retry-strategies
title: Retry Strategies
slug: ../../dev-guide/configuration/retry-strategies
---

## Why do you need retries

While working with DynamoDB, it's common to encounter transient errors, such as rate limiting.
Many of these errors are considered _normal_ behavior, to some degree, and should be handled appropriately.
Implementing effective retry strategies ensures smooth operations, mitigating the impact of such occasional disruptions.
Understanding why and how to handle retries in DynamoDB is crucial for consistent performance and resilience.

Though the concept of implementing retries might seem straightforward, the actual execution can be complex and error-prone.
If retries are set up incorrectly, they can easily overwhelm your system.
With DynamoDB, a poor retry strategy can increase the cost and latency of your application.

## Retries in EfficientDynamoDb

**EfficientDynamoDb** aims to simplify retries so that you don't have to handle them in your business layer.
You can specify a desired `RetryStrategy` for every retriable issue that can happen while interacting with DynamoDB.

EfficientDynamoDb supports retries for the following errors:

1. `InternalServerErrorStrategy` - Internal server error happened on AWS.
1. `LimitExceededStrategy` - There are too many concurrent control plane operations.
The cumulative number of tables and indexes in the `CREATING`, `DELETING`, or `UPDATING` state cannot exceed 50.
1. `ProvisionedThroughputExceededStrategy` - Maximum allowed provisioned throughput for a table or for one or more global secondary indexes was exceeded.
1. `RequestLimitExceededStrategy` - Throughput exceeds the current throughput limit for the account.
1. `ServiceUnavailableStrategy` - DynamoDB is currently unavailable. (This should be a temporary state.)
1. `ThrottlingStrategy` - Control plane API operations are performed too rapidly.

EfficientDynamoDb has sensible default retry strategies set for every retriable error.
They are the best choice for the vast majority of DynamoDB users.
At the same time, we provide an easy way to tune existing strategies or [implement your own](#implementing-custom-retry-strategy) from scratch.

### Default retry strategies

| Error category                | Retry configuration                            |
|-------------------------------|------------------------------------------------|
| InternalServerError           | Linear (5 attempts, 50ms delay)                |
| LimitExceeded                 | Linear (5 attempts, 50ms delay)                |
| ProvisionedThroughputExceeded | Jitter (5 attempts, 50ms delay, 16s max delay) |
| RequestLimitExceeded          | Jitter (5 attempts, 50ms delay, 16s max delay) |
| ServiceUnavailable            | Linear (5 attempts, 50ms delay)                |
| Throttling                    | Jitter (5 attempts, 50ms delay, 16s max delay) |

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

## Applying retry strategy

Retry strategies in EfficientDynamoDb are applied at the context level.
`DynamoDbContextConfig` provides an API to set any strategies you want.
Defaults will be used for all unset properties.

Example:

```csharp
var config = new DynamoDbContextConfig(RegionEndpoint.USEast1, new AwsCredentials("public", "secret"));
config.RetryStrategies.ThrottlingStrategy = RetryStrategyFactory.Jitter(baseDelayMs: 100);
```
