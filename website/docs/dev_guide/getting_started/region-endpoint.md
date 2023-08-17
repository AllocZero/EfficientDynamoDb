---
id: region-endpoint
title: Region management
slug: ../dev-guide/getting_started/region-endpoint
---

`RegionEndpoint` instance is required to create `DynamoDbContextConfig` and start making calls to the DynamoDB.
There are two main ways to create an instance of `RegionEndpoint` in EfficientDynamoDb:

1. Use one of static properties declared in [RegionEndpoint](https://github.com/AllocZero/EfficientDynamoDb/blob/main/src/EfficientDynamoDb/Context/Config/RegionEndpoint.cs).
1. Use `RegionEndpoint` constructor to dynamically create an instance.

## Using static properties

Static properties should be your default way to get an instance of `RegionEndpoint`.
It suits most of the production cases and makes sure that there is no mistake in the region name:

```csharp
var region = RegionEndpoint.USEast1;
```

We regularly update the [list of available AWS regions](#predefined-aws-regions) when new ones are launched.

## Create regions dynamically

You might want to create regions dynamically in following cases:

1. You don't know region to access at the compile time.
1. Region you want to access is not available as a static property.
1. You need to set custom request URI, e.g. for local DynamoDB.

Use `RegionEndpoint.Create(string region)` to dynamically create a region:

```csharp
var region = RegionEndpoint.Create("us-east-1");
```

Use `RegionEndpoint.Create(string region, string requestUri)` to create a region with custom service URL:

```csharp
var region = RegionEndpoint.Create("local", "http://localhost:8000");
```

## Predefined AWS regions

| AWS Region Code | AWS Region Name            | C# Region Property            |
|-----------------|----------------------------|-------------------------------|
| us-east-1       | US East (N. Virginia)      | `RegionEndpoint.USEast1`      |
| us-east-2       | US East (Ohio)             | `RegionEndpoint.USEast2`      |
| us-west-1       | US West (N. California)    | `RegionEndpoint.USWest1`      |
| us-west-2       | US West (Oregon)           | `RegionEndpoint.USWest2`      |
| af-south-1      | Africa (Cape Town)         | `RegionEndpoint.AFSouth1`     |
| ap-east-1       | Asia Pacific (Hong Kong)   | `RegionEndpoint.APEast1`      |
| ap-south-1      | Asia Pacific (Mumbai)      | `RegionEndpoint.APSouth1`     |
| ap-south-2      | Asia Pacific (Hyderabad)   | `RegionEndpoint.APSouth2`     |
| ap-northeast-1  | Asia Pacific (Tokyo)       | `RegionEndpoint.APNorthEast1` |
| ap-northeast-2  | Asia Pacific (Seoul)       | `RegionEndpoint.APNorthEast2` |
| ap-northeast-3  | Asia Pacific (Osaka-Local) | `RegionEndpoint.APNorthEast3` |
| ap-southeast-1  | Asia Pacific (Singapore)   | `RegionEndpoint.APSouthEast1` |
| ap-southeast-2  | Asia Pacific (Sydney)      | `RegionEndpoint.APSouthEast2` |
| ap-southeast-3  | Asia Pacific (Jakarta)     | `RegionEndpoint.APSouthEast3` |
| ap-southeast-4  | Asia Pacific (Melbourne)   | `RegionEndpoint.APSouthEast4` |
| ca-central-1    | Canada (Central)           | `RegionEndpoint.CACentral1`   |
| cn-north-1      | China (Beijing)            | `RegionEndpoint.CNNorth1`     |
| cn-northwest-1  | China (Ningxia)            | `RegionEndpoint.CNNorthWest1` |
| eu-central-1    | Europe (Frankfurt)         | `RegionEndpoint.EUCenteral1`  |
| eu-central-2    | Europe (Zurich)            | `RegionEndpoint.EUCenteral2`  |
| eu-west-1       | Europe (Ireland)           | `RegionEndpoint.EUWest1`      |
| eu-west-2       | Europe (London)            | `RegionEndpoint.EUWest2`      |
| eu-west-3       | Europe (Paris)             | `RegionEndpoint.EUWest3`      |
| eu-south-1      | Europe (Milan)             | `RegionEndpoint.EUSouth1`     |
| eu-south-2      | Europe (Spain)             | `RegionEndpoint.EUSouth2`     |
| eu-north-1      | Europe (Stockholm)         | `RegionEndpoint.EUNorth1`     |
| me-south-1      | Middle East (Bahrain)      | `RegionEndpoint.MESouth1`     |
| me-central-1    | Middle East (UAE)          | `RegionEndpoint.MECentral1`   |
| il-central-1    | Israel (Tel Aviv)          | `RegionEndpoint.ILCentral1`   |
| sa-east-1       | South America (São Paulo)  | `RegionEndpoint.SAEast1`      |
| us-gov-east-1   | AWS GovCloud (US-East)     | `RegionEndpoint.USGovEast1`   |
| us-gov-west-1   | AWS GovCloud (US)          | `RegionEndpoint.USGovWest1`   |
