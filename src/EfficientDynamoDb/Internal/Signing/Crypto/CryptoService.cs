using System;
using System.IO;
using System.Security.Cryptography;

namespace EfficientDynamoDb.Internal.Signing.Crypto
{
    internal static class CryptoService
    {
        [ThreadStatic] private static HashAlgorithm? _hashAlgorithm;

        private static HashAlgorithm Sha256HashAlgorithmInstance => _hashAlgorithm ??= SHA256.Create();

        public static byte[] ComputeSha256Hash(Stream stream) => Sha256HashAlgorithmInstance.ComputeHash(stream);

        public static byte[] ComputeSha256Hash(byte[] data) => Sha256HashAlgorithmInstance.ComputeHash(data);

        public static void ComputeSha256Hash(in ReadOnlySpan<byte> data, Span<byte> destination, out int bytesWritten)
        {
            if (!Sha256HashAlgorithmInstance.TryComputeHash(data, destination, out bytesWritten))
                throw new InvalidOperationException("Couldn't compute SHA256 hash");
        }
        
        public static bool TryComputeSha256Hash(in ReadOnlySpan<byte> data, Span<byte> destination, out int bytesWritten) => Sha256HashAlgorithmInstance.TryComputeHash(data, destination, out bytesWritten);

        public static byte[] HmacSignBinary(byte[] data, byte[] key, SigningAlgorithm algorithmName)
        {
            if (key.Length == 0)
                throw new ArgumentNullException(nameof(key), "Please specify a Secret Signing Key.");
            if (data.Length == 0)
                throw new ArgumentNullException(nameof(data), "Please specify data to sign.");

            using var keyedHashAlgorithm = CreateKeyedHashAlgorithm(algorithmName, key);
            return keyedHashAlgorithm.ComputeHash(data);
        }
        
        public static bool TryHmacSignBinary(ReadOnlySpan<byte> data, byte[] key, Span<byte> destination, SigningAlgorithm algorithmName, out int bytesWritten)
        {
            if (key.Length == 0)
                throw new ArgumentNullException(nameof(key), "Please specify a Secret Signing Key.");
            if (data.Length == 0)
                throw new ArgumentNullException(nameof(data), "Please specify data to sign.");

            using var keyedHashAlgorithm = CreateKeyedHashAlgorithm(algorithmName, key);
            return keyedHashAlgorithm.TryComputeHash(data, destination, out bytesWritten);
        }
        

        // TODO: Consider pooling keyed hash algorithms
        private static KeyedHashAlgorithm CreateKeyedHashAlgorithm(SigningAlgorithm algorithm, byte[] key) => algorithm switch
        {
            SigningAlgorithm.HmacSHA1 => new HMACSHA1 {Key = key},
            SigningAlgorithm.HmacSHA256 => new HMACSHA256 {Key = key},
            _ => throw new NotSupportedException($"Keyed hash algorithm {algorithm.ToString()} is not supported")
        };
    }
}