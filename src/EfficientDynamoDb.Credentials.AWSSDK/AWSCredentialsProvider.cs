using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using EfficientDynamoDb.Configs;

namespace EfficientDynamoDb.Credentials.AWSSDK
{
    public class AWSCredentialsProvider : IAwsCredentialsProvider
    {
        private readonly AWSCredentials _awsCredentials;

        public AWSCredentialsProvider(AWSCredentials awsCredentials)
        {
            _awsCredentials = awsCredentials;
        }

        public async ValueTask<AwsCredentials> GetCredentialsAsync(CancellationToken cancellationToken = default)
        {
            var credentials = await _awsCredentials.GetCredentialsAsync().ConfigureAwait(false);

            return new AwsCredentials(credentials.AccessKey, credentials.SecretKey, credentials.UseToken ? credentials.Token : null);
        }
    }
}