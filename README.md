# EfficientDynamoDb ![.NET Core](https://github.com/AllocZero/EfficientDynamoDb/workflows/Build/badge.svg) ![NuGet Version](https://img.shields.io/nuget/v/EfficientDynamoDb)
EfficientDynamoDb is a high performance DynamoDb library with a huge focus on efficient resources utilization. Due to DynamoDb extreme scaling capabilities it is very important for backend services to not waste valuable CPU time on unmarshalling responses. EfficientDynamoDb is capable of zero allocation deserialization. In general it allocates up to 26X less memory and is up to 21X faster than official AWS SDK.

## Benchmarks

### Comparison with official DynamoDb SDK for .NET

 ```
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
 Every benchmark simulates `QUERY` request to DynamoDb that return responses with number of items specified in `Items` column. All network calls are excluded and data is served from memory to eliminate network inconsistency in benchmarks. [Entity](https://github.com/AllocZero/EfficientDynamoDb/blob/42d6ed914ae37be0c2ef6e4cba1334c7a27cade8/src/Benchmarks/AwsDdbSdk/Entities/MixedEntity.cs) contains various data types including lists, hashsets, strings, etc.
 
 **Configuration**
```
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.685 (2004/?/20H1)
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.110
  [Host]     : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT
  DefaultJob : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT
```

## Introduction

### API overview

**EfficientDynamoDb** has two types of API: high-level and low-level.
High-level API, in most cases, is on-par with low-level in terms of raw processing speed and requires fewer memory allocations.
It is recommended to use the high-level API in most cases unless you're sure about what you do.

Data classes should be marked by `[DynamoDbTable(string tableName)]` attributes to make it work with most high-level features.
Most operations are provided through the `DynamoDbContext` object.

Examples of API usage (`context` is an object of type `DynamoDbContext`):

#### GetItem
Retrieves a single item.

```csharp
var user = await _context.GetItemAsync<UserEntity>("qwerty", "1234");
```

#### Query

Retrieves a list of items that match key and filter conditions.

```csharp
var items = await _context.Query<UserEntity>()
    .WithKeyExpression(Filter<UserEntity>.On(x => x.Username).EqualsTo("qwerty"))
    .WithFilterExpression(Filter<UserEntity>.On(x => x.Age).GreaterThanOrEqualsTo(18))
    .ToListAsync();
```

#### UpdateItem
Edits an existing item's attributes or adds a new item to the table if it does not already exist.

```csharp
await ddbContext.UpdateItem<UserEntity>()
    .WithPrimaryKey("partitionKey", "sortKey")
    .On(x => x.FirstName).Assign("John")
    .On(x => x.LastName).Assign("Doe")
    .ExecuteAsync();
```

#### TransactWriteItems

Atomically applies one of four operations per item within the same AWS account and Region.

```csharp
await context.TransactWrite()
    .WithItems(
        Transact.PutItem(new UserEmailEntity("test@test.com")),
        Transact.ConditionCheck<UserEntity>()
            .WithPrimaryKey("partitionKey", "sortKey")
            .WithCondition(Condition<UserEntity>.On(x => x.Verified).EqualsTo(false))
    )
    .ExecuteAsync();
```

### Compatibility with official [AWS SDK for .NET](https://github.com/aws/aws-sdk-net)

**EfficientDynamoDb** API is quite similar to the official DynamoDB SDK for .NET, so migration should be relatively easy.
The most significant differences are described in the [compatibility guide](https://alloczero.github.io/EfficientDynamoDb/docs/dev-guide/sdk-compatibility).

### [Documentation](https://alloczero.github.io/EfficientDynamoDb/docs/dev-guide/setup)
