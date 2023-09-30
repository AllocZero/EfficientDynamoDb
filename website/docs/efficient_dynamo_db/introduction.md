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
    .WithKeyExpression(Condition<UserEntity>.On(x => x.Username).EqualsTo("qwerty"))
    .WithFilterExpression(Condition<UserEntity>.On(x => x.Age).GreaterThanOrEqualsTo(18))
    .ToListAsync();
```

* `DeleteItem` - Delete a single item

```csharp
await _context.DeleteItemAsync<UserEntity>("qwerty", "1234");
```

## Benchmarks

A strong focus on performance allows EfficientDynamoDb to consume much less CPU time in most operations than other DynamoDB libraries. It's also capable of zero-allocation deserialization. In general, EfficientDynamoDb allocates up to 26X less memory and is up to 21X faster than the official AWS .NET SDK.

```markdown
|          Method | Items |         Mean |      Error |     StdDev |  Gen 0 |  Gen 1 | Gen 2 |  Allocated |
|---------------- |------ |-------------:|-----------:|-----------:|-------:|-------:|------:|-----------:|
|    EfficientDdb |    10 |      79.1 us |     0.8 us |     0.7 us |    4.3 |      - |     - |    18.2 KB |
|     aws-sdk-net |    10 |     620.8 us |     7.2 us |     6.0 us |   85.9 |   18.5 |     - |   352.3 KB |
|                 |       |              |            |            |        |        |       |            |
|    EfficientDdb |   100 |     484.1 us |     1.7 us |     1.6 us |   29.2 |    5.8 |     - |   120.8 KB |
|     aws-sdk-net |   100 |   6,127.1 us |   120.6 us |   148.1 us |  500.0 |  250.0 |     - |  3066.6 KB |
|                 |       |              |            |            |        |        |       |            |
|    EfficientDdb |  1000 |   4,733.0 us |    24.2 us |    22.7 us |  195.3 |   93.7 |     - |  1147.5 KB |
|     aws-sdk-net |  1000 |  99,438.8 us | 1,951.4 us | 3,518.9 us | 5200.0 | 1600.0 | 600.0 | 30177.0 KB |
 ```

Every benchmark simulates `QUERY` request to DynamoDb that returns responses with a number of items specified in `Items` column. All network calls are excluded, so the data is served from memory to eliminate network inconsistency in benchmarks. [Entity](https://github.com/AllocZero/EfficientDynamoDb/blob/42d6ed914ae37be0c2ef6e4cba1334c7a27cade8/src/Benchmarks/AwsDdbSdk/Entities/MixedEntity.cs) contains various data types, including lists, hashsets, strings, etc. Complete benchmark sources are [on GitHub](https://github.com/AllocZero/EfficientDynamoDb/blob/main/src/Benchmarks/Benchmarks/Query/QueryEntityComparisonBenchmark.cs).

Benchmark machine configuration:

```markdown
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.685 (2004/?/20H1)
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.110
  [Host]     : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT
  DefaultJob : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT
```

## Compatibility with official [AWS SDK for .NET](https://github.com/aws/aws-sdk-net)

**EfficientDynamoDb** API is quite similar to the official DynamoDB SDK for .NET, so migration should be relatively easy.
The most significant differences are described in the [compatibility guide](../dev_guide/sdk-compatibility.md).
