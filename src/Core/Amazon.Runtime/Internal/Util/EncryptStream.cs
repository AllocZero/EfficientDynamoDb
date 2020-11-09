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
using Amazon.Runtime;
using System.Collections;
using System.Diagnostics;
#if AWS_ASYNC_API
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Amazon.Runtime.Internal.Util
{
    /// <summary>
    /// A wrapper stream that encrypts the base stream as it
    /// is being read.
    /// </summary>
    public abstract class EncryptStream : WrapperStream
    {
        #region Properties

        protected IEncryptionWrapper Algorithm { get; set; }
        private const int internalEncryptionBlockSize = 16;
        private byte[] internalBuffer;
        private bool performedLastBlockTransform;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an EncryptStream with an encryption algorithm and a base stream.
        /// </summary>
        /// <param name="baseStream">Stream to perform encryption on..</param>
        protected EncryptStream(Stream baseStream)
            : base(baseStream)
        {
            performedLastBlockTransform = false;
            internalBuffer = new byte[internalEncryptionBlockSize];
            ValidateBaseStream();
        }
        #endregion

        #region Stream overrides


        /// <summary>
        /// Reads a sequence of bytes from the current stream and advances the position
        /// within the stream by the number of bytes read.
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
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the
        /// number of bytes requested if that many bytes are not currently available,
        /// or zero (0) if the end of the stream has been reached.
        /// </returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (performedLastBlockTransform)
                return 0;

            long previousPosition = this.Position;
            int maxBytesRead = count - (count % internalEncryptionBlockSize);
            int readBytes = base.Read(buffer, offset, maxBytesRead);

            return Append(buffer, offset, previousPosition, readBytes);
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
            if (performedLastBlockTransform)
                return 0;

            long previousPosition = this.Position;
            int maxBytesRead = count - (count % internalEncryptionBlockSize);
            int readBytes = await base.ReadAsync(buffer, offset, maxBytesRead, cancellationToken).ConfigureAwait(false);

            return Append(buffer, offset, previousPosition, readBytes);
        }
#endif

        private int Append(byte[] buffer, int offset, long previousPosition, int readBytes)
        {
            if (readBytes == 0)
            {
                byte[] finalBytes = Algorithm.AppendLastBlock(buffer, offset, 0);
                finalBytes.CopyTo(buffer, offset);
                performedLastBlockTransform = true;
                return finalBytes.Length;
            }

            long currentPosition = previousPosition;

            while (this.Position - currentPosition >= internalEncryptionBlockSize)
            {
                currentPosition += Algorithm.AppendBlock(buffer, offset, internalEncryptionBlockSize, internalBuffer, 0);
                Buffer.BlockCopy(internalBuffer, 0, buffer, offset, internalEncryptionBlockSize);
                offset = offset + internalEncryptionBlockSize;
            }

            if ((this.Length - this.Position) < internalEncryptionBlockSize)
            {
                byte[] finalBytes = Algorithm.AppendLastBlock(buffer, offset, (int)(this.Position - currentPosition));
                finalBytes.CopyTo(buffer, offset);
                currentPosition += finalBytes.Length;
                performedLastBlockTransform = true;
            }

            return (int)(currentPosition - previousPosition);
        }

#if BCL
        public override void Close()
        {
            base.Close();
        }
#endif

        /// <summary>
        /// Gets a value indicating whether the current stream supports seeking.
        /// </summary>
        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Returns encrypted content length.
        /// </summary>
        public override long Length
        {
            get
            {
                if (base.Length % internalEncryptionBlockSize == 0)
                {
                    return (base.Length + internalEncryptionBlockSize);
                }
                else
                {
                    return (base.Length + internalEncryptionBlockSize - (base.Length % internalEncryptionBlockSize));
                }
            }
        }

        /// <summary>
        /// Gets or sets the position within the current stream.
        /// </summary>
        public override long Position
        {
            get
            {
                return BaseStream.Position;
            }
            set
            {
                Seek(offset: value, origin: SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// Sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        /// <param name="origin">
        /// A value of type System.IO.SeekOrigin indicating the reference point used
        /// to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            long position = BaseStream.Seek(offset, origin);

            this.performedLastBlockTransform = false;
            this.Algorithm.Reset();

            return position;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Validates the underlying stream.
        /// </summary>
        private void ValidateBaseStream()
        {
            if (!BaseStream.CanRead && !BaseStream.CanWrite)
                throw new InvalidDataException("EncryptStream does not support base streams that are not capable of reading or writing");
        }

        #endregion
    }

    /// <summary>
    /// A wrapper stream that encrypts the base stream as it
    /// is being read.
    /// </summary>   
    public class EncryptStream<T> : EncryptStream
            where T : class, IEncryptionWrapper, new()
    {
        #region Constructors

        /// <summary>
        /// Initializes an EncryptStream with an encryption algorithm and a base stream.
        /// </summary>
        /// <param name="baseStream">Stream to perform encryption on..</param>
        /// <param name="key">Symmetric key to perform encryption</param>
        /// <param name="IV">Initialization vector to perform encryption</param>
        public EncryptStream(Stream baseStream, byte[] key, byte[] IV)
            : base(baseStream)
        {
            Algorithm = new T();
            Algorithm.SetEncryptionData(key, IV);
            Algorithm.CreateEncryptor();
        }
        #endregion
    }


    /// <summary>
    /// A wrapper stream that encrypts the base stream using AES algorithm as it
    /// is being read.
    /// </summary>
    public class AESEncryptionPutObjectStream : EncryptStream<EncryptionWrapperAES>
    {
        #region Constructors

        /// <summary>
        /// Initializes an AESEncryptionStream with a base stream.
        /// </summary>
        /// <param name="baseStream">Stream to perform encryption on..</param>
        /// <param name="key">Symmetric key to perform encryption</param>
        /// <param name="IV">Initialization vector to perform encryption</param>
        public AESEncryptionPutObjectStream(Stream baseStream, byte[] key, byte[] IV)
            : base(baseStream, key, IV) { }

        #endregion
    }
}