using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Json
{
    internal partial class DocumentJsonReader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadValue(ref DdbReader reader, ref DdbEntityReadStackFrame frame)
        {
            return reader.JsonReaderValue.TokenType switch
            {
                JsonTokenType.String => TryReadString(ref reader, ref frame),
                JsonTokenType.Number => TryReadNumber(ref reader, ref frame),
                JsonTokenType.True => TryReadTrue(ref reader, ref frame),
                JsonTokenType.False => TryReadFalse(ref reader, ref frame),
                JsonTokenType.StartObject => TryReadMap(ref reader, ref frame),
                JsonTokenType.StartArray => TryReadList(ref reader, ref frame),
                JsonTokenType.Null => TryReadNull(ref reader, ref frame),
                _ => throw new ArgumentOutOfRangeException()
            };
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
            frame.AttributesBuffer.Add(new AttributeValue(new NumberAttributeValue(Encoding.UTF8.GetString(reader.JsonReaderValue.ValueSpan))));
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryReadTrue(ref DdbReader reader, ref DdbEntityReadStackFrame frame)
        {
            frame.AttributesBuffer.Add(new AttributeValue(new BoolAttributeValue(true)));
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryReadFalse(ref DdbReader reader, ref DdbEntityReadStackFrame frame)
        {
            frame.AttributesBuffer.Add(new AttributeValue(new BoolAttributeValue(false)));
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryReadNull(ref DdbReader reader, ref DdbEntityReadStackFrame frame)
        {
            frame.AttributesBuffer.Add(new AttributeValue(new NullAttributeValue(true)));
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