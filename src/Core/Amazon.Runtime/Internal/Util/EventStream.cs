/*
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 * 
 *  http://aws.amazon.com/apache2.0
 * 
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
#if AWS_ASYNC_API
using System.Threading.Tasks;
#endif

namespace Amazon.Runtime.Internal.Util
{
    internal class EventStream : WrapperStream
    {
        internal delegate void ReadProgress(int bytesRead);
        internal event ReadProgress OnRead;
        bool disableClose;

        //Stream wrappedStream;

        internal EventStream(Stream stream, bool disableClose)
            : base(stream)
        {
            //this.wrappedStream = stream;
            this.disableClose = disableClose;
        }

        protected override void Dispose(bool disposing)
        {
        }

        public override bool CanRead
        {
            get { return BaseStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return BaseStream.CanSeek; }
        }

        public override bool CanTimeout
        {
            get { return BaseStream.CanTimeout; }
        }

        public override bool CanWrite
        {
            get { return BaseStream.CanWrite; }
        }

        public override long Length
        {
            get { return BaseStream.Length; }
        }

        public override long Position
        {
            get { return BaseStream.Position; }
            set { BaseStream.Position = value; }
        }

        public override int ReadTimeout
        {
            get { return BaseStream.ReadTimeout; }
            set { BaseStream.ReadTimeout = value; }
        }

        public override int WriteTimeout
        {
            get { return BaseStream.WriteTimeout; }
            set { BaseStream.WriteTimeout = value; }
        }

        public override void Flush()
        {
            BaseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead = BaseStream.Read(buffer, offset, count);
            if (this.OnRead != null)
            {
                this.OnRead(bytesRead);
            }

            return bytesRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return BaseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            BaseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override void WriteByte(byte value)
        {
            throw new NotImplementedException();
        }


#if AWS_ASYNC_API
        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            return BaseStream.FlushAsync(cancellationToken);
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            int bytesRead = await BaseStream.ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
            if (this.OnRead != null)
            {
                this.OnRead(bytesRead);
            }

            return bytesRead;
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
#endif

#if !NETSTANDARD
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, Object state)
        {
            var result = new AsyncResult()
            {
                AsyncState = state,
                CompletedSynchronously = true,
                IsCompleted = true,
                AsyncWaitHandle = new ManualResetEvent(true)
            };

            try
            {
                int bytesRead = this.Read(buffer, offset, count);
                result.Return = bytesRead;
            }
            catch (Exception e)
            {
                result.Return = e;
            }

            if (callback != null)
                callback(result);

            return result;
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, Object state)
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            if (!disableClose)
            {
                BaseStream.Close();
            }
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            var result = asyncResult as AsyncResult;
            if (result == null)
                throw new ArgumentException("Parameter asyncResult was not of type Amazon.Runtime.Internal.Util.AsyncResult", "asyncResult");
            if (result.Return is Exception)
                throw (Exception)result.Return;

            return Convert.ToInt32(result.Return, CultureInfo.InvariantCulture);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        class AsyncResult : IAsyncResult
        {
            public object AsyncState { get; internal set; }
            public WaitHandle AsyncWaitHandle { get; internal set; }
            public bool CompletedSynchronously { get; internal set; }
            public bool IsCompleted { get; internal set; }

            internal object Return { get; set; }
        }

#endif

    }
}
