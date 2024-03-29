using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Exceptions;

namespace EfficientDynamoDb.Internal.Reader.DocumentDdbReader
{
    internal static partial class DocumentDdbReader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadValue(ref DdbReader reader, ref DdbEntityReadStackFrame frame)
        {
            return frame.AttributeType switch
            {
                AttributeType.String => TryReadString(ref reader, ref frame),
                AttributeType.Number => TryReadNumber(ref reader, ref frame),
                AttributeType.Bool => TryReadBool(ref reader, ref frame),
                AttributeType.Map => TryReadMap(ref reader, ref frame),
                AttributeType.List => TryReadList(ref reader, ref frame),
                AttributeType.StringSet => TryReadStringSet(ref reader, ref frame),
                AttributeType.NumberSet => TryReadNumberSet(ref reader, ref frame),
                AttributeType.Null => TryReadNull(ref reader, ref frame),
                AttributeType.Binary => TryReadBinary(ref reader, ref frame),
                AttributeType.BinarySet => TryReadBinarySet(ref reader, ref frame),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadValue(ref DdbReader reader, out AttributeValue value)
        {
            switch (reader.AttributeType)
            {
                case AttributeType.String:
                {
                    value = new AttributeValue(new StringAttributeValue(reader.JsonReaderValue.GetString()!));
                    return true;
                }
                case AttributeType.Number:
                {
                    value = new AttributeValue(new NumberAttributeValue(reader.JsonReaderValue.GetString()!));
                    return true;
                }
                case AttributeType.Bool:
                {
                    value = new AttributeValue(new BoolAttributeValue(reader.JsonReaderValue.GetBoolean()));
                    return true;
                }
                case AttributeType.Map:
                {
                    if (!TryReadMap(ref reader, out var document))
                    {
                        Unsafe.SkipInit(out value);
                        return false;
                    }

                    value = new AttributeValue(new MapAttributeValue(document));
                    return true;
                }
                case AttributeType.List:
                {
                    if (!TryReadList(ref reader, out var list))
                    {
                        Unsafe.SkipInit(out value);
                        return false;
                    }

                    value = new AttributeValue(new ListAttributeValue(list));
                    return true;
                }
                case AttributeType.StringSet:
                {
                    if (!TryReadStringSet(ref reader, out var set))
                    {
                        Unsafe.SkipInit(out value);
                        return false;
                    }

                    value = new AttributeValue(new StringSetAttributeValue(set));
                    return true;
                }
                case AttributeType.NumberSet:
                {
                    if (!TryReadNumberSet(ref reader, out var set))
                    {
                        Unsafe.SkipInit(out value);
                        return false;
                    }

                    value = new AttributeValue(new NumberSetAttributeValue(set));
                    return true;
                }
                case AttributeType.Null:
                {
                    value = new AttributeValue(new NullAttributeValue(true));
                    return true;
                }
            }

            throw new DdbException("Unknown attribute type.");
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryReadString(ref DdbReader reader, ref DdbEntityReadStackFrame frame)
        {
            frame.AttributesBuffer.Add(new AttributeValue(new StringAttributeValue(reader.JsonReaderValue.GetString()!)));
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryReadNumber(ref DdbReader reader, ref DdbEntityReadStackFrame frame)
        {
            frame.AttributesBuffer.Add(new AttributeValue(new NumberAttributeValue(reader.JsonReaderValue.GetString()!)));
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryReadBool(ref DdbReader reader, ref DdbEntityReadStackFrame frame)
        {
            frame.AttributesBuffer.Add(new AttributeValue(new BoolAttributeValue(reader.JsonReaderValue.GetBoolean())));
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryReadNull(ref DdbReader reader, ref DdbEntityReadStackFrame frame)
        {
            frame.AttributesBuffer.Add(new AttributeValue(new NullAttributeValue(true)));
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryReadBinary(ref DdbReader reader, ref DdbEntityReadStackFrame frame)
        {
            frame.AttributesBuffer.Add(new AttributeValue(new BinaryAttributeValue(reader.JsonReaderValue.GetBytesFromBase64())));
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetCachedString(ref Utf8JsonReader reader, ref DdbEntityReadStack state)
        {
            return !state.KeysCache.IsInitialized || !state.KeysCache.TryGetOrAdd(ref reader, out var value)
                ? reader.GetString()!
                : value!;
        }
    }
}