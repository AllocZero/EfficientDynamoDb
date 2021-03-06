# EfficientDynamoDb (In Development) ![.NET Core](https://github.com/AllocZero/EfficientDynamoDb/workflows/.NET%20Core/badge.svg)
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
