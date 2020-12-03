namespace EfficientDynamoDb
{
    public static class GlobalDynamoDbConfig
    {
        /// <summary>
        /// Uses custom memory stream implemenation (does not use ArrayPool).
        /// </summary>
        public static bool UseMemoryStreamPooling { get; set; } = true;
    }
}