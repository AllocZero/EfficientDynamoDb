---
id: sdk-compatibility
title: AWS SDK Compatibility
slug: ../dev-guide/sdk-compatibility
---

This guide describes key differences from the AWS .NET SDK and how to make the code backward compatible.

## Attributes

By default class properties are not mapped to a table attributes and you have to explicitly specify the `[DynamoDBProperty]` attribute.
It also means that the `[DynamoDbIgnore]` attribute no longer needed.

There are no `[DynamoDBHashKey]` and `[DynamoDBRangeKey]` attributes for hash and sort key anymore.
You should use `[DynamoDBProperty]` and set the `AttributeType` property to the correct type.

Check out the  [attributes guide](./high_level/attributes.md#mapping-primary-key) for more info.

## Operations

In EfficientDynamoDb operation names match the names in DynamoDB itself. E.g. `LoadAsync(...)` in official SDK becomes `GetItemAsync(...)` in EfficientDynamoDb.

`SaveAsync(...)` in official SDK uses `UpdateItem` operation under the hood but accepts a full item like `PutItem`. 
EfficientDynamoDb supports both `UpdateItemAsync(...)` and `PutItemAsync(...)` that behave as intended by DynamoDB (i.e., former updates only specified properties and later replaces the full item).

For compatibility reasons, EfficientDynamoDb provides `SaveAsync(...)` and `DeleteAsync(...)` extension methods that mock the behavior of the official SDK.
However, it's highly encouraged to use native API suitable for your use-case because they scale better and lead to better table designs.

Keep in mind that `[DynamoDBVersion]` attribute will only have effect while using `SaveAsync` and `DeleteAsync` extension methods.

## DateTimes

AWS SDK has several `DateTime` flaws which are the reason why the default behavior was changed:
* SDK converts and saves all dates as UTC, even when a timezone is unknown (`DateTimeKind.Unspecified`).
* SDK completely ignores timezone from the date string and always converts dates back to local using current server timezone.

| | AWS SDK  | EfficientDynamoDb   |
|---|---|---|
| Stores Local as | UTC | Local |
| Stores UTC as | UTC  | UTC |
| Stores Unspecified as | UTC | Unspecified  |
| Persists `DateTime.Kind` | No | Yes |
| Format | `yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK` | `O` ISO8601 |

To make `DateTime` logic backward compatible with AWS SDK use `SdkDateTimeDdbConverter`.

