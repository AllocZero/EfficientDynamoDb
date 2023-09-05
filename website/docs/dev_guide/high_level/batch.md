---
id: batch
title: Batch Operations
slug: ../../dev-guide/high-level/batch
---

DynamoDB provides two batch operations:

* `BatchGetItem` - Read multiple items from one or more tables.
* `BatchWriteItem` -  Put or delete multiple items in one or more tables.

EfficientDynamoDb automatically delays and retries if the batch operation returns unprocessed items,
which can happen when provisioned throughput is exceeded, the size limit is reached, or an internal DynamoDB error occurred.

## BatchGetItem

Reads up to 100 items in a single request.

Each entity's primary key is configured using `Batch.GetItem` factory method.

```csharp
 var items = await context.BatchGet()
     .WithItems(
         Batch.GetItem<EntityClass>().WithPrimaryKey("partitionKey", "sortKey_1"),
         Batch.GetItem<EntityClass>().WithPrimaryKey("partitionKey", "sortKey_2")
     )
     .ToListAsync<EntityClass>();
```

When strong consistency or a projection is needed, a more sophisticated `FromTables` method can be used:

```csharp
 var items = await context.BatchGet()
     .FromTables(
        Batch.FromTable<EntityClass>()
            .WithConsistentRead(true)
            .WithProjectedAttributes<ProjectionClass>()
            .WithItems(
                Batch.GetItem<EntityClass>().WithPrimaryKey("partitionKey", "sortKey_1"),
                Batch.GetItem<EntityClass>().WithPrimaryKey("partitionKey", "sortKey_2")
                )
    )
     .ToListAsync<EntityClass>();
```

*Entities of different types can be retrieved by using `AsDocuments()` method the same way as for other read operations.*

## BatchWriteItem

Puts or deletes up to 25 items in a single request.

Each write operation is configured using either `Batch.PutItem` or `Batch.DeleteItem` factory method.

```csharp
await context.BatchWrite()
    .WithItems(
        Batch.PutItem(new UserEntity("John", "Doe")),
        Batch.DeleteItem<UserEntity>().WithPrimaryKey("partitionKey", "sortKey")
    )
    .ExecuteAsync();
```
