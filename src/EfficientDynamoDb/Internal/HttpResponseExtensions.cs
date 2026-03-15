using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EfficientDynamoDb.Internal
{
    internal static class HttpResponseExtensions
    {
        /// <summary>
        /// Returns a decoding stream for a successful response.
        /// Trusts the Content-Encoding header (DynamoDB is honest for 2xx responses).
        /// The returned stream owns the underlying stream when gzip is used.
        /// </summary>
        internal static async Task<Stream> GetDecodedStreamAsync(this HttpResponseMessage response, CancellationToken cancellationToken = default)
        {
            var rawStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            return !response.Content.Headers.ContentEncoding.Contains("gzip") 
                ? rawStream 
                : new GZipStream(rawStream, CompressionMode.Decompress, leaveOpen: false);
        }
    }
}
