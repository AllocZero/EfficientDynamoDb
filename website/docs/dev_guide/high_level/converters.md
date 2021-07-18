---
id: converters
title: Converters
slug: ../dev-guide/high-level/converters
---

A converter is a class that converts a .NET type to and from DynamoDB JSON or low-level `Document` object. A custom converter allows working with unsupported types or overriding the default converter behavior.

Converters, along with with DynamoDB JSON parsing, are one of the most critical components from a performance perspective.
All **EfficientDynamoDb** built-in converters are optimized separately for both `Document` and JSON conversion, in order to allocate no additional memory.

## Built-in converters

EfficientDynamoDb does not require specifying a converter explicitly for the following built-in types:
* Classes 
* Strings
* Numbers: `byte`, `short`, `int`, `long`, `decimal`, `float`, `double`, `ushort`, `uint`, `ulong`
* Enums (saved as numbers)
* DateTimes (saved in ISO8601 format)
* Guids
* Booleans
* Collections: arrays, lists, dictionaries, sets (including their read-only and mutable interfaces)
* `AttributeValue` structs (low-level API representation of the DynamoDB attribute)

In addition, you can use one of the following converters to change the default behavior:
* `StringEnumDdbConverter<T>` - saves enums as strings instead of numbers.
* `DateTimeDdbConverter` - allows to customize `DateTime` formatting parameters: `Format`, `DateTimeStyles` and `CultureInfo`.
* `SdkDateTimeDdbConverter` - makes `DateTime` behavior backward compatible with the official AWS SDK.

## Applying converters

### For a property

```csharp
[DynamoDbProperty("address", typeof(CompositeAddressConverter))]
public Address Address { get; set; }
```

### For a type

```csharp
[DynamoDbConverter(typeof(CompositeAddressConverter))]
public struct Address { ... }
```

### For a context

```csharp
var config = new DynamoDbContextConfig(regionEndpoint, awsCredentials)
{
    Converters = new[] {new CompositeAddressConverter()}
};
```

If a converter can't be instantiated in advance and depends on the target value type, a custom converter factory can be implemented by inheriting from the `DdbConverterFactory` class and registering it with the context the same way as other custom converters.

For example, a string enum converter factory can be defined like this:

```csharp
public sealed class StringEnumDdbConverterFactory : DdbConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

    public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata)
    {
        return (DdbConverter) Activator.CreateInstance(typeof(StringEnumDdbConverter<>).MakeGenericType(typeToConvert));
    }
}
```

## Basic converter

To create a custom converter:

* Inherit from `DdbConverter<TValue>` class.
* Implement both `Read` and `Write` methods.

```csharp
public class CompositeAddressConverter : DdbConverter<Address>
{
    // Converts DynamoDb attribute to the .NET type
    public override Address Read(in AttributeValue attributeValue)
    {
        var parts = attributeValue.AsString().Split('#');
        return new Address(parts[0], parts[1]);
    }

    // Converts .NET type to the DynamoDb attribute
    public override AttributeValue Write(ref Address address)
    {
        return new StringAttributeValue($"{address.Country}#{address.Street}");
    }
}
```

*Check out the [working with documents](../low-level.md#working-with-documents) chapter to better understand how to work with attribute values.*

## Direct JSON converter

Not all .NET types map nicely to DynamoDb attributes. Creation of intermediate `AttributeValue` struct can involve unnecessary allocations that can be avoided by reading/writing directly into JSON buffer.
In case when a custom type can't be converted to the `AttributeValue` without allocations, it is possible to implement two additional low-level `Read` and `Write` methods that work with JSON buffers.
During deserialization/serialization of entities to JSON, more optimized low-level implementations will be called.

```csharp
public class CustomIntConverter : DdbConverter<int>
{
    // Efficient zero-allocation JSON to int conversion
    public override int Read(ref DdbReader reader)
    {
        if (!Utf8Parser.TryParse(reader.JsonReader.ValueSpan, out int value, out _))
            throw new DdbException($"Couldn't parse int ddb value from '{reader.JsonReaderValue.GetString()}'.");

        return value;
    }
    
    // Efficient zero-allocation int to JSON conversion
    public override void Write(in DdbWriter writer, ref int value)
    {
         writer.JsonWriter.WriteStartObject();
         writer.JsonWriter.WriteString(DdbTypeNames.Number, value);
         writer.JsonWriter.WriteEndObject();
    }
    
    public override int Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToInt();
    
    public override AttributeValue Write(ref T value) => new NumberAttributeValue(value.ToString());   
}
```

**EfficientDynamoDb** uses `System.Text.Json` for all JSON manipulations.

### JSON reading

When a low-level read is called, `DdbReader.JsonReader` is already pointed to the JSON value. Current attribute type is automatically parsed and can be accessed using `DdbReader.AttributeType` property.

The `DdbReader.JsonReader.Read` method should not be explicitly called unless you are writing a converter for a non-primitive JSON type like an object or array.

### JSON writing

When a low-level write is called, a converter has to write DynamoDb JSON, including the attribute type.
`DdbWriter` class provides various simplified overloads that write attribute types automatically. But in case if suitable overload does not exist, the attribute type has to be written manually like in the `CustomIntConverter` example above.

## Sparse converters

Sparse converters don't save certain values and completely remove an attribute instead. It is a powerful concept that can be used for various purposes like size savings or to conditionally include an entity in the GSI.

By default, all built-in converters act as sparse converters when it comes to handling `null` values, meaning that `null` properties are never saved and the entire attribute is deleted.

To add an additional sparse condition, `ShouldWrite` method has to be overridden. For example. here is a simple sparse int converter:

```csharp
public class SparseIntConverter : DdbConverter<int>
{
    public override bool ShouldWrite(ref int value) => value != 0;
    
    ...
}
```

Note: Sparse converters don't remove attributes when they are part of a `Dictionary` class.

## Set converters

Both string and number sets store values as strings in the DB.
To store a custom type inside a set, a converter should implement the `ISetValueConverter<T>` interface:

```csharp
public class CustomDdbConverter : DdbConverter<CustomType>, ISetValueConverter<CustomType>
{
    public string WriteStringValue(ref CustomType value) => value.ToString();
    
    // Optionally implement direct write method
    public void WriteStringValue(in DdbWriter ddbWriter, ref CustomType value) => 
        ddbWriter.JsonWriter.WriteStringValue(value.AsSpan());
}
```

## Dictionary key converters

To store a custom type as a dictionary key, a converter should implement the `IDicitonaryKeyConverter<T>` interface:

```csharp
public class CustomDdbConverter : DdbConverter<CustomType>, ISetValueConverter<CustomType>
{
    public string WriteStringValue(ref CustomType value) => value.ToString();
    
    // Optionally implement direct write method
    public void WritePropertyName(in DdbWriter ddbWriter, ref CustomType value) => 
        ddbWriter.JsonWriter.WritePropertyName(value.AsSpan());
}
```
