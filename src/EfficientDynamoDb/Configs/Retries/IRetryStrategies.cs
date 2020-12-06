namespace EfficientDynamoDb.Configs.Retries
{
    public class RetryStrategies
    {
        public IRetryStrategy InternalServerErrorStrategy { get; set; } = DefaultRetryStrategy.Instance;

        public IRetryStrategy LimitExceededStrategy { get; set; } = DefaultRetryStrategy.Instance;

        public IRetryStrategy ProvisionedThroughputExceededStrategy { get; set; } = DefaultRetryStrategy.Instance;

        public IRetryStrategy RequestLimitExceededStrategy { get; set; } = DefaultRetryStrategy.Instance;

        public IRetryStrategy ServiceUnavailableStrategy { get; set; } = DefaultRetryStrategy.Instance;

        public IRetryStrategy ThrottlingStrategy { get; set; } = DefaultRetryStrategy.Instance;
    }
}