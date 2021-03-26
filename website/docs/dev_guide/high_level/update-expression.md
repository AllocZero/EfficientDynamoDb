---
id: update-expression
title: Building Update Expressions
slug: ../dev-guide/high-level/update-expression
---

This guide focuses on EfficientDynamoDb's API for building update expressions.
It's assumed that you are already familiar with update expressions in DynamoDb.
If not, please check out [official AWS docs](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.UpdateExpressions.html) for a better understanding of topics covered in this section.

## Overview

Much like condition expressions builder API, update expressions API in EfficientDynamoDb makes it easy to perform complex updates.
You don't need to think about reserved words, attribute names escaping, and other low-level DynamoDB stuff.

## Getting started

You can build update expression as part of the `UpdateItem` request.

```csharp
ddbContext.UpdateItem<EntityClass>()
    .On(x => x.FirstName).Assign("John")
    .ExecuteAsync();
```

`On(...)` accepts an expression that should point to a property marked by `DynamoDbProperty` attribute, element inside the collection, or the nested property of another object.

### Using value from another attribute

You can use another attribute in the update operation instead of an outside value.

```csharp
ddbContext.UpdateItem<EntityClass>()
    .On(x => x.LastName).Assign(x => x.FirstName)
    .ExecuteAsync();
```

Such overloads also allow providing a fallback value that will be used when an attribute not exists.

```csharp
ddbContext.UpdateItem<EntityClass>()
    .On(x => x.LastName).Assign(x => x.FirstName, "Fallback Name")
    .ExecuteAsync();
```

## Arithmetic operations

Use the `AssignSum` and `AssignSubtraction` methods to increment or decrement attributes.

```csharp
ddbContext.UpdateItem<EntityClass>()
    .On(x => x.Age).AssignSum(x => x.Age, 1) // increment age
    .On(x => x.CacheTtl).AssignSubtraction(x => x.CacheTtl, 10) // reduce cache TTL by 10 (seconds)
    .ExecuteAsync();
```

It's possible to assign a sum of two attributes to the third one:

```csharp
ddbContext.UpdateItem<EntityClass>()
    .On(x => x.Age).AssignSum(x => x.FirstProperty, x => x.SecondProperty)
    .ExecuteAsync();
```

Every expression may have a fallback value to handle the case when an attribute not exists.

## Collection operations

The main operation to concatenate collections is `AssignConcat(...)`.
It can be used for any possible concatenation scenario, but there are also `Append(...)` and `Prepend(...)` methods to simplify mutating the collection inplace.

Appending example. Both calls are doing the same thing:

```csharp
ddbContext.UpdateItem<EntityClass>()
    .On(x => x.Friends).AssignConcat(x => x.Friends, new [] {"New friend"})
    .ExecuteAsync();

ddbContext.UpdateItem<EntityClass>()
    .On(x => x.Friends).Append(new [] {"New friend"})
    .ExecuteAsync();
```

Prepending example. Both calls are doing the same thing:

```csharp
ddbContext.UpdateItem<EntityClass>()
    .On(x => x.Friends).AssignConcat(new [] {"New friend"}, x => x.Friends)
    .ExecuteAsync();

ddbContext.UpdateItem<EntityClass>()
    .On(x => x.Friends).Prepend(new [] {"New friend"})
    .ExecuteAsync();
```

To update `SET`-like collections, you can use the `Insert(...)` method:

```csharp
ddbContext.UpdateItem<EntityClass>()
    .On(x => x.UniqueValues).Insert(newUniqueValue)
    .ExecuteAsync();
```

### Removing from collections

Removing an element from a list can be done by index:

```csharp
ddbContext.UpdateItem<EntityClass>()
    .On(x => x.Friends[2]).Remove()
    .ExecuteAsync();
```

To remove elements from the set, you need to pass a subset to remove:

```csharp
ddbContext.UpdateItem<EntityClass>()
    .On(x => x.UniqueValues).Remove(new HashSet<int>() {1, 2, 3})
    .ExecuteAsync();
```

## Removing a property

You can remove any top-level property, nested property, or a list member by calling the parameterless `Remove()` method:

```csharp
ddbContext.UpdateItem<EntityClass>()
    .On(x => x.SomeProperty).Remove()
    .ExecuteAsync();

ddbContext.UpdateItem<EntityClass>()
    .On(x => x.Array[3]).Remove()
    .ExecuteAsync();

ddbContext.UpdateItem<EntityClass>()
    .On(x => x.TopLvl.Nested).Remove()
    .ExecuteAsync();
```
