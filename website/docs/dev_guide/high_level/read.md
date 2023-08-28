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

## Retrieving an item

To read an item from a DynamoDB table, use the `GetItem` operation.
You must provide a type marked by the [DynamoDbTable](attributes.md#dynamodbtable) attribute.

```csharp
var item = await ddbContext.GetItemAsync<EntityClass>("partitionKey");
```

You must specify the *entire* primary key, not just part of it.
For example, if a table has a composite primary key (partition key and sort key), you must supply a value for the partition key and a value for the sort key.

```csharp
var item = await ddbContext.GetItemAsync<EntityClass>("partitionKey", "sortKey");
```

You can use the fluent API when you need better control over the operation behavior.

```csharp
var item = await ddbContext.GetItem<EntityClass>()
    .WithConsistentRead(true)
    .WithPrimaryKey("partitionKey", "sortKey")
    .ToItemAsync();
```

## Querying data

The `Query` operation in Amazon DynamoDB finds items based on primary key values.

Since `Query` is a rather complicated operation, you can only use the fluent API to perform it.
You must provide the `KeyExpression` in every request.

```csharp
var condition = Condition<EntityClass>.On(x => x.Pk).EqualTo("test");

var items = await ddbContext.Query<EntityClass>()
    .WithKeyExpression(condition)
    .ToListAsync();
```

Instead of creating `Condition<T>` explicitly, you can utilize the overload that accepts `Func<EntityFilter<TEntity>, FilterBase>`:

```csharp
var items = await ddbContext.Query<EntityClass>()
    .WithKeyExpression(cond => cond.On(item => item.Pk).EqualTo("test"))
    .ToListAsync();
```

DynamoDB can only return up to 1 MB of data per response.
If your query contains more, DynamoDB will paginate the response.
In this case, `ToListAsync()` makes multiple calls until all the data is fetched and put into a single resulting array.

Check the [condition building guide](conditions.md) for detailed information about the condition builder API.

### Using Query with GSIs and LSIs (Global and Local Secondary Indexes)

DynamoDB supports two types of indexes: Global Secondary Indexes (GSIs) and Local Secondary Indexes (LSIs).
EfficientDynamoDb can utilize both types of indexes using the same API.
For both types, use `.FromIndex(string indexName)` method to run the query against the index.

Example of `Query` on GSI or LSI:

```csharp
var items = await ddbContext.Query<EntityClass>()
    .FromIndex("IndexName")
    .WithKeyExpression(c => c.On(item => item.IndexPk).EqualTo("IndexPartitionKeyValue"))
    .ToListAsync();
```

## Scanning data

The `Scan` operation iterates over the whole table and returns values that satisfy `FilterExpression`, if provided.
The Fluent API is the only option for high-level scanning.

Unlike `Query`, the `Scan` API doesn't have a `ToListAsync()` method to encourage better table design for your DB and correct scanning usage.
The closest replacement is `ToAsyncEnumerable()`.

```csharp
var scan = ddbContext.Scan<EntityClass>();

await foreach (var item in scan.ToAsyncEnumerable())
{
    // Process an item here.
}
```

### Parallel Scan

DynamoDB supports parallel scans which are straightforward to use with EfficientDynamoDb.
All you need to do is decide the number of scanning segments and pass it in the `ToParallelAsyncEnumerable(...)` method.

```csharp
var scan = ddbContext.Scan<EntityClass>();
var segmentsCount = 8;

await foreach (var item in scan.ToParallelAsyncEnumerable(segmentsCount))
{
    // Process an item here.
}
```

### Using Scan with GSIs and LSIs (Global and Local Secondary Indexes)

DynamoDB supports two types of indexes: Global Secondary Indexes (GSIs) and Local Secondary Indexes (LSIs).
EfficientDynamoDb can utilize both types of indexes using the same API.
For both types, use `.FromIndex(string indexName)` method to run the query against the index.

Example of `Scan` on GSI or LSI:

```csharp
var scan = ddbContext.Scan<EntityClass>().FromIndex("IndexName");

await foreach (var item in scan.ToAsyncEnumerable())
{
    // Process an item here.
}
```

## Document returns

Sometimes, your queries return different entities in a single response.
This frequently happens when using a single-table design.

The Fluent API allows you to return `Document` objects instead of your entities which you can convert to correct entities in applications code.
Just call the `AsDocument()` (for `GetItem`) or `AsDocuments()` (for `Query` and `Scan`) anywhere in the call chain before the executing method
(e.g., `ToItemAsync()` for `GetItem`, `ToListAsync()` for `Query`, etc.)

For example, consider the case when a single query returns the user's profile data and a list of his transactions.

Retrieving documents using the `Query` operation:

```csharp
var condition = Condition<EntityClass>.On(x => x.Pk).EqualTo("test");

var documents = await ddbContext.Query<EntityClass>()
    .WithKeyExpression(condition)
    .AsDocuments()
    .ToListAsync();
```

Mapping documents to entities can be done by calling the `Document.ToObject<T>()` method:

```csharp
// sort key prefix determines the 'type' of item
var userInfoDocument = documents.First(x => x["sortKey"].StartsWith("userInfo#"));

// convert Document to entity class
var userInfo = ddbContext.ToObject<UserInfo>(userInfoDocument); 

// assuming that all other items except user info are transactions
var transactions = documents.Except(userInfoDocument) 
    .Select(x => ddbContext.ToObject<UserTransaction>(x))
    .ToList();
```

`GetItem` document example:

```csharp
var item = await ddbContext.GetItem<EntityClass>()
    .WithPrimaryKey("partitionKey", "sortKey")
    .AsDocument()
    .ToItemAsync();
```

`Scan` example:

```csharp
var scan = ddbContext.Scan<EntityClass>().AsDocuments();

await foreach (var item in scan.ToAsyncEnumerable())
{
    // Process an item here.
}
```

## Projections

Use projections to retrieve only specific attributes of item(s).
All read operations support projection using the same API.

Use the `AsProjection<TProjection>()` method to get a projection to the specified class.

**Projected class and its properties must be marked with corresponding attributes in the same way as entities are marked!**

```csharp
var projectedItem = await ddbContext.GetItem<EntityClass>()
    .WithPrimaryKey("partitionKey", "sortKey")
    .AsProjection<ProjectionClass>()
    .ToItemAsync()
```

Use the `WithProjectedAttributes(...)` method if you don't want to create a separate projection class.
When this method is used, the response will keep the original entity class but pull and populate only specified attributes.

**Passing the same property multiple times is not allowed!**

```csharp
var item = await ddbContext.GetItem<EntityClass>()
    .WithPrimaryKey("partitionKey", "sortKey")
    .WithProjectedAttributes(x => x.FirstName, x => x.LastName)
    .ToItemAsync()
```

## Indexing

DynamoDB supports two types of indexes: Global Secondary Indexes (GSIs) and Local Secondary Indexes (LSIs).
`Scan` and `Query` operations can utilize both types of indexes using the same API in EfficientDynamoDb.
In both cases, use `.FromIndex(string indexName)` method to run your request against the index.

Example of `Query` on GSI or LSI:

```csharp
var items = await ddbContext.Query<EntityClass>()
    .FromIndex("IndexName")
    .WithKeyExpression(c => c.On(item => item.IndexPk).EqualTo("IndexPartitionKeyValue"))
    .ToListAsync();
```

Example of `Scan` on GSI or LSI:

```csharp
var items = await ddbContext.Scan<EntityClass>()
    .FromIndex("IndexName")
    .WithFilterExpression(c => c.On(item => item.IndexPk).EqualTo("IndexPartitionKeyValue"))
    .ToListAsync();
```
## Pagination

`Scan` and `Query` have two ways of handling paginated requests.
APIs for both operations are the same, so that the following examples will show only `Query` for the sake of simplicity.

The easiest way to handle a paginated request manually is to use `ToAsyncEnumerable()`.

```csharp
await foreach (var item in query.ToAsyncEnumerable())
{
    // Process an item here.
}
```

There are also cases when you might need to manage pagination tokens yourself.
To do so, use the `ToPageAsync()` to get the pagination token in response and then pass it to the subsequent request.

```csharp
var page = await query.ToPageAsync();

var nextPage = await query.WithPaginationToken(page.PaginationToken)
    .ToPageAsync();
```

Note: *Due to the internals of the DynamoDB, `page.Items` being empty doesn't mean that there are no more data to read.*
*The only way to know that all data is retrieved is by checking the `page.PaginationToken` value. It is `null` when there are no more items to pull*.

## Filtering

DynamoDB supports filtering results returned by `Scan` and `Query` by providing a `FilterExpression` in requests.
EfficientDynamoDb provides the same API for specifying filters for both operations:

```csharp
var condition = Condition<EntityClass>.On(x => x.FirstName).EqualTo("John");

var scan = ddbContext.Scan<EntityClass>()
    .WithFilterExpression(condition);

await foreach (var item in scan.ToAsyncEnumerable())
{
    // Process an item here.
}
```

Keep in mind that filtering doesn't reduce your RCU consumption, but it reduces transferred data size, thus reducing latency and network usage.

The [Conditions builder API](conditions.md) for filter expressions is the same API used for key expressions.

## Useful links

* API references
  * [GetItem](../../api_reference/get-item.md)
  * [Query](../../api_reference/query.md)
  * [Scan](../../api_reference/scan.md)
* [Condition Builder guide](conditions.md)
