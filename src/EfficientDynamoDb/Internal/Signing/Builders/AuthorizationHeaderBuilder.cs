using System.Runtime.CompilerServices;
using System.Text;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Signing.Constants;
using EfficientDynamoDb.Internal.Signing.Crypto;

namespace EfficientDynamoDb.Internal.Signing.Builders
{
    internal static class AuthorizationHeaderBuilder
    {
        public static string Build(string signedHeaders, string credentialScope, string stringToSign, in SigningMetadata metadata)
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
            var signingKey = ComposeSigningKey(metadata.Credentials.SecretKey, metadata.RegionName, metadata.Timestamp.ToIso8601BasicDate(), metadata.ServiceName);

            // Calculate the signature. To do this, use the signing key that you derived and the
            // string to sign as inputs to the keyed hash function. After you calculate the
            // signature, convert the binary value to a hexadecimal representation.
            var hash = ComputeKeyedHash(SigningAlgorithm.HmacSHA256, signingKey, stringToSign);
            var signature = hash.ToHex(true);

            return $"{SigningConstants.Aws4AlgorithmTag} Credential={metadata.Credentials.AccessKey}/{credentialScope}, SignedHeaders={signedHeaders}, Signature={signature}";
        }

        /// <summary>
        /// Compute and return the multi-stage signing key for the request.
        /// </summary>
        /// <param name="awsSecretAccessKey">The clear-text AWS secret key, if not held in secureKey</param>
        /// <param name="region">The region in which the service request will be processed</param>
        /// <param name="date">Date of the request, in yyyyMMdd format</param>
        /// <param name="service">The name of the service being called by the request</param>
        /// <returns>Computed signing key</returns>
        private static byte[] ComposeSigningKey(string awsSecretAccessKey, string region, string date, string service)
        {
            var combinedSecret = "AWS4" + awsSecretAccessKey;
            return ComputeKeyedHash(SigningAlgorithm.HmacSHA256,
                ComputeKeyedHash(SigningAlgorithm.HmacSHA256,
                    ComputeKeyedHash(SigningAlgorithm.HmacSHA256,
                        ComputeKeyedHash(SigningAlgorithm.HmacSHA256, Encoding.UTF8.GetBytes(combinedSecret), Encoding.UTF8.GetBytes(date)),
                        Encoding.UTF8.GetBytes(region)), Encoding.UTF8.GetBytes(service)), SigningConstants.AwsSignTerminatorBytes);
        }

        /// <summary>
        /// Compute and return the hash of a data blob using the specified key
        /// </summary>
        /// <param name="algorithm">Algorithm to use for hashing</param>
        /// <param name="key">Hash key</param>
        /// <param name="data">Data blob</param>
        /// <returns>Hash of the data</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte[] ComputeKeyedHash(SigningAlgorithm algorithm, byte[] key, byte[] data) => CryptoService.HmacSignBinary(data, key, algorithm);

        /// <summary>
        /// Compute and return the hash of a data blob using the specified key
        /// </summary>
        /// <param name="algorithm">Algorithm to use for hashing</param>
        /// <param name="key">Hash key</param>
        /// <param name="data">Data blob</param>
        /// <returns>Hash of the data</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte[] ComputeKeyedHash(SigningAlgorithm algorithm, byte[] key, string data) =>
            ComputeKeyedHash(algorithm, key, Encoding.UTF8.GetBytes(data));


    }
}