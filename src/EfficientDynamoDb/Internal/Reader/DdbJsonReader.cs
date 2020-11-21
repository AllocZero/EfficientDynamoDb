using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Reader
{
    public static class DdbJsonReader
    {
        private const int DefaultBufferSize = 16 * 1024;
        
        public static async ValueTask<Document> ReadAsync(Stream utf8Json, IParsingOptions options)
        {
            var readerState = new JsonReaderState();

            var readStack = new DdbReadStack(DdbReadStack.DefaultStackLength, options.Metadata);

            try
            {
                var buffer = ArrayPool<byte>.Shared.Rent(DefaultBufferSize);
                var clearMax = 0;
                
                try
                {
                    var bytesInBuffer = 0;

                    while (true)
                    {
                        var isFinalBlock = false;

                        while (true)
                        {
                            var bytesRead = await utf8Json.ReadAsync(new Memory<byte>(buffer, bytesInBuffer, buffer.Length - bytesInBuffer)).ConfigureAwait(false);
                            if (bytesRead == 0)
                            {
                                isFinalBlock = true;
                                break;
                            }

                            bytesInBuffer += bytesRead;

                            if (bytesInBuffer == buffer.Length)
                                break;
                        }
                        
                        if (bytesInBuffer > clearMax)
                            clearMax = bytesInBuffer;

                        ReadCore(ref readerState, isFinalBlock, new ReadOnlySpan<byte>(buffer, 0, bytesInBuffer), ref readStack, options);

                        var bytesConsumed = (int) readStack.BytesConsumed;
                        bytesInBuffer -= bytesConsumed;

                        if (isFinalBlock)
                            break;

                        // Check if we need to shift or expand the buffer because there wasn't enough data to complete deserialization.
                        if ((uint) bytesInBuffer > ((uint) buffer.Length / 2))
                        {
                            // We have less than half the buffer available, double the buffer size.
                            byte[] dest = ArrayPool<byte>.Shared.Rent((buffer.Length < (int.MaxValue / 2)) ? buffer.Length * 2 : int.MaxValue);

                            // Copy the unprocessed data to the new buffer while shifting the processed bytes.
                            Buffer.BlockCopy(buffer, bytesConsumed, dest, 0, bytesInBuffer);

                            new Span<byte>(buffer, 0, clearMax).Clear();
                            ArrayPool<byte>.Shared.Return(buffer);

                            clearMax = bytesInBuffer;
                            buffer = dest;
                        }
                        else if (bytesInBuffer != 0)
                        {
                            // Shift the processed bytes to the beginning of buffer to make more room.
                            Buffer.BlockCopy(buffer, bytesConsumed, buffer, 0, bytesInBuffer);
                        }
                    }

                    return readStack.GetCurrent().CreateDocumentFromBuffer()!;
                }
                finally
                {
                    new Span<byte>(buffer, 0, clearMax).Clear();
                    ArrayPool<byte>.Shared.Return(buffer);
                }
            }
            finally
            {
                readStack.Dispose();
            }
        }
        
        private static void ReadCore(ref JsonReaderState readerState, bool isFinalBlock, ReadOnlySpan<byte> buffer, ref DdbReadStack readStack, IParsingOptions options)
        {
            var reader = new Utf8JsonReader(buffer, isFinalBlock, readerState);
            readStack.BytesConsumed = 0;
            ReadCore(ref reader, ref readStack, options);

            readerState = reader.CurrentState;
        }

        private static void ReadCore(ref Utf8JsonReader reader, ref DdbReadStack state, IParsingOptions options)
        {
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.String:
                    {
                        HandleStringValue(ref reader, ref state);
                        break;
                    }
                    case JsonTokenType.PropertyName:
                    {
                        HandlePropertyName(ref reader, ref state);
                        break;
                    }
                    case JsonTokenType.StartObject:
                    {
                        if (!state.IsLastFrame || state.GetCurrent().IsProcessingValue())
                        {
                            // Parse inner object start
                            HandleNestedStartObject(ref state);
                        }
                        break;
                    }
                    case JsonTokenType.EndObject:
                    {
                        HandleEndObject(ref state);
                        break;
                    }
                    case JsonTokenType.True:
                    {
                        HandleBoolValue(ref state, true);
                        break;
                    }
                    case JsonTokenType.False:
                    {
                        HandleBoolValue(ref state, false);
                        break;
                    }
                    case JsonTokenType.StartArray:
                    {
                        HandleStartArray(ref state);
                        break;
                    }
                    case JsonTokenType.EndArray:
                    {
                        HandleEndArray(ref state);
                        break;
                    }
                    case JsonTokenType.Number:
                    {
                        HandleNumberValue(ref reader, ref state, options);
                        break;
                    }
                    case JsonTokenType.Null:
                    {
                        HandleNullValue(ref state);
                        break;
                    }
                }
            }
            
            state.BytesConsumed += reader.BytesConsumed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleStringValue(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
            ref var current = ref state.GetCurrent();
            
            if (current.AttributeType != AttributeType.Unknown)
            {
                ref var prevState = ref state.GetPrevious();

                prevState.StringBuffer.Add(prevState.KeyName!);
                prevState.AttributesBuffer.Add(current.AttributeType == AttributeType.String
                    ? new AttributeValue(new StringAttributeValue(reader.GetString()!))
                    : new AttributeValue(new NumberAttributeValue(reader.GetString()!)));
            }
            else
            {
                if (current.KeyName == null)
                {
                    current.StringBuffer.Add(reader.GetString()!);
                }
                else
                {
                    var value = reader.GetString();
                    current.StringBuffer.Add(current.KeyName);
                    current.AttributesBuffer.Add(value != null ? new AttributeValue(new StringAttributeValue()) : new AttributeValue(new NullAttributeValue(true)));
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleBoolValue(ref DdbReadStack state, bool value)
        {
            ref var current = ref state.GetCurrent();

            switch (current.AttributeType)
            {
                case AttributeType.Bool:
                {
                    ref var prevState = ref state.GetPrevious();
                
                    prevState.StringBuffer.Add(prevState.KeyName!);
                    prevState.AttributesBuffer.Add(new AttributeValue(new BoolAttributeValue(value)));
                    break;
                }
                case AttributeType.Null:
                {
                    ref var prevState = ref state.GetPrevious();
                
                    prevState.StringBuffer.Add(prevState.KeyName!);
                    prevState.AttributesBuffer.Add(new AttributeValue(new BoolAttributeValue(value)));
                    break;
                }
                default:
                {
                    current.StringBuffer.Add(current.KeyName!);
                    current.AttributesBuffer.Add(new AttributeValue(new BoolAttributeValue(value)));
                    break;
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleNullValue(ref DdbReadStack state)
        {
            ref var current = ref state.GetCurrent();
            
            current.StringBuffer.Add(current.KeyName!);
            current.AttributesBuffer.Add(new AttributeValue(new NullAttributeValue(true)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleNumberValue(ref Utf8JsonReader reader, ref DdbReadStack state, IParsingOptions callbacks)
        {
            if (state.IsLastFrame && callbacks.HasNumberCallback)
                callbacks.OnNumber(ref reader, ref state);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandlePropertyName(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
            ref var current = ref state.GetCurrent();
            if (state.ContainsDdbAttributeType())
            {
                current.AttributeType = GetDdbAttributeType(ref reader);
            }
            else
            {
                current.KeyName = reader.GetString();

                if (current.Metadata?.Fields == null)
                    return; 
                
                if(current.Metadata.Fields.TryGetValue(current.KeyName!, out var metadata))
                {
                    current.NextMetadata = metadata;
                    state.IsDdbSyntax = metadata.IsDdbSyntax;
                }
                else
                {
                    current.NextMetadata = null;
                    state.IsDdbSyntax = 0;
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleNestedStartObject(ref DdbReadStack state)
        {
            state.PushObject();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleEndObject(ref DdbReadStack state)
        {
            if (state.IsLastFrame)
                return;

            var document = state.GetCurrent().CreateDocumentFromBuffer();
                
            state.PopObject();

            if (document == null)
                return;
            
            ref var current = ref state.GetCurrent();

            if (current.AttributeType == AttributeType.Map)
            {
                ref var prevState = ref state.GetPrevious();
                prevState.StringBuffer.Add(prevState.KeyName!);
                prevState.AttributesBuffer.Add(new AttributeValue(new MapAttributeValue(document)));
            }
            else
            {
                current.StringBuffer.Add(current.KeyName!);
                current.AttributesBuffer.Add(new AttributeValue(new MapAttributeValue(document)));
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleStartArray(ref DdbReadStack state)
        {
            state.PushArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleEndArray(ref DdbReadStack state)
        {
            ref var initialCurrent = ref state.GetCurrent();
            
            state.PopArray();
            ref var current = ref state.GetCurrent();
            
            switch (current.AttributeType)
            {
                case AttributeType.List:
                {
                    ref var prevState = ref state.GetPrevious();
                    prevState.StringBuffer.Add(prevState.KeyName!);
                    prevState.AttributesBuffer.Add(new AttributeValue(new ListAttributeValue(DdbReadStackFrame.CreateListFromBuffer(ref initialCurrent.AttributesBuffer))));
                    break;
                }
                case AttributeType.StringSet:
                {
                    ref var prevState = ref state.GetPrevious();
                    prevState.StringBuffer.Add(prevState.KeyName!);
                    prevState.AttributesBuffer.Add(new AttributeValue(new StringSetAttributeValue(DdbReadStackFrame.CreateStringSetFromBuffer(ref initialCurrent.StringBuffer))));
                    break;
                }
                case AttributeType.NumberSet:
                {
                    ref var prevState = ref state.GetPrevious();
                    prevState.StringBuffer.Add(prevState.KeyName!);
                    prevState.AttributesBuffer.Add(new AttributeValue(new NumberSetAttributeValue(DdbReadStackFrame.CreateNumberArrayFromBuffer(ref initialCurrent.StringBuffer))));
                    break;
                }
                default:
                {
                    current.StringBuffer.Add(current.KeyName!);

                    current.AttributesBuffer.Add(current.NextMetadata?.ReturnDocuments == true
                        ? new AttributeValue(new DocumentListAttributeValue(DdbReadStackFrame.CreateDocumentListFromBuffer(ref initialCurrent.AttributesBuffer)))
                        : new AttributeValue(new ListAttributeValue(DdbReadStackFrame.CreateListFromBuffer(ref initialCurrent.AttributesBuffer))));

                    break;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static AttributeType GetDdbAttributeType(ref Utf8JsonReader reader)
        {
            var propertyName = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;

            var key = propertyName.Length > 1 ? MemoryMarshal.Read<short>(propertyName) : propertyName[0];

            return AttributeTypesMap.Get(key);
        }
    }
}