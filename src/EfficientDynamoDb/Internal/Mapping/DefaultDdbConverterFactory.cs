using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal.Mapping.Converters;
using EfficientDynamoDb.Internal.Mapping.Converters.Collections;
using EfficientDynamoDb.Internal.Mapping.Converters.Collections.NumberSet;
using EfficientDynamoDb.Internal.Mapping.Converters.Primitives;
using EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Enums;
using EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers;

namespace EfficientDynamoDb.Internal.Mapping
{
    internal static class DefaultDdbConverterFactory
    {
        private static readonly ConcurrentDictionary<Type, DdbConverter> ConvertersCache = new ConcurrentDictionary<Type, DdbConverter>();
        private static readonly ConcurrentDictionary<Type, DdbConverter> ConvertersFromTypeCache = new ConcurrentDictionary<Type, DdbConverter>();

        public static DdbConverter Create(Type converterType) => ConvertersCache.GetOrAdd(converterType, x => (DdbConverter) Activator.CreateInstance(x));

        public static DdbConverter CreateFromType(Type sourceType)
        {
            return ConvertersFromTypeCache.GetOrAdd(sourceType, st =>
            {
                var (type, isNullable) = GetTypeInfo(st);

                var converter = type switch
                {
                    _ when type == typeof(string) => Create<StringDdbConverter>(),
                    _ when type == typeof(DateTime) => Create<DateTimeDdbConverter>(),
                    _ when type == typeof(int) => Create<IntDdbConverter>(),
                    _ when type == typeof(double) => Create<DoubleDdbConverter>(),
                    _ when type == typeof(long) => Create<LongDdbConverter>(),
                    _ when type == typeof(decimal) => Create<DecimalDdbConverter>(),
                    _ when type == typeof(bool) => Create<BoolDdbConverter>(),
                    _ when type.IsEnum => CreateEnumConverter(type),
                    _ when IsStringSet(type) => Create<StringSetDdbConverter>(),
                    _ when IsNumberSet(type) => CreateNumberSetConverter(type),
                    _ when IsList(type) => CreateListConverter(type),
                    _ when IsArray(type) => CreateArrayConverter(type),
                    _ when type.IsClass => Create(typeof(NestedObjectConverter<>).MakeGenericType(type)),
                    _ when type == typeof(byte) => Create<ByteDdbConverter>(),
                    _ when type == typeof(short) => Create<ShortDdbConverter>(),
                    _ when type == typeof(ushort) => Create<UShortDdbConverter>(),
                    _ when type == typeof(uint) => Create<UIntDdbConverter>(),
                    _ when type == typeof(ulong) => Create<ULongDdbConverter>(),
                    _ when type == typeof(float) => Create<FloatDdbConverter>(),
                    _ when IsDictionary(type) => CreateDictionaryConverter(type),
                    _ => throw new DdbException($"Type '{type.Name}' requires an explicit ddb converter.")
                };

                if (!type.IsValueType || !isNullable)
                    return converter;

                var converterType = typeof(NullableValueTypeConverter<>).MakeGenericType(type);
                return ConvertersCache.GetOrAdd(converterType, (x, c) => (DdbConverter) Activator.CreateInstance(x, c), converter);
            });
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static DdbConverter Create<TConverter>() where TConverter : DdbConverter, new() => ConvertersCache.GetOrAdd(typeof(TConverter), x => new TConverter());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (Type Type, bool IsNullable) GetTypeInfo(Type sourceType)
        {
            var type = Nullable.GetUnderlyingType(sourceType);
            var isNullable = true;
            if (type is null)
            {
                isNullable = false;
                type = sourceType;
            }

            return (type, isNullable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static DdbConverter CreateEnumConverter(Type type)
        {
            var underlyingType = type.GetEnumUnderlyingType();

            return underlyingType switch
            {
                _ when underlyingType == typeof(int) => Create(typeof(IntEnumDdbConverter<>).MakeGenericType(type)),
                _ when underlyingType == typeof(byte) => Create(typeof(ByteEnumDdbConverter<>).MakeGenericType(type)),
                _ when underlyingType == typeof(short) => Create(typeof(ShortEnumDdbConverter<>).MakeGenericType(type)),
                _ when underlyingType == typeof(long) => Create(typeof(LongEnumDdbConverter<>).MakeGenericType(type)),
                _ when underlyingType == typeof(uint) => Create(typeof(UIntEnumDdbConverter<>).MakeGenericType(type)),
                _ when underlyingType == typeof(ushort) => Create(typeof(UShortEnumDdbConverter<>).MakeGenericType(type)),
                _ when underlyingType == typeof(ulong) => Create(typeof(ULongEnumDdbConverter<>).MakeGenericType(type)),
                _ => throw new DdbException($"Type '{type.Name}' requires an explicit ddb converter.")
            };
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsStringSet(Type type) => type == typeof(HashSet<string>) || type == typeof(ISet<string>);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNumberSet(Type type)
        {
            if (!type.IsGenericType)
                return false;
            
            var genericType = type.GetGenericTypeDefinition();
            var isSet =  genericType == typeof(HashSet<>) || type == typeof(ISet<>);
            if (!isSet)
                return false;

            var elementType = type.GenericTypeArguments[0];
            switch (elementType)
            {
                case var _ when elementType == typeof(int):
                case var _ when elementType == typeof(uint):
                case var _ when elementType == typeof(long):
                case var _ when elementType == typeof(ulong):
                case var _ when elementType == typeof(short):
                case var _ when elementType == typeof(ushort):
                case var _ when elementType == typeof(byte):
                case var _ when elementType == typeof(float):
                case var _ when elementType == typeof(double):
                case var _ when elementType == typeof(decimal):
                    return true;
                default:
                    return false;
            }
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static DdbConverter CreateNumberSetConverter(Type type)
        {
            var elementType = type.GenericTypeArguments[0];

            return elementType switch
            {
                _ when elementType == typeof(int) => Create<IntNumberSetDdbConverter>(),
                _ when elementType == typeof(uint) => Create<UIntNumberSetDdbConverter>(),
                _ when elementType == typeof(long) => Create<LongNumberSetDdbConverter>(),
                _ when elementType == typeof(ulong) => Create<ULongNumberSetDdbConverter>(),
                _ when elementType == typeof(short) => Create<ShortNumberSetDdbConverter>(),
                _ when elementType == typeof(ushort) => Create<UShortNumberSetDdbConverter>(),
                _ when elementType == typeof(byte) => Create<ByteNumberSetDdbConverter>(),
                _ when elementType == typeof(float) => Create<FloatNumberSetDdbConverter>(),
                _ when elementType == typeof(double) => Create<DoubleNumberSetDdbConverter>(),
                _ when elementType == typeof(decimal) => Create<DecimalNumberSetDdbConverter>(),
                _ => throw new DdbException($"Type '{type.Name}' requires an explicit ddb converter.")
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsList(Type type)
        {
            if (!type.IsGenericType)
                return false;

            var genericType = type.GetGenericTypeDefinition();
            return genericType == typeof(List<>) || genericType == typeof(IList<>) || genericType == typeof(ICollection<>);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static DdbConverter CreateListConverter(Type type)
        {
            var converterType = typeof(ListDdbConverter<>).MakeGenericType(type.GenericTypeArguments[0]);

            return ConvertersCache.GetOrAdd(converterType, (x, t) => (DdbConverter) Activator.CreateInstance(x, CreateFromType(t.GenericTypeArguments[0])), type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsArray(Type type)
        {
            if (type.IsArray)
                return true;

            if (!type.IsGenericType)
                return false;

            var genericType = type.GetGenericTypeDefinition();
            return genericType == typeof(IReadOnlyCollection<>) || genericType == typeof(IReadOnlyList<>);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static DdbConverter CreateArrayConverter(Type type)
        {
            var elementType = type.IsArray ? type.GetElementType()! : type.GetGenericArguments()[0]!;
            var converterType = typeof(ArrayDdbConverter<>).MakeGenericType(elementType);

            return ConvertersCache.GetOrAdd(converterType, (x, t) => (DdbConverter) Activator.CreateInstance(x, CreateFromType(t)), elementType);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsDictionary(Type type)
        {
            if (!type.IsGenericType)
                return false;

            var genericType = type.GetGenericTypeDefinition();
            var isDictionary = genericType == typeof(Dictionary<,>) || genericType == typeof(IDictionary<,>) || genericType == typeof(IReadOnlyDictionary<,>);

            return isDictionary && genericType.GenericTypeArguments[0] == typeof(string);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static DdbConverter CreateDictionaryConverter(Type type)
        {
            var converterType = typeof(DictionaryDdbConverter<>).MakeGenericType(type.GenericTypeArguments[0], type.GenericTypeArguments[1]);
            
            return ConvertersCache.GetOrAdd(converterType, (x, t) => (DdbConverter) Activator.CreateInstance(x, CreateFromType(t.GenericTypeArguments[1])), type);
        }
    }
}