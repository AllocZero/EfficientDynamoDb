using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.Internal.Converters
{
    internal sealed class PaginationDdbConverter : DdbConverter<string?>
    {
        internal override DdbClassType ClassType => DdbClassType.None;

        public override string Read(in AttributeValue attributeValue) => throw new NotSupportedException("Should never be called.");

        public override AttributeValue Write(ref string? value) => throw new NotSupportedException("Should never be called.");

        public override void Write(in DdbWriter writer, string attributeName, ref string? value)
        {
            writer.JsonWriter.WritePropertyName(attributeName);
            
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
            
            var start = checked((int) reader.JsonReaderValue.TokenStartIndex);
            
            if (reader.State.UseFastPath)
            {
                reader.JsonReaderValue.Skip();
            }
            else
            {
                var initialReaderState = reader.JsonReaderValue.CurrentState;
                var initialReaderBytesConsumed = reader.JsonReaderValue.BytesConsumed;
                
                // Attempt to skip to make sure we have all the data we need.
                if (!reader.JsonReaderValue.TrySkip())
                {
                    var originalSpan = new ReadOnlySpan<byte>(reader.State.Buffer, reader.State.BufferStart, reader.State.BufferLength);
                    var offset = checked((int) initialReaderBytesConsumed);
                    reader = new DdbReader(originalSpan.Slice(offset),
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
            var bytes = reader.State.Buffer!.AsSpan(reader.State.BufferStart, reader.State.BufferLength).Slice(start, end - start);
            value = Encoding.UTF8.GetString(bytes);
            if (value == "{}")
                value = null;

            return true;
        }
    }
}