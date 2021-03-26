using System.Text.Json.Serialization;

namespace EfficientDynamoDb.Operations.Shared.Misc
{
    public readonly struct Range<T>
    {
        public T Min { get; }
        
        public T Max { get; }

        [JsonConstructor]
        public Range(T min, T max)
        {
            Min = min;
            Max = max;
        }
    }
}