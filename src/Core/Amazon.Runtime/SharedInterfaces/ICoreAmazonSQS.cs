using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amazon.Runtime.SharedInterfaces
{
    /// <summary>
    /// ICoreAmazonSQS is not meant to use directly. It defines SQS with basic .NET types
    /// and allows other services to be able to use SQS as a runtime dependency. This interface
    /// is implemented by the AmazonSQSClient defined in the SQS assembly.
    /// </summary>
    public interface ICoreAmazonSQS
    {
#if BCL
        /// <summary>
        /// <para>
        /// This method is used internally to access the Amazon SQS service within other service assemblies.
        /// Please use AmazonSQSClient to access Amazon SQS instead.
        /// </para>
        /// Get the attributes for the queue identified by the queue URL.
        /// </summary>
        /// <param name="queueUrl">The queue URL to get attributes for.</param>
        /// <returns>The attributes for the queue.</returns>
        Dictionary<string, string> GetAttributes(string queueUrl);
#endif
#if AWS_ASYNC_API
        /// <summary>
        /// Get the attributes for the queue identified by the queue URL asynchronously.
        /// </summary>
        /// <param name="queueUrl">The queue URL to get attributes for.</param>
        /// <returns>A Task containing the result of a dictionary of attributes for the queue.</returns>
        System.Threading.Tasks.Task<Dictionary<string, string>> GetAttributesAsync(string queueUrl);
#endif

#if BCL
        /// <summary>
        /// <para>
        /// This method is used internally to access the Amazon SQS service within other service assemblies.
        /// Please use AmazonSQSClient to access Amazon SQS instead.
        /// </para>
        /// Set the attributes on the queue identified by the queue URL.
        /// </summary>
        /// <param name="queueUrl">The queue URL to set the attributues.</param>
        /// <param name="attributes">The attributes to set.</param>
        void SetAttributes(string queueUrl, Dictionary<string, string> attributes);
#endif
#if AWS_ASYNC_API
        /// <summary>
        /// Set the attributes on the queue identified by the queue URL asynchronously.
        /// </summary>
        /// <param name="queueUrl">The queue URL to set the attributues.</param>
        /// <param name="attributes">The attributes to set.</param>
        /// <returns>A Task</returns>
        System.Threading.Tasks.Task SetAttributesAsync(string queueUrl, Dictionary<string, string> attributes);
#endif
    }
}
