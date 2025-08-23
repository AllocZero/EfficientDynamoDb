using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Exceptions;

namespace EfficientDynamoDb.Converters;

public abstract class TimeDdbConverter<T> : DdbConverter<T>, IDictionaryKeyConverter<T>, ISetValueConverter<T>
{
    protected const int MaxDateTimeStringLength = 64;
        
    // Accessing cached type name is faster than using typeof(T).Name every time.
    private static readonly string CachedTypeName = typeof(T).Name;
        
    public string Format { get; }

    public CultureInfo CultureInfo { get; set; } = CultureInfo.InvariantCulture;
        
    public DateTimeStyles DateTimeStyles { get; set; } = DateTimeStyles.RoundtripKind;

    internal int StackAllocSize { get; } = MaxDateTimeStringLength;

    public TimeDdbConverter(string format) : base(true)
    {
        Format = format;
    }

    internal TimeDdbConverter(string format, int maxDateTimeStringLength) : this(format)
    {
        StackAllocSize = maxDateTimeStringLength;
    }

    public override void Write(in DdbWriter writer, ref T value)
    {
        writer.JsonWriter.WriteStartObject();
            
        Span<char> buffer = stackalloc char[StackAllocSize];

        WriteToBuffer(value, buffer, out var length);
            
        writer.JsonWriter.WriteString(DdbTypeNames.String, buffer[..length]);
            
        writer.JsonWriter.WriteEndObject();
    }

    public virtual void WritePropertyName(in DdbWriter writer, ref T value)
    {
        Span<char> buffer = stackalloc char[StackAllocSize];
            
        WriteToBuffer(value, buffer, out var length);
            
        writer.JsonWriter.WritePropertyName(buffer[..length]);
    }

    public abstract string WriteStringValue(ref T value);

    public virtual void WriteStringValue(in DdbWriter writer, ref T value)
    {
        Span<char> buffer = stackalloc char[StackAllocSize];
            
        WriteToBuffer(value, buffer, out var length);

        writer.JsonWriter.WriteStringValue(buffer[..length]);
    }

    public sealed override T Read(ref DdbReader reader)
    {
        Span<char> buffer = stackalloc char[StackAllocSize];

        var length = Encoding.UTF8.GetChars(reader.JsonReaderValue.ValueSpan, buffer);
            
        if (!TryParseFromBuffer(buffer[..length], out var value))
            throw new DdbException($"Couldn't parse '{CachedTypeName}' value from JSON: {reader.JsonReaderValue.GetString()}");
                
        return value;
    }
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteToBuffer(T value, Span<char> buffer, out int charsWritten)
    {
        if (!TryWriteToBuffer(value, buffer, out charsWritten))
            throw new DdbException($"Couldn't format {CachedTypeName} ddb value from '{value}'.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract bool TryWriteToBuffer(T value, Span<char> buffer, out int charsWritten);
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract bool TryParseFromBuffer(Span<char> buffer, out T value);
}