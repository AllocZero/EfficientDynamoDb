---
id: attributes
title: Attributes
slug: ../dev-guide/high-level/attributes
---

When using high-level API, data classes have to be marked with certain attributes to map the data to DynamoDB tables.

## DynamoDbTable

Specifies a target table name.

**Required:** false

```csharp
[DynamoDbTable("users")]
public class UserEntity { ... }
```

If the same entity class needs to be stored in different tables, the table name can be overridden using the `WithTableName` extension method.

Note: *`DynamoDbTable` supports inheritance, can be applied to the base class.*

## DynamoDbProperty

Maps a property to the DynamoDB attribute.

**Required:** true

```csharp
[DynamoDbProperty("first_name")]
public string FirstName { get; set; }
```

### Mapping primary key

Partition and sort key properties have to specify `DynamoDbAttributeType` additionally:

```csharp
[DynamoDbProperty("pk", DynamoDbAttributeType.PartitionKey)]
public string Pk { get; set; }

[DynamoDbProperty("sk", DynamoDbAttributeType.SortKey)]
public string Sk { get; set; }
```

### Custom converters

An optional converter can also be specified per property. For more details describing how to create your own converters, check our Converters guide.

```csharp
[DynamoDbProperty("gender", typeof(StringEnumDdbConverter<Gender>))]
public Gender Gender { get; set; }
```

## DynamoDbConverter

Associates class or struct with specified converter, thus removing the need to specify converter type in `DynamoDbProperty` every single time.

```csharp
[DynamoDbConverter(typeof(CompositeAddressConverter))]
public class Address { ... }
```

## DynamoDbVersion

Enables optimistic concurrency. Can only be applied to properties of `byte?`, `short?`, `int?` and `long?` types.

```csharp
[DynamoDbVersion, DynamoDbProperty("version"))]
public int? Version { get; set; }
```

`DynamoDbVersion` attribute is designed for backward compatibility with the official AWS .NET SDK.
It only works with `SaveAsync` and `DeleteAsync` extension methods and does not affect other operations.
