using System;
using EfficientDynamoDb.Internal.Core.Utilities;

namespace EfficientDynamoDb.Configs.Retries
{
    internal class JitterRetryStrategy : IRetryStrategy
    {
        private readonly IRandom _random;
        private readonly int _maxRetriesCount;
        private readonly int _baseDelayMs;
        private readonly int _maxDelayMs;

        internal JitterRetryStrategy(IRandom random, int maxRetriesCount, int baseDelayMs, int maxDelayMs)
        {
            _random = random;
            _maxRetriesCount = maxRetriesCount;
            _baseDelayMs = baseDelayMs;
            _maxDelayMs = maxDelayMs;
        }

        public bool TryGetRetryDelay(int attempt, out TimeSpan delay)
        {
            var upperLimitMs = Math.Min(_maxDelayMs, _baseDelayMs * FastMath.TwoPowX(attempt));
            delay = TimeSpan.FromMilliseconds(_random.Next(upperLimitMs));

            return attempt < _maxRetriesCount;
        }
    }
}