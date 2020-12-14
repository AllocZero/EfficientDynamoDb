using System;
using System.Collections.Generic;
using EfficientDynamoDb.Internal.Mapping.Converters;

namespace EfficientDynamoDb.Internal.Mapping
{
    public static class DefaultDdbConverterFactory
    {
        public static DdbConverter Create(Type type)
        {
            return type switch
            {
                _ when type == typeof(string) => new StringDdbConverter(),
                _ when type == typeof(DateTime) => new DateTimeDdbConverter(),
                _ when type == typeof(int) => new IntDdbConverter(),
                _ when type == typeof(bool) => new BoolDdbConverter(),
                _ when type == typeof(HashSet<string>) => new StringSetDdbConverter(),
                _ when type == typeof(HashSet<int>) => new NumberSetDdbConverter(),
                _ when type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>)) => (DdbConverter) Activator.CreateInstance(
                    typeof(ListDdbConverter<>).MakeGenericType(type.GenericTypeArguments[0]), Create(type.GenericTypeArguments[0])),
                _ when type.IsClass => (DdbConverter) Activator.CreateInstance(typeof(NestedObjectConverter<>).MakeGenericType(type))
            };
        }
    }
}