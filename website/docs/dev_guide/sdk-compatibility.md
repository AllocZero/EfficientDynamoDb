---
id: sdk-compatibility
title: AWS SDK Compatibility
slug: ../dev-guide/sdk-compatibility
---

This guide describes key differences from the AWS .NET SDK and how to make the code backward compatible.

## Attributes

By default, class properties are not mapped to table attributes, and you have to specify the `[DynamoDbProperty]` attribute explicitly.
It also means that the `[DynamoDbIgnore]` attribute is no longer needed.

There are no `[DynamoDbHashKey]` and `[DynamoDbRangeKey]` attributes for hash and sort key anymore.
You should use `[DynamoDbProperty]` and set the `AttributeType` property to the correct type.

Check out the  [attributes guide](./high_level/attributes.md#mapping-primary-key) for more info.

## Credentials

By default, EfficientDynamoDb uses its own credential entities. However, sometimes it's beneficial to use credentials flow from the official .NET AWS SDK. E.g., you have complex credentials retrieving patterns that are not supported by EfficientDynamoDb natively yet, or you prefer using battle-tested code for such critical pieces as credentials management.

To integrate official `Amazon.Runtime.AWSCredentials` objects follow this simple steps:

1. Install the [EfficientDynamoDb.Credentials.AWSSDK nuget package](https://www.nuget.org/packages/EfficientDynamoDb.Credentials.AWSSDK/)
1. Use extension method `ToCredentialsProvider()` to convert `AWSCredentials` into the `IAwsCredentialsProvider` which is accepted by `DynamoDbContextConfig`.

The following example shows how to integrate `EnvironmentVariablesAWSCredentials` from the AWS SDK into EfficientDynamoDb.

```csharp
var awsSdkCredentials = new EnvironmentVariablesAWSCredentials(); // AWS SDK credentials class
var config = new DynamoDbContextConfig(RegionEndpoint.USEast1, awsSdkCredentials.ToCredentialsProvider());
var context = new DynamoDbContext(config);
```

## Operations

In EfficientDynamoDb operation names match the names in DynamoDB itself. E.g. `LoadAsync(...)` in official SDK becomes `GetItemAsync(...)` in EfficientDynamoDb.

`SaveAsync(...)` in official SDK uses `UpdateItem` operation under the hood but accepts a full item like `PutItem`.
EfficientDynamoDb supports both `UpdateItemAsync(...)` and `PutItemAsync(...)` that behave as intended by DynamoDB (i.e., former updates only specified properties and later replaces the full item).

For compatibility reasons, EfficientDynamoDb provides `SaveAsync(...)` and `DeleteAsync(...)` extension methods that mock the behavior of the official SDK.
However, it's highly encouraged to use native API suitable for your use-case because they scale better and lead to better table designs.

Keep in mind that `[DynamoDbVersion]` attribute will only have an effect while using `SaveAsync` and `DeleteAsync` extension methods.

## DateTimes

AWS SDK has several `DateTime` flaws, which are the reason why the default behavior was changed:

* SDK converts and saves all dates as UTC, even when a timezone is unknown (`DateTimeKind.Unspecified`).
* SDK completely ignores timezone from the date string and always converts dates back to local using the current server timezone.

| | AWS SDK  | EfficientDynamoDb   |
|---|---|---|
| Stores Local as | UTC | Local |
| Stores UTC as | UTC  | UTC |
| Stores Unspecified as | UTC | Unspecified  |
| Persists `DateTime.Kind` | No | Yes |
| Format | `yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK` | `O` ISO8601 |

To make `DateTime` logic backward compatible with AWS SDK, use `SdkDateTimeDdbConverter`.
