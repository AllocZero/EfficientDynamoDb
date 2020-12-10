using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using EfficientDynamoDb.Context.Config;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Signing.Constants;
using EfficientDynamoDb.Internal.Signing.Crypto;

namespace EfficientDynamoDb.Internal.Signing.Builders
{
    internal static class AuthorizationHeaderBuilder
    {
        private const int MaxStackAllocSize = 256;

        public static string Build(ref NoAllocStringBuilder builder, ref NoAllocStringBuilder signedHeadersBuilder, in SigningMetadata metadata)
        {
            // The following pseudocode shows the construction of the Authorization header value.
            //
            //   <algorithm> Credential=<access key id>/<credential scope>, SignedHeaders=<signed headers>, Signature=<signature>
            //
            // Note the following:
            //
            // - There is no comma between the algorithm and Credential. However, the SignedHeaders
            //   and Signature are separated from the preceding values with a comma.
            // - The Credential value starts with the access key id, which is followed by a forward
            //   slash (/), which is followed by the credential scope. The secret access key is
            //   used to derive the signing key for the signature, but is not included in the
            //   signing information sent in the request.
            //
            // To derive your signing key, use your secret access key to create a series of hash-
            // based message authentication codes (HMACs).
            //
            // Note that the date used in the hashing process is in the format YYYYMMDD (for
            // example, 20150830), and does not include the time.

            Span<byte> sourceDataBuffer = stackalloc byte[32];
            Span<byte> destinationDataBuffer = stackalloc byte[32];
            var keysBuffer = ArrayPool<byte>.Shared.Rent(64); // Can't stackalloc because Keys is a byte array

            try
            {
                keysBuffer.AsSpan().Clear();

                var prefixLength = Encoding.UTF8.GetBytes("AWS4", keysBuffer);
                Encoding.UTF8.GetBytes(metadata.Credentials.SecretKey, keysBuffer.AsSpan(prefixLength));

                using var algorithm = new HMACSHA256(keysBuffer);

                ComputeKeyedSha256Hash(algorithm, ref sourceDataBuffer, ref destinationDataBuffer, ref keysBuffer, metadata.Timestamp.ToIso8601BasicDate());
                ComputeKeyedSha256Hash(algorithm, ref sourceDataBuffer, ref destinationDataBuffer, ref keysBuffer, metadata.RegionEndpoint.Region);
                ComputeKeyedSha256Hash(algorithm, ref sourceDataBuffer, ref destinationDataBuffer, ref keysBuffer, RegionEndpoint.ServiceName);
                ComputeKeyedSha256Hash(algorithm, ref sourceDataBuffer, ref destinationDataBuffer, ref keysBuffer, SigningConstants.AwsSignTerminator);

                // Calculate the signature. To do this, use the signing key that you derived and the
                // string to sign as inputs to the keyed hash function. After you calculate the
                // signature, convert the binary value to a hexadecimal representation.
                ComputeKeyedSha256Hash(algorithm, ref sourceDataBuffer, ref destinationDataBuffer, ref keysBuffer, builder.GetBuffer());

                builder.Clear();
                builder.Append(SigningConstants.Aws4AlgorithmTag);
                builder.Append(" Credential=");
                builder.Append(metadata.Credentials.AccessKey);
                builder.Append('/');

                // Credential Scope
                builder.AppendCredentialScope(metadata);

                builder.Append(", SignedHeaders=");
                builder.Append(signedHeadersBuilder.GetBuffer());
                builder.Append(", Signature=");

                foreach (var item in destinationDataBuffer)
                {
                    builder.Append(HexAlphabet.Lowercase[item >> 4]);
                    builder.Append(HexAlphabet.Lowercase[item & 0xF]);
                }
            }
            finally
            {
                keysBuffer.AsSpan().Clear();
                ArrayPool<byte>.Shared.Return(keysBuffer);
            }

            return builder.ToString();
        }

        private static void ComputeKeyedSha256Hash(KeyedHashAlgorithm algorithm, ref Span<byte> sourceDataBuffer, ref Span<byte> destinationDataBuffer, ref byte[] keysBuffer, ReadOnlySpan<char> data)
        {
            var utf8Length = Encoding.UTF8.GetByteCount(data);
            byte[]? rentedBuffer = null;

            var source = utf8Length <= sourceDataBuffer.Length
                ? sourceDataBuffer
                : utf8Length <= MaxStackAllocSize // Only expected to allocate more data during the last step when we hash all data
                    ? stackalloc byte[utf8Length]
                    : rentedBuffer = ArrayPool<byte>.Shared.Rent(utf8Length);

            try
            {
                var sourceDataSize = Encoding.UTF8.GetBytes(data, source);

                if (!CryptoService.TryHmacSignBinary(algorithm, source.Slice(0, sourceDataSize), keysBuffer, destinationDataBuffer, out _))
                    throw new InvalidOperationException("Couldn't generate HmacSHA256 hash.");

                destinationDataBuffer.CopyTo(keysBuffer);
                keysBuffer.AsSpan(destinationDataBuffer.Length).Clear();
            }
            finally
            {
                if (rentedBuffer != null)
                {
                    rentedBuffer.AsSpan(0, utf8Length).Clear();
                    ArrayPool<byte>.Shared.Return(rentedBuffer);
                }
            }
        }
    }
}