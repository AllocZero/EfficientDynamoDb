using Amazon.Runtime;
using EfficientDynamoDb.Configs;

namespace EfficientDynamoDb.Credentials.AWSSDK
{
    public static class AWSCredentialsExtensions
    {
        /// <summary>
        /// Converts <see cref="AWSCredentials"/> to credentials provider that is used by <see cref="DynamoDbContextConfig"/>.
        /// </summary>
        /// <param name="awsCredentials">AWS SDK credentials instance to convert.</param>
        /// <returns></returns>
        public static IAwsCredentialsProvider ToCredentialsProvider(this AWSCredentials awsCredentials) => new AWSCredentialsProvider(awsCredentials);
    }
}