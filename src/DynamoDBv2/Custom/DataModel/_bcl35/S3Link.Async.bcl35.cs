using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Amazon.DynamoDBv2.DataModel
{
    public partial class S3Link
    {
        #region Upload/PutObject

        /// <summary>
        /// Initiates the asynchronous execution of the UploadFrom operation.
        /// </summary>
        /// <param name="sourcePath">Path of the file to be uploaded.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property</param>
        public IAsyncResult BeginUploadFrom(string sourcePath, AsyncCallback callback, object state)
        {
            return this.s3ClientCache.GetClient(this.RegionAsEndpoint).BeginUploadObjectFromFilePath(
                this.linker.s3.bucket, this.linker.s3.key, sourcePath, null, callback, state);

        }

        /// <summary>
        /// Finishes the asynchronous execution of the UploadFrom operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginUploadFrom.</param>
        public void EndUploadFrom(IAsyncResult asyncResult)
        {
            this.s3ClientCache.GetClient(this.RegionAsEndpoint).EndUploadObjectFromFilePath(asyncResult);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the UploadStream operation.
        /// </summary>
        /// <param name="stream">The stream of data to upload.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property</param>
        public IAsyncResult BeginUploadStream(Stream stream, AsyncCallback callback, object state)
        {
            return this.s3ClientCache.GetClient(this.RegionAsEndpoint).BeginUploadObjectFromStream(
                this.linker.s3.bucket, this.linker.s3.key, stream, null, callback, state);

        }

        /// <summary>
        /// Finishes the asynchronous execution of the UploadStream operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginUploadStream.</param>
        public void EndUploadStream(IAsyncResult asyncResult)
        {
            this.s3ClientCache.GetClient(this.RegionAsEndpoint).EndUploadObjectFromStream(asyncResult);
        }

        #endregion

        #region Download/GetObject

        /// <summary>
        /// Initiates the asynchronous execution of the DownloadTo operation.
        /// </summary>
        /// <param name="downloadPath">Path to save the file.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property</param>
        public IAsyncResult BeginDownloadTo(string downloadPath, AsyncCallback callback, object state)
        {
            return this.s3ClientCache.GetClient(this.RegionAsEndpoint).BeginDownloadToFilePath(
                this.linker.s3.bucket, this.linker.s3.key, downloadPath, null, callback, state);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the DownloadTo operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginDownloadTo.</param>
        public void EndDownloadTo(IAsyncResult asyncResult)
        {
            this.s3ClientCache.GetClient(this.RegionAsEndpoint).EndDownloadToFilePath(asyncResult);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the OpenStream operation.
        /// </summary>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property</param>
        public IAsyncResult BeginOpenStream(AsyncCallback callback, object state)
        {
            return this.s3ClientCache.GetClient(this.RegionAsEndpoint).BeginGetObjectStream(
                this.linker.s3.bucket, this.linker.s3.key, null, callback, state);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the OpenStream operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginOpenStream.</param>
        public Stream EndOpenStream(IAsyncResult asyncResult)
        {
            return this.s3ClientCache.GetClient(this.RegionAsEndpoint).EndGetObjectStream(asyncResult);
        }

        #endregion
    }
}
