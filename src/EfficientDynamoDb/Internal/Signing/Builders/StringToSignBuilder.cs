using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Signing.Constants;
using EfficientDynamoDb.Internal.Signing.Crypto;

namespace EfficientDynamoDb.Internal.Signing.Builders
{
    internal static class StringToSignBuilder
    {
        private const int MaxStackAllocSize = 256;

        /// <returns>
        /// The first value is the string to sign, the second value is the credentials scope.
        /// </returns>
        public static void Build(ref NoAllocStringBuilder builder, in SigningMetadata metadata)
        {
            var charBuffer = builder.GetBuffer();
            var utf8BufferSize = Encoding.UTF8.GetByteCount(charBuffer);

            byte[]? rentedCanonicalUtf8Request = null;
            var canonicalUtf8Request = utf8BufferSize <= MaxStackAllocSize
                ? stackalloc byte[utf8BufferSize]
                : (rentedCanonicalUtf8Request = ArrayPool<byte>.Shared.Rent(utf8BufferSize));
            
            try
            {
                var canonicalRequestBytes = Encoding.UTF8.GetBytes(charBuffer, canonicalUtf8Request);
                builder.Clear();
                
                // Start with the algorithm designation, followed by a newline character. This value is
                // the hashing algorithm that you use to calculate the digests in the canonical
                // request. For SHA256, AWS4-HMAC-SHA256 is the algorithm.
                builder.Append(SigningConstants.Aws4AlgorithmTag);
                builder.Append('\n');

                // Append the request date value, followed by a newline character. The date is
                // specified with ISO8601 basic format in the x-amz-date header in the format
                // YYYYMMDD'T'HHMMSS'Z'. This value must match the value you used in any previous
                // steps.
                builder.Append(metadata.TimestampIso8601BasicDateTimeString);
                builder.Append('\n');

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
                builder.AppendCredentialScope(metadata);
                builder.Append('\n');

                // Append the hash of the canonical request. This value is not followed by a newline
                // character. The hashed canonical request must be lowercase base-16 encoded, as
                // defined by <see href="https://tools.ietf.org/html/rfc4648#section-8">Section 8 of
                // RFC 4648</see>.

                
                Span<byte> hash = stackalloc byte[32];
                CryptoService.ComputeSha256Hash(canonicalUtf8Request.Slice(0, canonicalRequestBytes), hash, out _);

                foreach (var item in hash)
                {
                    builder.Append(HexAlphabet.Lowercase[item >> 4]);
                    builder.Append(HexAlphabet.Lowercase[item & 0xF]);
                }
            }
            finally
            {
                if (rentedCanonicalUtf8Request != null)
                {
                    rentedCanonicalUtf8Request.AsSpan(0, utf8BufferSize).Clear();
                    ArrayPool<byte>.Shared.Return(rentedCanonicalUtf8Request);
                }
            }
        }
    }
}