# EfficientDynamoDb (In Development)
EfficientDynamoDb is a high performance DynamoDb library with a huge focus on efficient resources utilization. Due to DynamoDb extreme scaling capabilities it is very important for backend services to not waste valuable CPU time on unmarshalling responses. EfficientDynamoDb is capable of zero allocation deserialization. In general it allocates 2-5X less memory and is 2-10X faster than official AWS SDK.

## Benchmarks

### Comparison with official DynamoDb SDK for .NET

 ```
|            Method |  Items |        Mean |      Error |     StdDev |  Gen 0 | Gen 1 | Gen 2 |  Allocated |
|------------------ |------- |------------:|-----------:|-----------:|-------:|------:|------:|-----------:|
|       aws-sdk-net |     10 |    343.2 us |    1.81 us |    1.60 us |   35.1 |   0.9 |     - |   144.6 KB |
| EfficientDynamoDb |     10 |    110.1 us |    1.93 us |    1.81 us |   10.0 |     - |     - |    41.4 KB |
|                   |        |             |            |            |        |       |       |            |
|       aws-sdk-net |    100 |  2,792.3 us |   20.32 us |   19.00 us |  187.5 |  82.0 |     - |  1047.9 KB |
| EfficientDynamoDb |    100 |    579.5 us |    4.52 us |    3.53 us |   57.6 |   1.9 |     - |   235.5 KB |
|                   |        |             |            |            |        |       |       |            |
|       aws-sdk-net |   1000 | 36,024.8 us |  681.23 us |  603.89 us | 1733.3 | 800.0 | 200.0 | 10076.3 KB |
| EfficientDynamoDb |   1000 |  6,000.9 us |   39.55 us |   33.02 us |  375.0 | 179.6 |     - |  2176.1 KB |
 ```
 Every benchmark simulates `QUERY` request to DynamoDb that return responses with number of items specified in `Items` column. All network calls are excluded and data is served from memory to eliminate network inconsistency in benchmarks. [Entity](https://github.com/AllocZero/EfficientDynamoDb/blob/42d6ed914ae37be0c2ef6e4cba1334c7a27cade8/src/Benchmarks/AwsDdbSdk/Entities/MixedEntity.cs) contains various data types including lists, hashsets, strings, etc.
 
 **Configuration**
```
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.630 (2004/?/20H1)
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.101
  [Host]     : .NET Core 3.1.1 (CoreCLR 4.700.19.60701, CoreFX 4.700.19.60801), X64 RyuJIT
  DefaultJob : .NET Core 3.1.1 (CoreCLR 4.700.19.60701, CoreFX 4.700.19.60801), X64 RyuJIT
```

## Low Level API

Low Level API mimics offical DynamoDb HTTP API. To get started just create `DynamoDbContext` by specifying credentials and AWS region.

```
var credentials = new AwsCredentials("accessKey", "secretKey");
var config = new DynamoDbContextConfig(RegionEndpoint.USEast1, credentials);
var context = new DynamoDbContext(config);
```

Entities are represented by the `Document` class, which contains already parsed data and provides `IDictionary<string, AttributeValue>` API:
```
var response = await context.GetItemAsync(new GetItemRequest
            {
                TableName = "table_name",
                Key = new PrimaryKey("pk_value", "sk_value")
            });

var pk = response.Item["pk"].AsString();
var sk = response.Item["sk"].ToInt();
```

Every single DynamoDb attribute is represented by `AttributeValue` readonly struct. There are various implicit operators that simplify attribute value creation from common types like `string`, `int`, `Document`, `bool`. The minimum size of one attribute in x64 system is 9 bytes (1 byte for `AttributeType` enum and 8 bytes for a reference).

## High Level API

In Progress,  TODO:
1. `Document.ToObject<TValue>` API to convert documents to classes.
1. High Level dynamodb context with simplified API.
1. Converters support: high level user-friendly converters and low-level `ReadOnlySpan` converters with direct access to buffered memory.
1. `DdbJsonReader` for classes.
1. Built-in composite keys support.
