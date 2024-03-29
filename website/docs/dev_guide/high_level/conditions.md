---
id: conditions
title: Building Conditions
slug: ../../dev-guide/high-level/conditions
---

This guide focuses on EfficientDynamoDb's API for building conditions.
It's assumed that you are already familiar with condition expressions in DynamoDB.
If not, please check out [official AWS docs](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.ConditionExpressions.html) and [comparison operators reference](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.OperatorsAndFunctions.html) for a better understanding of topics covered in this section.

## Overview

EfficientDynamoDb aims to simplify condition expression building by providing an abstraction over DynamoDB expressions syntax.

Benefits of our API:

* Removes the requirement of managing `ExpressionAttributeNames`, `ExpressionAttributeValues`, and handling reserved words.
* Easy refactoring and usage search in your favorite IDE.

## Getting started

The simplest way of creating a condition is using the `Condition<T>.On(...)` static factory method:

```csharp
var condition = Condition<EntityClass>.On(x => x.YourProperty).EqualTo(10);
```

`On(...)` accepts an expression that should point to a property marked by `DynamoDbProperty` attribute, element inside the collection, or the nested property of another object.

### Conditions on array elements

Condition for the specific element inside the collection:

```csharp
var condition = Condition<EntityClass>.On(x => x.YourList[3]).EqualTo(10);
```

Currently, you can only use number literals, constants, fields, or variables inside the indexer.
You can't use methods or properties to get the index.

If you need to get an index from the method, you can save it to a local variable first:

```csharp
// Correct
var index = GetIndex();
var condition = Condition<EntityClass>.On(x => x.YourList[index]).EqualTo(10);

// Incorrect
var condition = Condition<EntityClass>.On(x => x.YourList[GetIndex()]).EqualTo(10);
```

### Nested attributes

You may access the nested attributes of lists and objects.
E.g., the following condition is valid:

```csharp
var condition = Condition<EntityClass>.On(x => x.TopLvlProperty.NestedList[3].MoreNestedProperty).EqualTo(10);
```

### Comparison with other attributes

The majority of DynamoDB condition operations support comparison with other attributes instead of an explicit value.
You can pass an expression inside the operation method in the same way you do in `On(...)`:

```csharp
var condition = Condition<EntityClass>.On(x => x.SomeProperty).EqualTo(x => x.AnotherProperty);
```

Some operations like `Between` can even accept a combination of explicit values and attributes:

```csharp
var condition = Condition<EntityClass>.On(x => x.SomeProperty).EqualTo(minValueVariable, x => x.MaxValueProperty);
```

### Multiple conditions on a single entity

Often, you need to create multiple conditions on a single entity.
In this case, the alternative API may be handy:

```csharp
var filter = Condition.ForEntity<EntityClass>();

var firstCondition = filter.On(x => x.SomeProperty).EqualTo(10);
var secondCondition = filter.On(x => x.RareProperty).Exists();
```

You can use these conditions in separate requests or join them into a single condition which is explained in the following section.

## Joining multiple conditions

There are two ways of combining multiple conditions into one expression.

### Joiner API

Use any combination of `Joiner.And(...)` and `Joiner.Or(...)` methods to create a complex condition.

For example, the DynamoDB condition `#firstName = :firstName AND (#age < :lowerAgeLimit OR #age > :upperAgeLimit) AND begins_with(#lastName, :lastNamePrefix)` would look like this:

```csharp
var filter = Condition.ForEntity<EntityClass>();
var condition = Joiner.And(
        filter.On(x => x.FirstName).EqualTo(firstNameValue),
        Joiner.Or(
            filter.On(x => x.Age).LessThan(lowerAgeLimit),
            filter.On(x => x.Age).GreaterThan(upperAgeLimit)
        ),
        filter.On(x => x.LastName).BeginsWith(lastNamePrefix)
    );
```

### Logical operators API

You might find the Joiner API quite verbose and difficult to read when there are many `AND`/`OR` operators.
That's where logical operators come to the rescue.
Conditions in EfficientDynamoDb support logical `&` and `|` for combining multiple into one.

The same DDB expression from the Joiner API example looks like this:

```csharp
var filter = Condition.ForEntity<EntityClass>();
var condition = filter.On(x => x.FirstName).EqualTo(firstNameValue) 
    & (filter.On(x => x.Age).LessThan(lowerAgeLimit) | filter.On(x => x.Age).GreaterThan(upperAgeLimit)) 
    & filter.On(x => x.LastName).BeginsWith(lastNamePrefix)
```

Note that this API follows all logical operator rules, e.g., you can use parentheses to change execution order.

### Using both APIs together

It's possible to use both Joiner and Logical operators API together to build a single query.

```csharp
var filter = Condition.ForEntity<EntityClass>();
var condition = Joiner.And(
        filter.On(x => x.FirstName).EqualTo(firstNameValue),
        filter.On(x => x.Age).LessThan(lowerAgeLimit) | filter.On(x => x.Age).GreaterThan(upperAgeLimit),
        filter.On(x => x.LastName).BeginsWith(lastNamePrefix)
    );
```
