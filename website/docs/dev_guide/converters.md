---
id: attributes
title: DynamoDB Attributes
slug: ../dev-guide/attributes
---

When using high-level API, data classes have to be marked with certain attributes in order to map the data to DynamoDB tables.

## DynamoDBTable

Specifies a target table name.

**Required:** true

```C#
[DynamoDBTable("users")]
public class UserEntity { ... }
```

:::note
Supports inheritance, can be applied to the base class.

## DynamoDBProperty

Maps a property to the DynamoDB attribute.

**Required:** true

```C# 
[DynamoDBProperty("fist_name")]
public string FirstName { get; set; }
```

### Mapping primary key
Partition and sort key properties have to additionally specify `DynamoDbAttributeType`:

```C#
[DynamoDBProperty("pk", DynamoDbAttributeType.PartitionKey)]
public string Pk { get; set; }

[DynamoDBProperty("sk", DynamoDbAttributeType.SortKey)]
public string Sk { get; set; }
```

### Custom converters

An optional converter can also be specified per property. For more details describing how to create your own converters check our Converters guide.

```C#
[DynamoDBProperty("gender", typeof(StringEnumDdbConverter<Gender>))]
public Gender Gender { get; set; }
```

## DynamoDBConverter
Associates class or struct with specified converter, thus removing the need to specify converter type in `DynamoDBProperty` every single time.

```C#
[DynamoDBConverter(typeof(CompositeAddressConverter))]
public class Address { ... }
```

## DynamoDBVersion

Enables optimistic concurrency. Can only be applied to properties of `byte?`, `short?`, `int?` and `long?` types.

```C#
[DynamoDBVersion, DynamoDBProperty("version"))]
public int? Version { get; set; }
```

:::caution
`DynamoDBVersion` attribute is designed for backward compatibility with official AWS SDK, it only works with `SaveAsync` extension method and does not effect other operations.