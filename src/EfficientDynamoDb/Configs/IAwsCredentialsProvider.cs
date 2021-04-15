using System.Threading;
using System.Threading.Tasks;

namespace EfficientDynamoDb.Configs
{
    public interface IAwsCredentialsProvider
    {
        ValueTask<AwsCredentials> GetCredentialsAsync(CancellationToken cancellationToken = default);
    }
}