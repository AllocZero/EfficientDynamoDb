---
id: low-level
title: Low-Level API
slug: ../dev-guide/low-level
---

**EfficientDynamoDb** provides access to DynamoDb HTTP API through `DynamoDbLowLevelContext` class, which can be accessed from `DynamoDbContext.LowLevel` property.

All low-level operations accept a request object as input and return a single response:

```csharp title="GetItem Request"
var response = await context.LowLevel.GetItemAsync(new GetItemRequest
{
    TableName = "users",
    Key = new PrimaryKey("partitionKey", "sortKey")
});
var document = response.Item;
```

## Working with Documents

In the low-level API, entities are represented in the form of the `Document` class.
It inherits `Dictionary<string, AttributeValue>` and has all dictionary lookup methods, including an indexer for efficient attribute access.

`AttributeValue` is a readonly struct representing a single DynamoDb attribute.
Overall there are 10 different attribute types supported by the database:

* `StringAttributeValue` - string.
* `NumberAttributeValue` - number.
* `BinaryAttributeValue` - binary.
* `BooleAttributeValue` - boolean.
* `NullAttributeVlaue` - null.
* `MapAttributeValue` - map.
* `ListAttributeValue` - list.
* `StringSetAttributeValue` - string set.
* `NumberSetAttributeValue` - number set.
* `BinarySetAttributeValue` - binary set.

### Accessing values

To understand what exact type is stored inside an `AttributeValue` instance, the `Type` property can be used.
To retrieve the underlying value itself, use one of the `As` methods.

For example, a numeric value can be accessed in a couple of ways:

```csharp
var numberAttribute = attributeValue.AsNumberAttribute();

var stringValue = numberAttribute.Value;
var intValue = numberAttribute.ToInt32();
```

*A separate `ToInt32` call is required because in DynamoDb numbers are stored as strings and the final type depends on the application.
Types that don't require any type conversions can be accessed directly using methods like `AsString()`, `AsBool()` or `AsDocument()`.*

### Creating an attribute

All attribute value types have implicit operators implemented, meaning there is no need to explicitly create the `AttributeValue`.
Instead, just assign the underlying attribute value type:

```csharp
var document = new Document 
{
    {"age", new NumberAttributeValue("30")}, // Using constructor and implicit conversion to AttributeValue
    {"first_name", "John"}, // Using implicit conversion from string to AttributeValue
};
```
