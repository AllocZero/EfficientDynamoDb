---
id: attributes
title: Attributes
slug: ../dev-guide/high-level/attributes
---

When using high-level API, data classes have to be marked with certain attributes to map the data to DynamoDB tables.

## DynamoDBTable

Specifies a target table name.

**Required:** true

```csharp
[DynamoDBTable("users")]
public class UserEntity { ... }
```

Note: *`DynamoDBTable` supports inheritance, can be applied to the base class.*

## DynamoDBProperty

Maps a property to the DynamoDB attribute.

**Required:** true

```csharp
[DynamoDBProperty("fist_name")]
public string FirstName { get; set; }
```

### Mapping primary key

Partition and sort key properties have to specify `DynamoDbAttributeType` additionally:

```csharp
[DynamoDBProperty("pk", DynamoDbAttributeType.PartitionKey)]
public string Pk { get; set; }

[DynamoDBProperty("sk", DynamoDbAttributeType.SortKey)]
public string Sk { get; set; }
```

### Custom converters

An optional converter can also be specified per property. For more details describing how to create your own converters, check our Converters guide.

```csharp
[DynamoDBProperty("gender", typeof(StringEnumDdbConverter<Gender>))]
public Gender Gender { get; set; }
```

## DynamoDBConverter

Associates class or struct with specified converter, thus removing the need to specify converter type in `DynamoDBProperty` every single time.

```csharp
[DynamoDBConverter(typeof(CompositeAddressConverter))]
public class Address { ... }
```

## DynamoDBVersion

Enables optimistic concurrency. Can only be applied to properties of `byte?`, `short?`, `int?` and `long?` types.

```csharp
[DynamoDBVersion, DynamoDBProperty("version"))]
public int? Version { get; set; }
```

`DynamoDBVersion` attribute is designed for backward compatibility with the official AWS .NET SDK.
It only works with `SaveAsync` and `DeleteAsync` extension methods and does not affect other operations.
