using System;

namespace EfficientDynamoDb.Configs.Retries
{
    internal sealed class LinearRetryStrategy : IRetryStrategy
    {
        private readonly int _maxRetriesCount;
        private readonly int _delayMs;

        public LinearRetryStrategy(int maxRetriesCount, int delayMs)
        {
            _maxRetriesCount = maxRetriesCount;
            _delayMs = delayMs;
        }

        public bool TryGetRetryDelay(int attempt, out TimeSpan delay)
        {
            delay = TimeSpan.FromMilliseconds(_delayMs);
            return attempt < _maxRetriesCount;
        }
    }
}