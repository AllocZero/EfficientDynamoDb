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

Creates a new item or replaces an old item with a new item.
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
It might be helpful if you want to know the item's state before or after the save.
`ReturnValues` enum contains many options, but `PutItem` operation only supports `None`, `AllOld`, and `AllNew`.

```csharp
var item = await ddbContext.PutItem()
    .WithItem(new UserEntity("John", "Doe"))
    .WithReturnValues(ReturnValues.AllOld)
    .ToEntityAsync();
```

For more info, check the detailed [PutItem API reference](../../api_reference/put-item.md)

## Update

Edits an existing item's attributes or adds a new item to the table if it does not already exist.
You can put, delete, or add attribute values.

`Update` is a rather complicated operation, so you can only use a fluent API to access it.
The primary key and at least one update operation must be specified in every request.

EfficientDynamoDb provides an easy way to build update expressions.
All you need to do is pass `Expression` referring to the property you want to update to `.On(...)` method of fluent API.
And then follow it with the action you want to perform, e.g. `Assign(...)`.

```csharp
await ddbContext.Update<UserEntity>()
    .WithPrimaryKey("partitionKey", "sortKey")
    .On(x => x.FirstName).Assign("John")
    .On(x => x.LastName).Assign("Doe")
    .ExecuteAsync();
```

Please, refer to [UpdateExpression developer guide](update-expression.md) for more details about `UpdateExpression` builder usage and tricks.

You can also perform a conditional update on an existing item (insert a new attribute name-value pair if it doesn't exist,or replace an existing name-value pair if it has specific expected attribute values).

`Update` conditions are exactly the same as their `Put` condition equivalents both API-wise and behavior-wise.

```csharp
var condition = Condition<UserEntity>
    .On(x => x.FirstName)
    .NotEqualsTo("John");

await ddbContext.Update<UserEntity>()
    .WithPrimaryKey("partitionKey", "sortKey")
    .On(x => x.FirstName).Assign("John")
    .WithCondition(condition)
    .ExecuteAsync();
```

You can return the item's attribute values in the same operation by setting `ReturnValues` in fluent API.
It might be helpful if you want to know the item's state before or after the update.

```csharp
var oldValues = await ddbContext.Update<UserEntity>()
    .WithPrimaryKey("partitionKey", "sortKey")
    .On(x => x.FirstName).Assign("John")
    .WithReturnValues(ReturnValues.UpdatedOld)
    .ToEntityAsync();
```

For more info, check the detailed [Update API reference](../../api_reference/update.md)