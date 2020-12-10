using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace EfficientDynamoDb.Internal.Signing.Crypto
{
    internal static class CryptoService
    {
        [ThreadStatic] private static HashAlgorithm? _hashAlgorithm;
        
        private static HashAlgorithm Sha256HashAlgorithmInstance => _hashAlgorithm ??= SHA256.Create();

        public static byte[] ComputeSha256Hash(Stream stream) => Sha256HashAlgorithmInstance.ComputeHash(stream);

        public static byte[] ComputeSha256Hash(byte[] data) => Sha256HashAlgorithmInstance.ComputeHash(data);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ComputeSha256Hash(in ReadOnlySpan<byte> data, Span<byte> destination, out int bytesWritten)
        {
            if (!Sha256HashAlgorithmInstance.TryComputeHash(data, destination, out bytesWritten))
                throw new InvalidOperationException("Couldn't compute SHA256 hash");
        }
        
        public static bool TryComputeSha256Hash(in ReadOnlySpan<byte> data, Span<byte> destination, out int bytesWritten) => Sha256HashAlgorithmInstance.TryComputeHash(data, destination, out bytesWritten);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryHmacSignBinary(KeyedHashAlgorithm algorithm, ReadOnlySpan<byte> data, Span<byte> destination, out int bytesWritten)
        {
            if (data.Length == 0)
                throw new ArgumentNullException(nameof(data), "Please specify data to sign.");

            return algorithm.TryComputeHash(data, destination, out bytesWritten);
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