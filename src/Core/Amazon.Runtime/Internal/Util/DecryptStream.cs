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
using System.Security.Cryptography;
#if AWS_ASYNC_API
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Amazon.Runtime.Internal.Util
{
    /// <summary>
    /// A wrapper stream that decrypts the base stream as it
    /// is being read.
    /// </summary>
    public abstract class DecryptStream : WrapperStream
    {
        #region Properties

        protected CryptoStream CryptoStream { get; set; }
        protected IDecryptionWrapper Algorithm { get; set; }
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an DecryptStream with an decryption algorithm and a base stream.
        /// </summary>
        /// <param name="baseStream">Stream to perform encryption on..</param>
        protected DecryptStream(Stream baseStream)
            : base(baseStream)
        {
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
            int result = this.CryptoStream.Read(buffer, offset, count);
            return result;
        }

#if AWS_ASYNC_API
        /// <summary>
        /// Asynchronously reads a sequence of bytes from the current stream and advances
        /// the position within the stream by the number of bytes read.
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
        /// The total number of bytes read into the buffer. This can be less than the
        /// number of bytes requested if that many bytes are not currently available,
        /// or zero (0) if the end of the stream has been reached.
        /// </returns>
        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            int result = await this.CryptoStream.ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
            return result;
        }
#endif

#if BCL
        public override void Close()
        {
            base.Close();
        }
#endif
        /// <summary>
        /// Gets a value indicating whether the current stream supports seeking.
        /// DecryptStream does not support seeking, this will always be false.
        /// </summary>
        public override bool CanSeek
        {
            get
            {
                // Restrict random access
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the position within the current stream.
        /// DecryptStream does not support seeking, attempting to set Position
        /// will throw NotSupportedException.
        /// </summary>
        public override long Position
        {
            get
            {
                throw new NotSupportedException("DecryptStream does not support seeking");
            }
            set
            {
                throw new NotSupportedException("DecryptStream does not support seeking");
            }
        }

        /// <summary>
        /// Sets the position within the current stream.
        /// DecryptStream does not support seeking, attempting to call Seek
        /// will throw NotSupportedException.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        /// <param name="origin">
        /// A value of type System.IO.SeekOrigin indicating the reference point used
        /// to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException("DecryptStream does not support seeking");
        }
        #endregion

        #region Private methods

        /// <summary>
        /// Validates the underlying stream.
        /// </summary>
        private void ValidateBaseStream()
        {
            if (!BaseStream.CanRead && !BaseStream.CanWrite)
                throw new InvalidDataException("DecryptStream does not support base streams that are not capable of reading or writing");
        }

        #endregion
    }


    /// <summary>
    /// A wrapper stream that decrypts the base stream as it
    /// is being read.
    /// </summary>
    public class DecryptStream<T> : DecryptStream
            where T : class, IDecryptionWrapper, new()
    {
        #region Constructors

        /// <summary>
        /// Initializes an DecryptStream with an decryption algorithm and a base stream.
        /// </summary>
        /// <param name="baseStream">Stream to perform encryption on..</param>
        /// <param name="envelopeKey">Symmetric key to perform decryption</param>
        /// <param name="IV">Initialization vector to perform decryption</param>
        public DecryptStream(Stream baseStream, byte[] envelopeKey, byte[] IV)
            : base(baseStream)
        {
            Algorithm = new T();
            Algorithm.SetDecryptionData(envelopeKey, IV);
            Algorithm.CreateDecryptor();
            CryptoStream = new CryptoStream(this.BaseStream, Algorithm.Transformer, CryptoStreamMode.Read);
        }


        #endregion
    }


    /// <summary>
    /// A wrapper stream that decrypts the base stream using AES algorithm as it
    /// is being read.
    /// </summary>
    public class AESDecryptionStream : DecryptStream<DecryptionWrapperAES>
    {
        #region Constructors

        /// <summary>
        /// Initializes an AESDecryptionStream with a base stream.
        /// </summary>
        /// <param name="baseStream">Stream to perform decryption on..</param>
        /// <param name="key">Symmetric key to perform decryption</param>
        /// <param name="IV">Initialization vector to perform decryption</param>
        public AESDecryptionStream(Stream baseStream, byte[] key, byte[] IV)
            : base(baseStream, key, IV) { }

        #endregion
    }
}