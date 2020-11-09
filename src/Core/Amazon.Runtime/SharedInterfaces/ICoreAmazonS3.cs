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
        /// Generate a presigned URL.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKey"></param>
        /// <param name="expiration"></param>
        /// <param name="additionalProperties"></param>
        /// <returns></returns>
        string GeneratePreSignedURL(string bucketName, string objectKey, DateTime expiration, IDictionary<string, object> additionalProperties);
        
#if AWS_APM_API

        /// <summary>
        /// Start a delete object.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKey"></param>
        /// <param name="additionalProperties"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        IAsyncResult BeginDelete(string bucketName, string objectKey, IDictionary<string, object> additionalProperties, AsyncCallback callback, object state);

        /// <summary>
        /// Get the results of a delete object.
        /// </summary>
        /// <param name="result"></param>
        void EndDelete(IAsyncResult result);

        /// <summary>
        /// Start an upload object from stream.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKey"></param>
        /// <param name="stream"></param>
        /// <param name="additionalProperties"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        IAsyncResult BeginUploadObjectFromStream(string bucketName, string objectKey, Stream stream, IDictionary<string, object> additionalProperties, AsyncCallback callback, object state);

        /// <summary>
        /// Get the results of an upload from stream.
        /// </summary>
        /// <param name="result"></param>
        void EndUploadObjectFromStream(IAsyncResult result);

        /// <summary>
        /// Start an upload object from file path.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKey"></param>
        /// <param name="filepath"></param>
        /// <param name="additionalProperties"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        IAsyncResult BeginUploadObjectFromFilePath(string bucketName, string objectKey, string filepath, IDictionary<string, object> additionalProperties, AsyncCallback callback, object state);

        /// <summary>
        /// Get the results of an upload from file path.
        /// </summary>
        /// <param name="result"></param>
        void EndUploadObjectFromFilePath(IAsyncResult result);

        /// <summary>
        /// Start a download to a file path.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKey"></param>
        /// <param name="filepath"></param>
        /// <param name="additionalProperties"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        IAsyncResult BeginDownloadToFilePath(string bucketName, string objectKey, string filepath, IDictionary<string, object> additionalProperties, AsyncCallback callback, object state);

        /// <summary>
        /// Get results of downloading an object to a file path.
        /// </summary>
        /// <param name="result"></param>
        void EndDownloadToFilePath(IAsyncResult result);

        /// <summary>
        /// Start opening a stream to an object in S3.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKey"></param>
        /// <param name="additionalProperties"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        IAsyncResult BeginGetObjectStream(string bucketName, string objectKey, IDictionary<string, object> additionalProperties, AsyncCallback callback, object state);

        /// <summary>
        /// Get results of opening a stream to an object in S3.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        Stream EndGetObjectStream(IAsyncResult result);
#endif
    }
}
