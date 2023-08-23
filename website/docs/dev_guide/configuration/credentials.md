---
id: credentials
title: Credentials management
slug: ../dev-guide/configuration/credentials
---

There are two main ways to pass credentials to EfficientDynamoDb:

1. Using native credential entities.
1. Using credentials flow from the official .NET AWS SDK.

By default, EfficientDynamoDb uses its own credential entities.
However, sometimes it's beneficial to use credentials flow from the official .NET AWS SDK.
E.g., you have complex credentials retrieving patterns that are not supported by EfficientDynamoDb natively yet, or you prefer using battle-tested code for such critical pieces as credentials management.

## Using native credential entities

`AwsCredentials` requires passing access and secret keys directly in the constructor.

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

## Using credentials flow from the official .NET AWS SDK

To integrate official `Amazon.Runtime.AWSCredentials` objects follow this simple steps:

1. Install the [EfficientDynamoDb.Credentials.AWSSDK nuget package](https://www.nuget.org/packages/EfficientDynamoDb.Credentials.AWSSDK/)
1. Use extension method `ToCredentialsProvider()` to convert `AWSCredentials` into the `IAwsCredentialsProvider` which is accepted by `DynamoDbContextConfig`.

The following example shows how to convert `EnvironmentVariablesAWSCredentials` from the .NET AWS SDK to `IAwsCredentialsProvider` accepted by EfficientDynamoDb.

```csharp
// AWS SDK credentials class
var awsSdkCredentials = new EnvironmentVariablesAWSCredentials(); 

// EfficientDynamoDb credentials
var effDdbCredentials = awsSdkCredentials.ToCredentialsProvider();

// EfficientDynamoDb context config
var config = new DynamoDbContextConfig(RegionEndpoint.USEast1, effDdbCredentials);
```

### Automatic credentials resolution

One of the common use-cases with the official SDK is to rely on authomatic credentials resolution:

```csharp
var client = new AmazonDynamoDBClient();
var context = new DynamoDBContext(client);
```

This behavior is achievable using the `FallbackCredentialsFactory` from the official SDK and compatibility extension method:

```csharp
var awsSdkCredentials = FallbackCredentialsFactory.GetCredentials();

// EfficientDynamoDb credentials
var effDdbCredentials = awsSdkCredentials.ToCredentialsProvider();

// EfficientDynamoDb context config
var config = new DynamoDbContextConfig(RegionEndpoint.USEast1, effDdbCredentials);
```