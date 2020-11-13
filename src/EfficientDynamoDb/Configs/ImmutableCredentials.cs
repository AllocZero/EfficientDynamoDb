using System;
using System.Runtime.InteropServices;

namespace EfficientDynamoDb.Configs
{
    [StructLayout(LayoutKind.Auto)]
    public readonly struct ImmutableCredentials : IEquatable<ImmutableCredentials>
    {
        public string AccessKey { get; }
        
        public string SecretKey { get; }
        
        public string? Token { get; }

        public bool UseToken => Token != null;

        public ImmutableCredentials(string accessKey, string secretKey, string? token = null)
        {
            AccessKey = accessKey;
            SecretKey = secretKey;
            Token = token;
        }

        public bool Equals(ImmutableCredentials other) => AccessKey == other.AccessKey && SecretKey == other.SecretKey;

        public override bool Equals(object obj) => obj is ImmutableCredentials other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(AccessKey, SecretKey);

        public static bool operator ==(ImmutableCredentials left, ImmutableCredentials right) => left.Equals(right);

        public static bool operator !=(ImmutableCredentials left, ImmutableCredentials right) => !left.Equals(right);
    }
}