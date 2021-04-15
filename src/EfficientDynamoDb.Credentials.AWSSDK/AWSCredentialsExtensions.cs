using Amazon.Runtime;
using EfficientDynamoDb.Configs;

namespace EfficientDynamoDb.Credentials.AWSSDK
{
    public static class AWSCredentialsExtensions
    {
        public static IAwsCredentialsProvider ToCredentialsProvider(this AWSCredentials awsCredentials) => new AWSCredentialsProvider(awsCredentials);
    }
}