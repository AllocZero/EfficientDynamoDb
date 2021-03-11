---
id: read
title: Reading Data
slug: ../dev-guide/high-level/read
---

DynamoDB provides three main operations for reads:

* `GetItem` - Read a single item.
* `Query` - Get items from a single partition based on provided query expression.
* `Scan` - Get all items from a table with a possibility to filter some of the results out.

DynamoDB also supports a `BatchGetItem` operation for executing up to 100 `GetItem` operations in a single request.
It's covered in [batch operations guide](batch.md).

## Reading an Item

To read an item from a DynamoDB table, use the `GetItem` operation.
You must provide a type marked by [DynamoDBTable](attributes.md#dynamodbtable) attribute.

```csharp
var item = await ddbContext.GetItemAsync<EntityClass>("partitionKey");
```

You must specify the *entire* primary key, not just part of it.
For example, if a table has a composite primary key (partition key and sort key), you must supply a value for the partition key and a value for the sort key.

```csharp
var item = await ddbContext.GetItemAsync<EntityClass>("partitionKey", "sortKey");
```

You can also use the fluent API when you need better control over the operation behavior.

```csharp
var item = await ddbContext.GetItem<EntityClass>()
    .WithConsistentRead(true)
    .WithPrimaryKey("partitionKey", "sortKey")
    .ToEntityAsync();
```

For more info, check the detailed [GetItem API reference](../../api_reference/get-item.md)

## Querying the Data

The `Query` operation in Amazon DynamoDB finds items based on primary key values.

Since `Query` is a rather complicated operation, you can only use fluent API to perform it.
You must provide the `KeyExpression` in every request.

```csharp
var query = ddbContext.Query<EntityClass>()
    .WithKeyExpression(Condition<EntityClass>.On(x => x.Pk).EqualsTo("test"));
    .BackwardSearch(true)
    .WithLimit(100)
    .FromIndex("indexName")

var items = await query.ToListAsync();
```

DynamoDB can only return up to 1 MB of data per response.
If your query contains more, DynamoDB will paginate the response.
In this case, `ToListAsync()` makes multiple calls until all the data is fetched and put into a single resulting array.

### Query Pagination

The easiest way to handle a paginated request manually is to use `ToAsyncEnumerable()` instead of `ToListAsync()`.

```csharp
await foreach (var item in query.ToAsyncEnumerable())
{
    // Process an item here.
}
```

There are also cases when you might need to manage pagination tokens yourself.
To do so, use the `ToPageAsync()` to get the pagination token in response and then pass it to the next request.

```csharp
var page = await query.ToPageAsync();

var nextPage = await query.WithPaginationToken(page.PaginationToken).ToPageAsync();
```

Note: *Due to the internals of the DynamoDB, `page.Items` being empty doesn't mean that there are no more data to read.*
*The only way to know that all data is retrieved is by checking the `page.PaginationToken` value. It is `null` when there are no more items to pull*.

### Useful links

For more info, check the detailed [Query API reference](../../api_reference/query.md)

## Scanning the Data

The `Scan` operation iterates over the whole table and returns values that satisfy `FilterExpression` if set.
Fluent API is the only option for high-level scanning.

```csharp
var scan = ddbContext.Scan<EntityClass>()
    .WithFilterExpression(Condition<EntityClass>.On(x => x.Pk).EqualsTo("test"));
    .BackwardSearch(true)
    .WithLimit(100)
    .FromIndex("indexName")

var items = await scan.ToListAsync();
```

Much like the `Query` response, the `Scan` response can only contain up to 1 MB of data.
`ToListAsync()` makes multiple calls until all the data is fetched and put into a single resulting array.

### Scan Pagination

Pagination of `Scan` operation works identical to the `Query`.
Check the [Query pagination section](#query-pagination) for description and examples.

Definitive desctiption of `Scan` operation is available on [Scan API reference](../../api_reference/scan.md) page
