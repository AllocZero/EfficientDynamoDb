/*******************************************************************************
 *  Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *  Licensed under the Apache License, Version 2.0 (the "License"). You may not use
 *  this file except in compliance with the License. A copy of the License is located at
 *
 *  http://aws.amazon.com/apache2.0
 *
 *  or in the "license" file accompanying this file.
 *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
 *  CONDITIONS OF ANY KIND, either express or implied. See the License for the
 *  specific language governing permissions and limitations under the License.
 * *****************************************************************************
 *    __  _    _  ___
 *   (  )( \/\/ )/ __)
 *   /__\ \    / \__ \
 *  (_)(_) \/\/  (___/
 *
 *  AWS SDK for .NET
 *  API Version: 2006-03-01
 *
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
#if AWS_ASYNC_API
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Amazon.Runtime.Internal.Util
{
    /// <summary>
    /// This class is used to wrap a stream for a particular segment of a stream.  It 
    /// makes that segment look like you are reading from beginning to end of the stream.
    /// </summary>
    public class PartialWrapperStream : WrapperStream
    {
        private long initialPosition;
        private long partSize;

        public PartialWrapperStream(Stream stream, long partSize)
            : base(stream)
        {
            if (!stream.CanSeek)
                throw new InvalidOperationException("Base stream of PartialWrapperStream must be seekable");

            this.initialPosition = stream.Position;
            long remainingData = stream.Length - stream.Position;
            if (partSize == 0 || remainingData < partSize)
            {
                this.partSize = remainingData;
            }
            else
            {

                this.partSize = partSize;
                var encryptionStream = BaseStream as AESEncryptionUploadPartStream;
                if (encryptionStream != null && (partSize % 16) != 0)
                {
                    this.partSize = partSize - (partSize % EncryptUploadPartStream.InternalEncryptionBlockSize);
                }
            }
        }

        private long RemainingPartSize
        {
            get
            {
                long remaining = this.partSize - this.Position;
                return remaining;
            }
        }

        #region Stream overrides

        public override long Length
        {
            get
            {
                long length = base.Length - this.initialPosition;
                if (length > this.partSize)
                {
                    length = this.partSize;
                }
                return length;
            }
        }

        public override long Position
        {
            get
            {
                return base.Position - this.initialPosition;
            }
            set
            {
                base.Position = this.initialPosition + value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesToRead = count < this.RemainingPartSize ? count : (int)this.RemainingPartSize;
            if (bytesToRead <= 0)
                return 0;
            return base.Read(buffer, offset, bytesToRead);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            long position = 0;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    position = this.initialPosition + offset;
                    break;
                case SeekOrigin.Current:
                    position = base.Position + offset;
                    break;
                case SeekOrigin.End:
                    position = base.Position + this.partSize + offset;
                    break;
            }

            if (position < this.initialPosition)
            {
                position = this.initialPosition;
            }
            else if (position > this.initialPosition + this.partSize)
            {
                position = this.initialPosition + this.partSize;
            }

            base.Position = position;

            return this.Position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override void WriteByte(byte value)
        {
            throw new NotSupportedException();
        }

#if AWS_ASYNC_API
        /// <summary>
        /// Asynchronously reads a sequence of bytes from the current stream, advances
        /// the position within the stream by the number of bytes read, and monitors
        /// cancellation requests.
        /// </summary>
        /// <param name="buffer">
        /// An array of bytes. When this method returns, the buffer contains the specified
        /// byte array with the values between offset and (offset + count - 1) replaced
        /// by the bytes read from the current source.
        /// </param>
        /// <param name="offset">
        /// The zero-based byte offset in buffer at which to begin storing the data read
        /// from the current stream.
        /// </param>
        /// <param name="count">
        /// The maximum number of bytes to be read from the current stream.
        /// </param>
        /// <param name="cancellationToken">
        /// The token to monitor for cancellation requests. The default value is
        /// System.Threading.CancellationToken.None.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The value of the TResult
        /// parameter contains the total number of bytes read into the buffer. This can be
        /// less than the number of bytes requested if that many bytes are not currently
        /// available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            int bytesToRead = count < this.RemainingPartSize ? count : (int)this.RemainingPartSize;
            if (bytesToRead <= 0)
                return 0;
            return await base.ReadAsync(buffer, offset, bytesToRead, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously writes a sequence of bytes to the current stream and advances the
        /// current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">
        /// An array of bytes. This method copies count bytes from buffer to the current stream.
        /// </param>
        /// <param name="offset">
        /// The zero-based byte offset in buffer at which to begin copying bytes to the
        /// current stream.
        /// </param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <param name="cancellationToken">
        /// The token to monitor for cancellation requests. The default value is
        /// System.Threading.CancellationToken.None.
        /// </param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
#endif

#if !NETSTANDARD
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, Object state)
        {
            throw new NotSupportedException();
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, Object state)
        {
            throw new NotSupportedException();
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }
#endif

        #endregion
    }
}
