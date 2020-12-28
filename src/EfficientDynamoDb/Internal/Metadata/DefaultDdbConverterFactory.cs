using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal.Converters;
using EfficientDynamoDb.Internal.Converters.Collections;
using EfficientDynamoDb.Internal.Converters.Collections.Dictionaries;
using EfficientDynamoDb.Internal.Converters.Primitives;
using EfficientDynamoDb.Internal.Converters.Primitives.Enums;
using EfficientDynamoDb.Internal.Converters.Primitives.Numbers;

namespace EfficientDynamoDb.Internal.Metadata
{
    internal static class DefaultDdbConverterFactory
    {
        private static readonly ConcurrentDictionary<Type, DdbConverter> ConvertersCache = new ConcurrentDictionary<Type, DdbConverter>();
        private static readonly ConcurrentDictionary<Type, DdbConverter?> ConvertersFromTypeCache = new ConcurrentDictionary<Type, DdbConverter?>();

        public static DdbConverter Create(Type converterType) => ConvertersCache.GetOrAdd(converterType, x => (DdbConverter) Activator.CreateInstance(x));

        public static DdbConverter? CreateFromType(Type sourceType)
        {
            return ConvertersFromTypeCache.GetOrAdd(sourceType, st =>
            {
                var (type, isNullable) = GetTypeInfo(st);

                // TODO: Add DateTimeOffset converters
                var converter = type switch
                {
                    _ when type == typeof(string) => Create<StringDdbConverter>(),
                    _ when type == typeof(DateTime) => Create<DateTimeDdbConverter>(),
                    _ when type == typeof(int) => Create<IntDdbConverter>(),
                    _ when type == typeof(double) => Create<DoubleDdbConverter>(),
                    _ when type == typeof(long) => Create<LongDdbConverter>(),
                    _ when type == typeof(decimal) => Create<DecimalDdbConverter>(),
                    _ when type == typeof(bool) => Create<BoolDdbConverter>(),
                    _ when type == typeof(Guid) => Create<GuidDdbConverter>(),
                    _ when type.IsEnum => CreateEnumConverter(type),
                    _ when type == typeof(byte) => Create<ByteDdbConverter>(),
                    _ when type == typeof(short) => Create<ShortDdbConverter>(),
                    _ when type == typeof(ushort) => Create<UShortDdbConverter>(),
                    _ when type == typeof(uint) => Create<UIntDdbConverter>(),
                    _ when type == typeof(ulong) => Create<ULongDdbConverter>(),
                    _ when type == typeof(float) => Create<FloatDdbConverter>(),
                    _ => null
                };

                if (type == null || !type.IsValueType || !isNullable)
                    return converter;

                var converterType = typeof(NullableValueTypeDdbConverter<>).MakeGenericType(type);
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
    }
}