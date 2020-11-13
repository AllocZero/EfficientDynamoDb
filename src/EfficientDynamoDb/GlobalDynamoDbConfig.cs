namespace EfficientDynamoDb
{
    public static class GlobalDynamoDbConfig
    {
        /// <summary>
        /// Uses custom memory stream implemenation (does not use ArrayPool).
        /// </summary>
        public static bool UseMemoryStreamPooling { get; set; } = true;

        /// <summary>
        /// Pooled buffer writer gives a huge performance boost and reduces memory allocations.
        /// </summary>
        public static bool UsePooledBufferForJsonWrites { get; set; } = true;
    }
}