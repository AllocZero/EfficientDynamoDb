using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace EfficientDynamoDb.Configs
{
    public class AwsCredentials : IEquatable<AwsCredentials>, IAwsCredentialsProvider
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
        
        public ValueTask<AwsCredentials> GetCredentialsAsync(CancellationToken cancellationToken = default) => new ValueTask<AwsCredentials>(this);

        public bool Equals(AwsCredentials? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return AccessKey == other.AccessKey && SecretKey == other.SecretKey && Token == other.Token;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AwsCredentials) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AccessKey, SecretKey, Token);
        }

        public static bool operator ==(AwsCredentials left, AwsCredentials right) => left.Equals(right);

        public static bool operator !=(AwsCredentials left, AwsCredentials right) => !left.Equals(right);
    }
}