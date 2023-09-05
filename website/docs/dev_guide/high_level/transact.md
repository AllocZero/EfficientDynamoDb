---
id: transact
title: Transactions
slug: ../../dev-guide/high-level/transact
---

DynamoDB provides two transactional operations:

* `TransactGetItems` - Atomically read multiple items from one or more tables.
* `TransactWriteItems` -  Atomically modify multiple items in one or more tables.

Both operations throw `TransactionCanceledException` when transaction is rejected.
The `CancellationReasons` property can be used to find out the reason behind the rejection.

## TransactGetItems

Atomically retrieves up to 25 items from one or more tables within the same AWS account and Region.

Each entity primary key is configured using `Transact.GetItem` factory method.

```csharp
var items = await context.TransactGet()
    .WithItems(
        Transact.GetItem<EntityClass>().WithPrimaryKey("partitionKey", "sortKey_1"),
        Transact.GetItem<EntityClass>().WithPrimaryKey("partitionKey", "sortKey_2")
    )
    .ToListAsync<MixedEntity>()
```

*Entities of different type can be retrieved by using `AsDocuments()` method the same way as for other read operations.*

## TransactWriteItems

Atomically applies one of four operations per item within the same AWS account and Region (up to 25 operations):
* `Transact.PutItem` - applies a [PutItem](./write.md#putitem) operation.
* `Transact.UpdateItem` - applies an [UpdateItem](./write.md#update) operation.
* `Transact.DeleteItem` - applies a [DeleteItem](./write.md#delete) operation.
* `Transact.ConditionCheck` - applies a condition to an item which is not modified by the transaction.

```csharp
await context.TransactWrite()
    .WithClientRequestToken(idempotencyKey)
    .WithItems(
        Transact.PutItem(new UserEmailEntity("test@test.com")),
        Transact.ConditionCheck<UserEntity>()
            .WithPrimaryKey("partitionKey", "sortKey")
            .WithCondition(Condition<UserEntity>.On(x => x.Verified).EqualTo(false))
    )
    .ExecuteAsync();
```