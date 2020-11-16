using System.Text;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Signing.Constants;
using EfficientDynamoDb.Internal.Signing.Crypto;

namespace EfficientDynamoDb.Internal.Signing.Builders
{
    internal static class StringToSignBuilder
    {
         /// <returns>
        /// The first value is the string to sign, the second value is the credentials scope.
        /// </returns>
        public static (string, string) Build(string canonicalRequest, in SigningMetadata metadata)
        {
            var builder = new StringBuilder();

            // Start with the algorithm designation, followed by a newline character. This value is
            // the hashing algorithm that you use to calculate the digests in the canonical
            // request. For SHA256, AWS4-HMAC-SHA256 is the algorithm.
            builder.Append($"{SigningConstants.Aws4AlgorithmTag}\n");

            // Append the request date value, followed by a newline character. The date is
            // specified with ISO8601 basic format in the x-amz-date header in the format
            // YYYYMMDD'T'HHMMSS'Z'. This value must match the value you used in any previous
            // steps.
            builder.Append($"{metadata.Timestamp.ToIso8601BasicDateTime()}\n");

            // Append the credential scope value, followed by a newline character. This value is a
            // string that includes the date, the region you are targeting, the service you are
            // requesting, and a termination string ("aws4_request") in lowercase characters. The
            // region and service name strings must be UTF-8 encoded.
            //
            // - The date must be in the YYYYMMDD format. Note that the date does not include a
            //   time value.
            // - Verify that the region you specify is the region that you are sending the request
            //   to. See <see href="https://docs.aws.amazon.com/general/latest/gr/rande.html">AWS
            //   Regions and Endpoints</see>.
            var credentialScope = $"{metadata.Timestamp.ToIso8601BasicDate()}/{metadata.RegionName}/{metadata.ServiceName}/{SigningConstants.AwsSignTerminator}";
            builder.Append($"{credentialScope}\n");

            // Append the hash of the canonical request. This value is not followed by a newline
            // character. The hashed canonical request must be lowercase base-16 encoded, as
            // defined by <see href="https://tools.ietf.org/html/rfc4648#section-8">Section 8 of
            // RFC 4648</see>.

            var bytes = Encoding.UTF8.GetBytes(canonicalRequest);
            var hash = CryptoService.ComputeSha256Hash(bytes);
            var hex = hash.ToHex(true);
            builder.Append(hex);

            return (builder.ToString(), credentialScope);
        }
    }
}