using System;
using System.Runtime.InteropServices;

namespace EfficientDynamoDb.Configs
{
    public class AwsCredentials : IEquatable<AwsCredentials>
    {
        public string AccessKey { get; }
        
        public string SecretKey { get; }
        
        public string? Token { get; }

        public bool UseToken => Token != null;

        public AwsCredentials(string accessKey, string secretKey) : this(accessKey, secretKey, null)
        {
        }

        public AwsCredentials(string accessKey, string secretKey, string? token)
        {
            AccessKey = accessKey;
            SecretKey = secretKey;
            Token = token;
        }

        public bool Equals(AwsCredentials other) => AccessKey == other.AccessKey && SecretKey == other.SecretKey;

        public override bool Equals(object obj) => obj is AwsCredentials other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(AccessKey, SecretKey);

        public static bool operator ==(AwsCredentials left, AwsCredentials right) => left.Equals(right);

        public static bool operator !=(AwsCredentials left, AwsCredentials right) => !left.Equals(right);
    }
}