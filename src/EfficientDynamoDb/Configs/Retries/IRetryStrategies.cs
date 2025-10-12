using EfficientDynamoDb.Exceptions;

namespace EfficientDynamoDb.Configs.Retries
{
    public class RetryStrategies
    {
        /// <summary>
        /// Retry strategy for <see cref="InternalServerErrorException"/>
        /// </summary>
        public IRetryStrategy InternalServerErrorStrategy { get; set; } = RetryStrategyFactory.Linear();

        /// <summary>
        /// Retry strategy for <see cref="LimitExceededException"/>
        /// </summary>
        public IRetryStrategy LimitExceededStrategy { get; set; } = RetryStrategyFactory.Linear();

        /// <summary>
        /// Retry strategy for <see cref="ProvisionedThroughputExceededException"/>
        /// </summary>
        public IRetryStrategy ProvisionedThroughputExceededStrategy { get; set; } = RetryStrategyFactory.Jitter();

        /// <summary>
        /// Retry strategy for <see cref="RequestLimitExceededException"/>
        /// </summary>
        public IRetryStrategy RequestLimitExceededStrategy { get; set; } = RetryStrategyFactory.Jitter();

        /// <summary>
        /// Retry strategy for <see cref="ServiceUnavailableException"/>
        /// </summary>
        public IRetryStrategy ServiceUnavailableStrategy { get; set; } = RetryStrategyFactory.Linear();

        /// <summary>
        /// Retry strategy for <see cref="ThrottlingException"/>
        /// </summary>
        public IRetryStrategy ThrottlingStrategy { get; set; } = RetryStrategyFactory.Jitter();
        
        /// <summary>
        /// Retry strategy for <see ref="System.IO.IOException"/> or <see ref="System.Net.Http.HttpIOException"/> or <see cref="System.Net.Sockets.SocketException"/>. 
        /// </summary>
        public IRetryStrategy IoExceptionStrategy { get; set; } = RetryStrategyFactory.Linear();
    }
}