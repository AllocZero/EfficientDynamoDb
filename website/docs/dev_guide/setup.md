---
id: setup
title: Accessing the API
slug: ../dev-guide/setup
---

Accessing both low-level and high-level APIs is possible through the instance of `DynamoDbContext`.
Its creation requires setting up credentials and region.
Additionally, you can setup retry strategies, the table name prefix, and default converters.

## Credentials

[AwsCredentials](https://github.com/AllocZero/EfficientDynamoDb/blob/master/src/EfficientDynamoDb/Configs/AwsCredentials.cs) class is used to pass credentials to `DynamoDbContext`.

**Important note:** Never put your credentials as plain text in your code! Use some secure store instead.

Example:

```csharp
var accessKey = Environment.GetEnvironmentVariable("accessKeyVariable");
var secretKey = Environment.GetEnvironmentVariable("secretKeyVariable");

var credentials = new AwsCredentials(accessKey, secretKey);
```

If you want to use token auth, you can pass it to the `AwsCredentials` constructor as well:

```csharp
var accessKey = Environment.GetEnvironmentVariable("accessKeyVariable");
var secretKey = Environment.GetEnvironmentVariable("secretKeyVariable");
var token = "YourToken";

var credentials = new AwsCredentials(accessKey, secretKey, token);
```

## Region

AWS Region selection allows you to access DynamoDb in a specific geographic region.
It can be useful for redundancy and to keep your data and applications running close to where you and your users access them.

There are static properties in [RegionEndpoint](https://github.com/AllocZero/EfficientDynamoDb/blob/master/src/EfficientDynamoDb/Context/Config/RegionEndpoint.cs) class for each available region. For a full list of supported regions check [RegionEndpoint reference](../api_reference/region-endpoint.md).

```csharp
var region = RegionEndpoint.USEast1;
```

## DynamoDbContextConfig

You can create a `DynamoDbContextConfig` from the region and credentials explained in the previous steps.

In the following example, it's assumed that the `credentials` variable already exists and initialized with your access and secret keys:

```csharp
var config = new DynamoDbContextConfig(RegionEndpoint.USEast1, credentials);
```

Optionally, you can set the table name prefix, HTTP client factory, and retry strategies:

```csharp
var config = new DynamoDbContextConfig(RegionEndpoint.USEast1, credentials)
{
    TableNamePrefix = "your_prefix",
    HttpClientFactory = yourFactory
};
config.RetryStrategies.ThrottlingStrategy = DefaultRetryStrategy.Instance;
```

For more info about retry strategies and possible options, check the [retry strategies page](retry-strategies.md).
