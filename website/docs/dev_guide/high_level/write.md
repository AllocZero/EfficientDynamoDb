---
id: write
title: Writing Data
slug: ../dev-guide/high-level/write
---

DynamoDB provides two primary operations for writing data:

* `PutItem` - Create a new item, or replace an old item with a new item.
* `Update` - Add or update specific attributes of an item.

DynamoDB also supports a `BatchWriteItem` operation for executing up to 25 `PutItem` operations in a single request.
It's covered in [batch operations guide](batch.md).

## PutItem

Creates a new item, or replaces an old item with a new item.
If an item that has the same primary key as the new item already exists in the specified table, the new item completely replaces the existing item.

```csharp
await ddbContext.PutItemAsync(new UserEntity("John", "Doe"));
```

Using the fluent API, you can perform a conditional put operation.
E.g., add a new item if one with the specified primary key doesn't exist, or replace an existing item if it has certain attribute values.

```csharp
await ddbContext.PutItem()
    .WithItem(new UserEntity("John", "Doe"))
    .WithCondition(Condition<UserEntity>.On(x => x.Pk).NotExists())
    .ExecuteAsync();
```

You can return the item's attribute values in the same operation by setting `ReturnValues` in fluent API.
It might be useful if you want to know the item's state before or after the save.
`ReturnValues` enum contains many options but only `None`, `AllOld`, and `AllNew` are supported by `PutItem` operation.

```csharp
var item = await ddbContext.PutItem()
    .WithItem(new UserEntity("John", "Doe"))
    .WithReturnValues(ReturnValues.AllOld)
    .ToEntityAsync();
```

For more info, check the detailed [PutItem API reference](../../api_reference/put-item.md)

## Update

TBD
