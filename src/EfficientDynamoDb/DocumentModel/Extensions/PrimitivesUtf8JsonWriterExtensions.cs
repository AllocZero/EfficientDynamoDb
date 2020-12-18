using System;
using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.Exceptions;

namespace EfficientDynamoDb.DocumentModel.Extensions
{
    public static class PrimitivesUtf8JsonWriterExtensions
    {
        private static readonly StandardFormat Iso8601Format = StandardFormat.Parse("O");
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteString(this Utf8JsonWriter writer, string propertyName, int value)
        {
            Span<byte> buffer = stackalloc byte[11];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WriteString(propertyName, buffer.Slice(0, bytesWritten));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteStringValue(this Utf8JsonWriter writer, int value)
        {
            Span<byte> buffer = stackalloc byte[11];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WriteStringValue(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePropertyName(this Utf8JsonWriter writer, int propertyName)
        {
            Span<byte> buffer = stackalloc byte[11];
            var success = Utf8Formatter.TryFormat(propertyName, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WritePropertyName(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteString(this Utf8JsonWriter writer, string propertyName, uint value)
        {
            Span<byte> buffer = stackalloc byte[10];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WriteString(propertyName, buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteStringValue(this Utf8JsonWriter writer, uint value)
        {
            Span<byte> buffer = stackalloc byte[10];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WriteStringValue(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePropertyName(this Utf8JsonWriter writer, uint propertyName)
        {
            Span<byte> buffer = stackalloc byte[10];
            var success = Utf8Formatter.TryFormat(propertyName, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WritePropertyName(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteString(this Utf8JsonWriter writer, string propertyName, short value)
        {
            Span<byte> buffer = stackalloc byte[6];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WriteString(propertyName, buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteStringValue(this Utf8JsonWriter writer, short value)
        {
            Span<byte> buffer = stackalloc byte[6];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WriteStringValue(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePropertyName(this Utf8JsonWriter writer, short propertyName)
        {
            Span<byte> buffer = stackalloc byte[6];
            var success = Utf8Formatter.TryFormat(propertyName, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WritePropertyName(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteString(this Utf8JsonWriter writer, string propertyName, ushort value)
        {
            Span<byte> buffer = stackalloc byte[5];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WriteString(propertyName, buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteStringValue(this Utf8JsonWriter writer, ushort value)
        {
            Span<byte> buffer = stackalloc byte[5];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WriteStringValue(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePropertyName(this Utf8JsonWriter writer, ushort propertyName)
        {
            Span<byte> buffer = stackalloc byte[5];
            var success = Utf8Formatter.TryFormat(propertyName, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WritePropertyName(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteString(this Utf8JsonWriter writer, string propertyName, byte value)
        {
            Span<byte> buffer = stackalloc byte[3];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WriteString(propertyName, buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteStringValue(this Utf8JsonWriter writer, byte value)
        {
            Span<byte> buffer = stackalloc byte[3];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WriteStringValue(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePropertyName(this Utf8JsonWriter writer, byte propertyName)
        {
            Span<byte> buffer = stackalloc byte[3];
            var success = Utf8Formatter.TryFormat(propertyName, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WritePropertyName(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteString(this Utf8JsonWriter writer, string propertyName, long value)
        {
            Span<byte> buffer = stackalloc byte[20];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WriteString(propertyName, buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteStringValue(this Utf8JsonWriter writer, long value)
        {
            Span<byte> buffer = stackalloc byte[20];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WriteStringValue(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePropertyName(this Utf8JsonWriter writer, long propertyName)
        {
            Span<byte> buffer = stackalloc byte[20];
            var success = Utf8Formatter.TryFormat(propertyName, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WritePropertyName(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteString(this Utf8JsonWriter writer, string propertyName, ulong value)
        {
            Span<byte> buffer = stackalloc byte[20];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WriteString(propertyName, buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteStringValue(this Utf8JsonWriter writer, ulong value)
        {
            Span<byte> buffer = stackalloc byte[20];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WriteStringValue(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePropertyName(this Utf8JsonWriter writer, ulong propertyName)
        {
            Span<byte> buffer = stackalloc byte[20];
            var success = Utf8Formatter.TryFormat(propertyName, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WritePropertyName(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteString(this Utf8JsonWriter writer, string propertyName, double value)
        {
            Span<byte> buffer = stackalloc byte[128];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WriteString(propertyName, buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteStringValue(this Utf8JsonWriter writer, double value)
        {
            Span<byte> buffer = stackalloc byte[128];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WriteStringValue(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePropertyName(this Utf8JsonWriter writer, double propertyName)
        {
            Span<byte> buffer = stackalloc byte[128];
            var success = Utf8Formatter.TryFormat(propertyName, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WritePropertyName(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteString(this Utf8JsonWriter writer, string propertyName, float value)
        {
            Span<byte> buffer = stackalloc byte[128];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WriteString(propertyName, buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteStringValue(this Utf8JsonWriter writer, float value)
        {
            Span<byte> buffer = stackalloc byte[128];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WriteStringValue(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePropertyName(this Utf8JsonWriter writer, float propertyName)
        {
            Span<byte> buffer = stackalloc byte[128];
            var success = Utf8Formatter.TryFormat(propertyName, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WritePropertyName(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteString(this Utf8JsonWriter writer, string propertyName, decimal value)
        {
            Span<byte> buffer = stackalloc byte[31];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WriteString(propertyName, buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteStringValue(this Utf8JsonWriter writer, decimal value)
        {
            Span<byte> buffer = stackalloc byte[31];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WriteStringValue(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePropertyName(this Utf8JsonWriter writer, decimal propertyName)
        {
            Span<byte> buffer = stackalloc byte[31];
            var success = Utf8Formatter.TryFormat(propertyName, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WritePropertyName(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteString(this Utf8JsonWriter writer, string propertyName, DateTime value, in StandardFormat format, int maxLength)
        {
            Span<byte> buffer = stackalloc byte[maxLength];
            if(!Utf8Formatter.TryFormat(value, buffer, out var bytesWritten, format))
                ThrowFormatException(value);
            
            writer.WriteString(propertyName, buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteStringValue(this Utf8JsonWriter writer, DateTime value, in StandardFormat format, int maxLength)
        {
            Span<byte> buffer = stackalloc byte[maxLength];
            if(!Utf8Formatter.TryFormat(value, buffer, out var bytesWritten, format))
                ThrowFormatException(value);
            
            writer.WriteStringValue(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteIso8601DateTime(this Utf8JsonWriter writer, string propertyName, DateTime value)
        {
            Span<byte> buffer = stackalloc byte[28];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten, Iso8601Format);
            Debug.Assert(success);
            
            writer.WriteString(propertyName, buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteIso8601DateTimeValue(this Utf8JsonWriter writer, DateTime value)
        {
            Span<byte> buffer = stackalloc byte[28];
            var success = Utf8Formatter.TryFormat(value, buffer, out var bytesWritten, Iso8601Format);
            Debug.Assert(success);
            
            writer.WriteStringValue(buffer.Slice(0, bytesWritten));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePropertyName(this Utf8JsonWriter writer, Guid propertyName)
        {
            Span<byte> buffer = stackalloc byte[36];
            var success = Utf8Formatter.TryFormat(propertyName, buffer, out var bytesWritten);
            Debug.Assert(success);
            
            writer.WritePropertyName(buffer.Slice(0, bytesWritten));
        }

        private static void ThrowFormatException<T>(T value) => throw new DdbException($"Couldn't write {typeof(T).Name} '{value}' value as string.");
    }
}