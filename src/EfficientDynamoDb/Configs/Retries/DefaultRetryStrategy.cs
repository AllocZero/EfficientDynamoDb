using System;

namespace EfficientDynamoDb.Configs.Retries
{
    internal class DefaultRetryStrategy : IRetryStrategy
    {
        private readonly int _maxRetriesCount;

        public static readonly DefaultRetryStrategy Instance = new DefaultRetryStrategy(5);

        public DefaultRetryStrategy(int maxRetriesCount)
        {
            _maxRetriesCount = maxRetriesCount;
        }

        public bool TryGetRetryDelay(int attempt, out TimeSpan delay)
        {
            delay = TimeSpan.FromMilliseconds(50);
            return attempt < _maxRetriesCount;
        }
    }
}