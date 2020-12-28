using System;
using System.Buffers;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Internal.Crc;

namespace EfficientDynamoDb.Internal.Reader
{
    internal static partial class EntityDdbJsonReader
    {
        private const int DefaultBufferSize = 16 * 1024;
        
        public static async ValueTask<ReadResult<TEntity>> ReadAsync<TEntity>(Stream utf8Json, DynamoDbContextMetadata metadata, bool returnCrc, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var readerState = new JsonReaderState();

            var readStack = new DdbEntityReadStack(DdbEntityReadStack.DefaultStackLength, metadata);

            try
            {
                var buffer = ArrayPool<byte>.Shared.Rent(DefaultBufferSize);
                var clearMax = 0;
                
                try
                {
                    var bytesInBuffer = 0;
                    uint crc = 0;
                    var isFirstBlock = true;
                    
                    while (true)
                    {
                        var isFinalBlock = false;
                        
                        while (true)
                        {
                            var bytesRead = await utf8Json.ReadAsync(new Memory<byte>(buffer, bytesInBuffer, buffer.Length - bytesInBuffer), cancellationToken).ConfigureAwait(false);
                            if (bytesRead == 0)
                            {
                                isFinalBlock = true;
                                break;
                            }
                            
                            if(returnCrc)
                                crc = Crc32Algorithm.Append(crc, buffer, bytesInBuffer, bytesRead);

                            bytesInBuffer += bytesRead;

                            if (bytesInBuffer == buffer.Length)
                                break;
                        }
                        
                        if (bytesInBuffer > clearMax)
                            clearMax = bytesInBuffer;

                        readStack.UseFastPath = isFirstBlock && isFinalBlock;
                        readStack.Buffer = buffer;
                        readStack.BufferStart = 0;
                        readStack.BufferLength = bytesInBuffer;
                        ReadCore<TEntity>(ref readerState, isFinalBlock, new ReadOnlySpan<byte>(buffer, 0, bytesInBuffer), ref readStack);

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

                        isFirstBlock = false;
                    }

                    return new ReadResult<TEntity>((TEntity) readStack.GetCurrent().ReturnValue!, crc);
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
    }
}