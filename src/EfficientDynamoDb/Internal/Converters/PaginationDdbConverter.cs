using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.Internal.Converters
{
    internal sealed class PaginationDdbConverter : DdbConverter<string?>
    {
        internal override DdbClassType ClassType => DdbClassType.None;

        public override string Read(in AttributeValue attributeValue) => throw new NotSupportedException("Should never be called.");

        public override AttributeValue Write(ref string? value) => throw new NotSupportedException("Should never be called.");

        public override void Write(in DdbWriter writer, ref string? value)
        {
            // Flush to make sure our changes don't overlap with pending changes
            writer.JsonWriter.Flush();

            var bytesSize = Encoding.UTF8.GetByteCount(value!);

            var bytesWritten = Encoding.UTF8.GetBytes(value, writer.BufferWriter.GetSpan(bytesSize));
            writer.BufferWriter.Advance(bytesWritten);
        }

        internal override bool TryRead(ref DdbReader reader, out string? value)
        {
            if (reader.AttributeType == AttributeType.Null)
            {
                value = null;
                return true;
            }

            if (reader.JsonReaderValue.TokenType != JsonTokenType.StartObject)
                throw new DdbException("Unsupported pagination token.");
            
            var initialReaderBytesConsumed = reader.JsonReaderValue.BytesConsumed;
            var offset = checked((int) initialReaderBytesConsumed);
            if (reader.State.UseFastPath)
            {
                reader.JsonReaderValue.Skip();
            }
            else
            {
                var initialReaderState = reader.JsonReaderValue.CurrentState;

                // Attempt to skip to make sure we have all the data we need.
                if (!reader.JsonReaderValue.TrySkip())
                {
                    var originalSpan = new ReadOnlySpan<byte>(reader.State.Buffer, reader.State.BufferStart, reader.State.BufferLength);
                    
                    reader = new DdbReader(originalSpan[offset..],
                        reader.JsonReaderValue.IsFinalBlock,
                        ref initialReaderState, ref reader.State);

                    reader.State.BufferStart += offset;
                    reader.State.BufferLength -= offset;

                    Debug.Assert(reader.JsonReaderValue.BytesConsumed == 0);
                    reader.State.BytesConsumed += initialReaderBytesConsumed;

                    Unsafe.SkipInit(out value);
                    return false;
                }
            }
            
            var end = checked((int) reader.JsonReaderValue.BytesConsumed);
            var length = end - offset;
            if (length == 1)
            {
                // Only possible when pagination token returned from DDB equals "{}" that means end of pagination
                value = null;
            }
            else
            {
                var span = reader.State.Buffer!.AsSpan(reader.State.BufferStart, reader.State.BufferLength).Slice(offset, length);
                var fullTokenLength = Encoding.UTF8.GetCharCount(span) + 1;

                // Concatenate '{' with the rest of pagination token
                // Can't use reader.JsonReaderValue.TokenStartIndex because '{' is missing after buffer re-read
                value = string.Create(fullTokenLength, (reader.State, Start: offset, Length: length), (arr, state) =>
                {
                    arr[0] = '{';
                    Encoding.UTF8.GetChars(state.State.Buffer!.AsSpan(state.State.BufferStart, state.State.BufferLength).Slice(state.Start, state.Length), arr[1..]);
                });
            }

            return true;
        }
    }
}