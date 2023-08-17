---
id: getting-started
title: Getting started
slug: ../dev-guide/getting_started/
---

## Installation

To get started, install the [EfficientDynamoDb nuget package](https://www.nuget.org/packages/EfficientDynamoDb).

## Initializing DynamoDbContext

All requests to DynamoDB in EfficientDynamoDb are done through `DynamoDbContext`.

First, add the following usings:

```csharp
using EfficientDynamoDb;
using EfficientDynamoDb.Configs;
```

Then you can create a `DynamoDbContext` like that:

```csharp
var accessKey = Environment.GetEnvironmentVariable("accessKeyVariable");
var secretKey = Environment.GetEnvironmentVariable("secretKeyVariable");

var credentials = new AwsCredentials(accessKey, secretKey);

var config = new DynamoDbContextConfig(RegionEndpoint.USEast1, credentials);
var context = new DynamoDbContext(config);
```

As you see, there are two main requirements to create the context: `RegionEndpoint` and `AwsCredentials`.

Optionally, you can set the table name prefix, HTTP client factory, and retry strategies:

```csharp
var config = new DynamoDbContextConfig(RegionEndpoint.USEast1, credentials)
{
    TableNamePrefix = "your_prefix",
    HttpClientFactory = yourFactory
};
config.RetryStrategies.ThrottlingStrategy = DefaultRetryStrategy.Instance;
```

For more info about retry strategies and possible options, check the [retry strategies guide](../retry-strategies.md).

## Region

AWS Region selection allows you to access DynamoDb in a specific geographic region.
It can be useful for redundancy and to keep your data and applications running close to where you and your users access them.

The simplest way to get `RegionEndpoint` is to use static property like `RegionEndpoint.USEast1`.
Learn more about ways to create `RegionEndpoint` in our [how-to guide](region-endpoint.md).

## Credentials

Natively, EfficientDynamoDb supports direct creation of [AwsCredentials](https://github.com/AllocZero/EfficientDynamoDb/blob/main/src/EfficientDynamoDb/Configs/AwsCredentials.cs) instance via constructor.

**Important note:** Never put your credentials as plain text in your code! Use some secure storage instead.

Example:

```csharp
var accessKey = Environment.GetEnvironmentVariable("accessKeyVariable");
var secretKey = Environment.GetEnvironmentVariable("secretKeyVariable");

var credentials = new AwsCredentials(accessKey, secretKey);
```

If you want to use token authentication, you can pass it to the `AwsCredentials` constructor as well:

```csharp
var accessKey = Environment.GetEnvironmentVariable("accessKeyVariable");
var secretKey = Environment.GetEnvironmentVariable("secretKeyVariable");
var token = "YourToken";

var credentials = new AwsCredentials(accessKey, secretKey, token);
```

Additionally, you can use all authentication methods from .NET AWS SDK via our [credentials compatibility package](https://www.nuget.org/packages/EfficientDynamoDb.Credentials.AWSSDK/).

For more info, refer to our guide to [credentials management in EfficientDynamoDb](credentials.md)
