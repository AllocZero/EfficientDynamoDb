using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Extensions
{
    internal static class HttpResponseMessageExtensions
    {
        internal static async ValueTask<Document?> ReadDocumentAsync(this HttpResponseMessage response, IParsingOptions options, CancellationToken cancellationToken = default)
        {
            await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            var expectedCrc = response.GetExpectedCrc();
            var result = await DdbJsonReader.ReadAsync(responseStream, options, expectedCrc.HasValue, cancellationToken).ConfigureAwait(false);
            
            if (expectedCrc.HasValue && expectedCrc.Value != result.Crc)
                throw new ChecksumMismatchException();

            return result.Value;
        }
        
        internal static uint? GetExpectedCrc(this HttpResponseMessage response)
        {
            if (!response.Content.Headers.ContentLength.HasValue)
                return null;
            
            if (response.Headers.TryGetValues("x-amz-crc32", out var crcHeaderValues) && uint.TryParse(crcHeaderValues.FirstOrDefault(), out var expectedCrc))
                return expectedCrc;

            return null;
        }
    }
}