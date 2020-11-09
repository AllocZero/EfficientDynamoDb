using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Amazon.DynamoDBv2.DataModel
{
    public partial class S3Link
    {
        /// <summary>
        /// Uploads the specified file and stores it in the specified bucket with the provided key from construction.
        /// </summary>
        /// <param name="sourcePath">Path of the file to be uploaded.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An asynchronous task of the request</returns>
        public Task UploadFromAsync(string sourcePath, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.s3ClientCache.GetClient(this.RegionAsEndpoint).UploadObjectFromFilePathAsync(this.linker.s3.bucket, this.linker.s3.key, sourcePath, null, cancellationToken);
        }

        /// <summary>
        /// Uploads the stream and stores it in the specified bucket with the provided key from construction.
        /// </summary>
        /// <param name="stream">Stream to be uploaded to Amazon S3.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An asynchronous task of the request</returns>
        public Task UploadStreamAsync(Stream stream, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.s3ClientCache.GetClient(this.RegionAsEndpoint).UploadObjectFromStreamAsync(this.linker.s3.bucket, this.linker.s3.key, stream, null, cancellationToken);
        }

        /// <summary>
        /// Downloads the file from the S3Link's specified bucket and key then saves it in the given path. 
        /// Creates directories and the file if they do not already exist.
        /// </summary>
        /// <param name="downloadPath">Path to save the file.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An asynchronous task of the request</returns>
        public Task DownloadToAsync(string downloadPath, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.s3ClientCache.GetClient(this.RegionAsEndpoint).DownloadToFilePathAsync(this.linker.s3.bucket, this.linker.s3.key, downloadPath, null, cancellationToken);
        }

        /// <summary>
        /// Opens a stream to object stored in Amazon S3.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An asynchronous task of the request</returns>
        public Task<Stream> OpenStreamAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.s3ClientCache.GetClient(this.RegionAsEndpoint).GetObjectStreamAsync(this.linker.s3.bucket, this.linker.s3.key, null, cancellationToken);
        }
    }
}
