---
id: introduction
title: Introduction
sidebar_label: Introduction
slug: /
---

**EfficientDynamoDb** is a high-performance DynamoDb library with a significant focus on efficient resources utilization.
DynamoDB delivers single-digit millisecond performance at any scale, so it is paramount to have capable clients to keep up.
EfficientDynamoDb optimizes the most critical execution paths to make typical operations up to 21x faster while consuming up to 26x less memory.
Despite being a high-performance library, EfficientDynamoDb still cares about API and provides easy-to-use wrappers even for complicated features like transactions, complex queries, update expressions, and retries.

## API overview

**EfficientDynamoDb** has two types of API: high-level and low-level.
High-level API, in most cases, is on-par with low-level in terms of raw processing speed and requires fewer memory allocations.
It is recommended to use the high-level API in most cases unless you're sure about what you do.

Data classes should be marked by `[DynamoDbTable(string tableName)]` attributes to make it work with most high-level features.
Most operations are provided through the `DynamoDbContext` object.

Examples of API usage (`context` is an object of type `DynamoDbContext`):

* `PutItem` - Save a full item

```csharp
var entity = new UserEntity {Username = "qwerty", Tag = "1234", Age = 15};
await _context.PutItemAsync(entity);
```

* `GetItem` - Retrieve a single item

```csharp
var user = await _context.GetItemAsync<UserEntity>("qwerty", "1234");
```

* `Query` - Retrieve a list of items that match key and filter conditions

```csharp
var items = await _context.Query<UserEntity>()
    .WithKeyExpression(Filter<UserEntity>.On(x => x.Username).EqualsTo("qwerty"))
    .WithFilterExpression(Filter<UserEntity>.On(x => x.Age).GreaterThanOrEqualsTo(18))
    .ToListAsync();
```

* `DeleteItem` - Delete a single item

```csharp
await _context.DeleteItemAsync<UserEntity>("qwerty", "1234");
```

## Performance overview

TBD

## Compatibility with official [AWS SDK for .NET](https://github.com/aws/aws-sdk-net)

**EfficientDynamoDb** API is quite similar to the official DynamoDB SDK for .NET, so migration should be relatively easy.
The most significant differences are:

1. There are no `[DynamoDBHashKey]` and `[DynamoDBRangeKey]` attributes for hash and sort key anymore.
You should use `[DynamoDBProperty]` and set the `AttributeType` property to the correct type.
1. Operations names match the names in DynamoDB itself. E.g. `LoadAsync(...)` in official SDK becomes `GetItemAsync(...)` in EfficientDynamoDb.
1. `SaveAsync(...)` in official SDK uses `UpdateItem` operation under the hood but accepts a full item like `PutItem`. EfficientDynamoDb supports both `UpdateItemAsync(...)` and `PutItemAsync(...)` that behave as intended by DynamoDB (i.e., former updates only specified properties and later replaces the full item).
For compatibility reasons, EfficientDynamoDb has the `SaveAsync(...)` method that mocks the behavior of the official SDK.
However, it's highly encouraged to use native API suitable for your use-case because they scale better and lead to better table designs.
