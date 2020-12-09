# EfficientDynamoDb (In Development)
EfficientDynamoDb is a high performance DynamoDb library with a huge focus on efficient resources utilization. Due to DynamoDb extreme scaling capabilities it is very important for backend services to not waste valuable CPU time on unmarshalling responses. EfficientDynamoDb is capable of zero allocation deserialization. In general it allocates 2-5X less memory and is 2-10X faster than official AWS SDK.

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
