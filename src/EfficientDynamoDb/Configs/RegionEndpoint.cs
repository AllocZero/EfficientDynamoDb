namespace EfficientDynamoDb.Configs
{
    public class RegionEndpoint
    {
        private const string ChinaEndpointFormat = "https://{0}.{1}.amazonaws.com.cn"; // 0 - Service Name, 1 - Region
        private const string RegularEndpointFormat = "https://{0}.{1}.amazonaws.com"; // 0 - Service Name, 1 - Region
        
        internal const string ServiceName = "dynamodb";

        internal string RequestUri { get; }
        
        public string Region { get; }

        static RegionEndpoint Create(string region, string uriFormat)
        {
            return new RegionEndpoint(region, string.Format(uriFormat, ServiceName, region));
        }
        
        public RegionEndpoint(string region, string requestUri)
        {
            Region = region;
            RequestUri = requestUri;
        }

        /// <summary>
        /// The US East (N. Virginia) endpoint.
        /// </summary>
        public static RegionEndpoint USEast1 => Create("us-east-1", RegularEndpointFormat);
        
        /// <summary>
        /// The US East (Ohio) endpoint.
        /// </summary>
        public static RegionEndpoint USEast2 => Create("us-east-2", RegularEndpointFormat);
        
        /// <summary>
        /// The US West (N. California) endpoint.
        /// </summary>
        public static RegionEndpoint USWest1 => Create("us-west-1", RegularEndpointFormat);
        
        /// <summary>
        /// The US West (Oregon) endpoint.
        /// </summary>
        public static RegionEndpoint USWest2 => Create("us-west-2", RegularEndpointFormat);
        
        /// <summary>
        /// The Africa (Cape Town) endpoint.
        /// </summary>
        public static RegionEndpoint AFSouth1 => Create("af-south-1", RegularEndpointFormat);
        
        /// <summary>
        /// The Asia Pacific (Hong Kong) endpoint.
        /// </summary>
        public static RegionEndpoint APEast1 => Create("ap-east-1", RegularEndpointFormat);
        
        /// <summary>
        /// The Asia Pacific (Mumbai) endpoint.
        /// </summary>
        public static RegionEndpoint APSouth1 => Create("ap-south-1", RegularEndpointFormat);
        
        /// <summary>
        /// The Asia Pacific (Tokyo) endpoint.
        /// </summary>
        public static RegionEndpoint APNorthEast1 => Create("ap-northeast-1", RegularEndpointFormat);
        
        /// <summary>
        /// The Asia Pacific (Seoul) endpoint.
        /// </summary>
        public static RegionEndpoint APNorthEast2 => Create("ap-northeast-2", RegularEndpointFormat);
        
        /// <summary>
        /// The Asia Pacific (Osaka-Local) endpoint.
        /// </summary>
        public static RegionEndpoint APNorthEast3 => Create("ap-northeast-3", RegularEndpointFormat);
        
        /// <summary>
        /// The Asia Pacific (Singapore) endpoint.
        /// </summary>
        public static RegionEndpoint APSouthEast1 => Create("ap-southeast-1", RegularEndpointFormat);
        
        /// <summary>
        /// The Asia Pacific (Sydney) endpoint.
        /// </summary>
        public static RegionEndpoint APSouthEast2 => Create("ap-southeast-2", RegularEndpointFormat);
        
        /// <summary>
        /// The Canada (Central) endpoint.
        /// </summary>
        public static RegionEndpoint CACentral1 => Create("ca-central-1", RegularEndpointFormat);
        
        /// <summary>
        /// The China (Beijing) endpoint.
        /// </summary>
        public static RegionEndpoint CNNorth1 => Create("cn-north-1", ChinaEndpointFormat);
        
        /// <summary>
        /// The China (Ningxia) endpoint.
        /// </summary>
        public static RegionEndpoint CNNorthWest1 => Create("cn-northwest-1", ChinaEndpointFormat);
        
        /// <summary>
        /// The Europe (Frankfurt) endpoint.
        /// </summary>
        public static RegionEndpoint EUCenteral1 => Create("eu-central-1", RegularEndpointFormat);
        
        /// <summary>
        /// The Europe (Ireland) endpoint.
        /// </summary>
        public static RegionEndpoint EUWest1 => Create("eu-west-1", RegularEndpointFormat);
        
        /// <summary>
        /// The Europe (London) endpoint.
        /// </summary>
        public static RegionEndpoint EUWest2 => Create("eu-west-2", RegularEndpointFormat);
        
        /// <summary>
        /// The Europe (Paris) endpoint.
        /// </summary>
        public static RegionEndpoint EUWest3 => Create("eu-west-3", RegularEndpointFormat);
        
        /// <summary>
        /// The Europe (Milan) endpoint.
        /// </summary>
        public static RegionEndpoint EUSouth1 => Create("eu-south-1", RegularEndpointFormat);
        
        /// <summary>
        /// The Europe (Stockholm) endpoint.
        /// </summary>
        public static RegionEndpoint EUNorth1 => Create("eu-north-1", RegularEndpointFormat);
        
        /// <summary>
        /// The Middle East (Bahrain) endpoint.
        /// </summary>
        public static RegionEndpoint MESouth1 => Create("me-south-1", RegularEndpointFormat);
        
        /// <summary>
        /// The South America (São Paulo) endpoint.
        /// </summary>
        public static RegionEndpoint SAEast1 => Create("sa-east-1", RegularEndpointFormat);
        
        /// <summary>
        /// The AWS GovCloud (US-East) endpoint.
        /// </summary>
        public static RegionEndpoint USGovEast1 => Create("us-gov-east-1", RegularEndpointFormat);
        
        /// <summary>
        /// The AWS GovCloud (US) endpoint.
        /// </summary>
        public static RegionEndpoint USGovWest1 => Create("us-gov-west-1", RegularEndpointFormat);
    }
}