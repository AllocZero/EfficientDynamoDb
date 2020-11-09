using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Amazon.Runtime.Internal.Util
{
    /// <summary>
    /// A wrapper stream which supresses disposal of the underlying stream.
    /// </summary>
    public class NonDisposingWrapperStream : WrapperStream
    {
        /// <summary>
        /// Constructor for NonDisposingWrapperStream.
        /// </summary>
        /// <param name="baseStream">The base stream to wrap.</param>
        public NonDisposingWrapperStream(Stream baseStream) : base (baseStream)
        {
        }
#if !NETSTANDARD
        /// <summary>
        /// The Close implementation for this wrapper stream
        /// does not close the underlying stream.
        /// </summary>
        public override void Close()
        {
            // Suppress disposing the stream by not calling Close() on the base stream.            
        }
#endif
        /// <summary>
        /// The Dispose implementation for this wrapper stream
        /// does not close the underlying stream.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            // Suppress disposing the stream by not calling Dispose() on the base stream.            
        }
    }
}
