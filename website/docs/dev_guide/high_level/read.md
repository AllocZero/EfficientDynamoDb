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

### Query Projection

TBD

### Query Document Returns

Sometimes, your queries return different entities in single response.
It happens frequently when you utilize single-table design.

Fluent API allows you to return `Document` objects instead of your entities which you can convert to correct entities in applications code.
E.g., consider the case, where a single query returns user's personal data and a list of his transactions.

```csharp
var documents = await query.ToDocumentListAsync();

var userInfoDocument = documents.First(x => x["sortKey"].StartsWith("userInfo#")); // sort key prefix determines the 'type' of item
var userInfo = ddbContext.ToObject<UserInfo>(userInfoDocument); // convert Document to entity class

var transactions = documents.Except(userInfoDocument) // assuming that all other items except user info are transactions
                            .Select(x => ddbContext.ToObject<UserTransaction>(x))
                            .ToList();
```

### Useful links

For more info, check the detailed [Query API reference](../../api_reference/query.md)

## Scanning the Data

The `Scan` operation iterates over the whole table and returns values that satisfy `FilterExpression` if set.
Fluent API is the only option for high-level scanning.

Unlike the `Query`, `Scan` API doesn't have a `ToListAsync()` method to encourage better table design for your DB and correct usage of scanning.
The closest replacement is `ToAsyncEnumerable()`

```csharp
var scan = ddbContext.Scan<EntityClass>()
    .WithFilterExpression(Condition<EntityClass>.On(x => x.Pk).EqualsTo("test"));
    .BackwardSearch(true)
    .WithLimit(100)
    .FromIndex("indexName")

await foreach (var item in scan.ToAsyncEnumerable())
{
    // Process an item here.
}
```

### Parallel Scan

DynamoDB supports parallel scans that are straightforward to use with EfficientDynamoDb.
All you need to do is to decide number of scanning segments and pass it in `ToParallelAsyncEnumerable(...)` method.

```csharp
var segmentsCount = 8;

await foreach (var item in scan.ToParallelAsyncEnumerable(segmentsCount))
{
    // Process an item here.
}
```

### Scan Pagination, Projection and Document returns

These features in `Scan` API are identical to corresponding ones in `Query` API.

Check query docs here:

* [Pagination](#query-pagination)
* [Projection](#query-projection)
* [Document returns](#query-document-returns)

Definitive desctiption of `Scan` operation is available on [Scan API reference](../../api_reference/scan.md) page.