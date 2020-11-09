using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Amazon.Runtime.SharedInterfaces
{
    /// <summary>
    /// ICoreAmazonS3 is not meant to use directly. It defines S3 with basic .NET types
    /// and allows other services to be able to use S3 as a runtime dependency. This interface
    /// is implemented by the AmazonS3Client defined in the S3 assembly.
    /// </summary>
    public partial interface ICoreAmazonS3
    {
        /// <summary>
        /// Get all the object keys for the particular bucket and key prefix.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="prefix"></param>
        /// <param name="additionalProperties"></param>
        /// <returns></returns>
        Task<IList<string>> GetAllObjectKeysAsync(string bucketName, string prefix, IDictionary<string, object> additionalProperties);

        /// <summary>
        /// Upload an object from a stream.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKey"></param>
        /// <param name="stream"></param>
        /// <param name="additionalProperties"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UploadObjectFromStreamAsync(string bucketName, string objectKey, Stream stream, IDictionary<string, object> additionalProperties, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete an object.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKey"></param>
        /// <param name="additionalProperties"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAsync(string bucketName, string objectKey, IDictionary<string, object> additionalProperties, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete an object.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKeys"></param>
        /// <param name="additionalProperties"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeletesAsync(string bucketName, IEnumerable<string> objectKeys, IDictionary<string, object> additionalProperties, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Open a stream to an object in S3.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKey"></param>
        /// <param name="additionalProperties"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Stream> GetObjectStreamAsync(string bucketName, string objectKey, IDictionary<string, object> additionalProperties, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Upload an object from a file path.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKey"></param>
        /// <param name="filepath"></param>
        /// <param name="additionalProperties"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UploadObjectFromFilePathAsync(string bucketName, string objectKey, string filepath, IDictionary<string, object> additionalProperties, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Download an object in S3 to a file path.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKey"></param>
        /// <param name="filepath"></param>
        /// <param name="additionalProperties"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DownloadToFilePathAsync(string bucketName, string objectKey, string filepath, IDictionary<string, object> additionalProperties, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Set the ACL on the object to public readable.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKey"></param>
        /// <param name="enable"></param>
        Task MakeObjectPublicAsync(string bucketName, string objectKey, bool enable);

        /// <summary>
        /// Check to see if the bucket exists and if it doesn't create the bucket.
        /// </summary>
        /// <param name="bucketName"></param>
        Task EnsureBucketExistsAsync(string bucketName);

        /// <summary>
        /// Check to see if the bucket exists.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task<bool> DoesS3BucketExistAsync(string bucketName);
    }
}
