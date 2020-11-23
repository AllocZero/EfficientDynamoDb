namespace EfficientDynamoDb.Context.Responses.Misc
{
    public class Range<T>
    {
        public T Min { get; }
        
        public T Max { get; }

        public Range(T min, T max)
        {
            Min = min;
            Max = max;
        }
    }
}