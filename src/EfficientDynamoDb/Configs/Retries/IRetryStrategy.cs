using System;

namespace EfficientDynamoDb.Configs.Retries
{
    public interface IRetryStrategy
    {
        public bool TryGetRetryDelay(int attempt, out TimeSpan delay);
    }
}