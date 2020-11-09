using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

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
        IList<string> GetAllObjectKeys(string bucketName, string prefix, IDictionary<string, object> additionalProperties);

        /// <summary>
        /// Delete the object.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKey"></param>
        /// <param name="additionalProperties"></param>
        void Delete(string bucketName, string objectKey, IDictionary<string, object> additionalProperties);

        /// <summary>
        /// Deletes the ojects.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKeys"></param>
        /// <param name="additionalProperties"></param>
        void Deletes(string bucketName, IEnumerable<string> objectKeys, IDictionary<string, object> additionalProperties);


        /// <summary>
        /// Upload an object from a stream.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKey"></param>
        /// <param name="stream"></param>
        /// <param name="additionalProperties"></param>
        void UploadObjectFromStream(string bucketName, string objectKey, Stream stream, IDictionary<string, object> additionalProperties);

        /// <summary>
        /// Upload an object from a file path.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKey"></param>
        /// <param name="filepath"></param>
        /// <param name="additionalProperties"></param>
        void UploadObjectFromFilePath(string bucketName, string objectKey, string filepath, IDictionary<string, object> additionalProperties);

        /// <summary>
        /// Download object to a file path.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKey"></param>
        /// <param name="filepath"></param>
        /// <param name="additionalProperties"></param>
        void DownloadToFilePath(string bucketName, string objectKey, string filepath, IDictionary<string, object> additionalProperties);

        /// <summary>
        /// Get stream for an object.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKey"></param>
        /// <param name="additionalProperties"></param>
        /// <returns></returns>
        Stream GetObjectStream(string bucketName, string objectKey, IDictionary<string, object> additionalProperties);

        /// <summary>
        /// Set the ACL on the object to public readable.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKey"></param>
        /// <param name="enable"></param>
        void MakeObjectPublic(string bucketName, string objectKey, bool enable);

        /// <summary>
        /// Check to see if the bucket exists and if it doesn't create the bucket.
        /// </summary>
        /// <param name="bucketName"></param>
        void EnsureBucketExists(string bucketName);

        /// <summary>
        /// Check to see if the bucket exists.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        bool DoesS3BucketExist(string bucketName);
    }
}
