namespace EfficientDynamoDb.Context.Config
{
    public class RegionEndpoint
    {
        public string SystemName { get; }

        public RegionEndpoint(string systemName) => SystemName = systemName;

        public static RegionEndpoint USEast1 { get; } = new RegionEndpoint("us-east-1");
    }
}