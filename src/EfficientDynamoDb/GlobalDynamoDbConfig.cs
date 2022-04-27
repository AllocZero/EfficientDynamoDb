namespace EfficientDynamoDb
{
    public static class GlobalDynamoDbConfig
    {
        /// <summary>
        /// Uses custom memory stream implementation (does not use ArrayPool).
        /// </summary>
        public static bool UseMemoryStreamPooling { get; set; } = true;

        /// <summary>
        /// Automatically interns attribute names to reduce memory allocations and increase deserialization performance. Does not use built-in C# intern functionality and instead relies on the custom JSON-optimized logic.
        /// </summary>
        public static bool InternAttributeNames { get; set; } = true;
    }
}