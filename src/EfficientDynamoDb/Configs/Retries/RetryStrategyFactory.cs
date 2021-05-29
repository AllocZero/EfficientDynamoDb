using EfficientDynamoDb.Internal.Core.Utilities;

namespace EfficientDynamoDb.Configs.Retries
{
    public static class RetryStrategyFactory
    {
        /// <summary>
        /// Creates an exponential backoff strategy with jitter.
        /// </summary>
        /// <param name="maxRetriesCount">Maximum retry attempts.</param>
        /// <param name="baseDelayMs">Base delay in milliseconds. I.e., delay on the first retry.</param>
        /// <param name="maxDelayMs">Maximum allowed delay in milliseconds. Delay can't be longer than value of this parameter regardless number of retries.</param>
        /// <returns>Configured retry strategy.</returns>
        /// <remarks>
        /// Represents <c>FullJitter</c> retry strategy from this article: https://aws.amazon.com/blogs/architecture/exponential-backoff-and-jitter/
        /// </remarks>
        public static IRetryStrategy Jitter(int maxRetriesCount = 5, int baseDelayMs = 50, int maxDelayMs = 16_000) =>
            new JitterRetryStrategy(ThreadSafeRandom.Instance, maxRetriesCount, baseDelayMs, maxDelayMs);

        /// <summary>
        /// Creates a linear strategy.
        /// </summary>
        /// <param name="maxRetriesCount">Maximum retry attempts.</param>
        /// <param name="delayMs">Delay in milliseconds.</param>
        /// <returns>Configured retry strategy.</returns>
        public static IRetryStrategy Linear(int maxRetriesCount = 5, int delayMs = 50) => new LinearRetryStrategy(maxRetriesCount, delayMs);
    }
}