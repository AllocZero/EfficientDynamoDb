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
using System.IO;
#if AWS_ASYNC_API
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Amazon.Runtime.Internal.Util
{
    /// <summary>
    /// Wrapper stream that only supports reading
    /// </summary>
    public class ReadOnlyWrapperStream : WrapperStream
    {
        public ReadOnlyWrapperStream(Stream baseStream)
            : base(baseStream)
        {
            if (!baseStream.CanRead)
                throw new InvalidOperationException("Base stream must be readable");
        }

        #region Overrides

#if !NETSTANDARD
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }
        public override void EndWrite(IAsyncResult asyncResult)
        {
            throw new NotSupportedException();
        }
#endif

        public override bool CanRead
        {
            get { return true; }
        }
        public override bool CanSeek
        {
            get { return false; }
        }
        public override bool CanWrite
        {
            get { return false; }
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }
        public override long Length
        {
            get
            {
                throw new NotSupportedException();
            }
        }
        public override long Position
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                
                throw new NotSupportedException();
            }
        }
        public override void Flush()
        {
            throw new NotSupportedException();
        }

#if AWS_ASYNC_API
        /// <summary>
        /// Asynchronously clears all buffers for this stream and causes any buffered data
        /// to be written to the underlying device.
        /// </summary>
        /// <param name="cancellationToken">
        /// The token to monitor for cancellation requests. The default value is
        /// System.Threading.CancellationToken.None.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous flush operation.
        /// </returns>
        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
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

        #endregion
    }

    /// <summary>
    /// Partial wrapper stream that only supports reading
    /// </summary>
    public class PartialReadOnlyWrapperStream : ReadOnlyWrapperStream
    {
        private long _currentPosition;
        private long _size;

        public PartialReadOnlyWrapperStream(Stream baseStream, long size)
            : base(baseStream)
        {
            _currentPosition = 0;
            _size = size;
        }

        private long RemainingSize
        {
            get
            {
                long remaining = this._size - this._currentPosition;
                return remaining;
            }
        }

        #region Overrides

        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesToRead = count < this.RemainingSize ? count : (int)this.RemainingSize;
            if (bytesToRead <= 0)
                return 0;

            int result = base.Read(buffer, offset, bytesToRead);
            _currentPosition += result;
            return result;
        }

#if AWS_ASYNC_API
        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            int bytesToRead = count < this.RemainingSize ? count : (int)this.RemainingSize;
            if (bytesToRead <= 0)
                return 0;

            int result = await base.ReadAsync(buffer, offset, bytesToRead, cancellationToken).ConfigureAwait(false);
            _currentPosition += result;
            return result;
        }
#endif

        public override long Length
        {
            get
            {
                return _size;
            }
        }

        public override long Position
        {
            get
            {
                return _currentPosition;
            }
        }

#endregion
    }
}
