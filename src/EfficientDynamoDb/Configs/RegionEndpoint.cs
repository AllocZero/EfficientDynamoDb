namespace EfficientDynamoDb.Configs
{
    public class RegionEndpoint
    {
        private const string ChinaEndpointFormat = "https://{0}.{1}.amazonaws.com.cn"; // 0 - Service Name, 1 - Region
        private const string RegularEndpointFormat = "https://{0}.{1}.amazonaws.com"; // 0 - Service Name, 1 - Region
        
        internal const string ServiceName = "dynamodb";

        internal string RequestUri { get; private set; }
        
        public string Region { get; }

        public static RegionEndpoint Create(string region)
        {
            return new RegionEndpoint(region, RegularEndpointFormat);
        }

        public static RegionEndpoint Create(string region, string requestUri)
        {
            return new RegionEndpoint(region, RegularEndpointFormat) { RequestUri = requestUri };
        }

        public static RegionEndpoint Create(RegionEndpoint region, string requestUri)
        {
            return new RegionEndpoint(region.Region, RegularEndpointFormat) { RequestUri = requestUri };
        }

        private RegionEndpoint(string region, string uriFormat = RegularEndpointFormat)
        {
            Region = region;
            RequestUri = string.Format(uriFormat, ServiceName, region);
        }

        /// <summary>
        /// The US East (N. Virginia) endpoint.
        /// </summary>
        public static RegionEndpoint USEast1 => new RegionEndpoint("us-east-1");
        
        /// <summary>
        /// The US East (Ohio) endpoint.
        /// </summary>
        public static RegionEndpoint USEast2 => new RegionEndpoint("us-east-2");
        
        /// <summary>
        /// The US West (N. California) endpoint.
        /// </summary>
        public static RegionEndpoint USWest1 => new RegionEndpoint("us-west-1");
        
        /// <summary>
        /// The US West (Oregon) endpoint.
        /// </summary>
        public static RegionEndpoint USWest2 => new RegionEndpoint("us-west-2");
        
        /// <summary>
        /// The Africa (Cape Town) endpoint.
        /// </summary>
        public static RegionEndpoint AFSouth1 => new RegionEndpoint("af-south-1");
        
        /// <summary>
        /// The Asia Pacific (Hong Kong) endpoint.
        /// </summary>
        public static RegionEndpoint APEast1 => new RegionEndpoint("ap-east-1");
        
        /// <summary>
        /// The Asia Pacific (Mumbai) endpoint.
        /// </summary>
        public static RegionEndpoint APSouth1 => new RegionEndpoint("ap-south-1");
        
        /// <summary>
        /// The Asia Pacific (Tokyo) endpoint.
        /// </summary>
        public static RegionEndpoint APNorthEast1 => new RegionEndpoint("ap-northeast-1");
        
        /// <summary>
        /// The Asia Pacific (Seoul) endpoint.
        /// </summary>
        public static RegionEndpoint APNorthEast2 => new RegionEndpoint("ap-northeast-2");
        
        /// <summary>
        /// The Asia Pacific (Osaka-Local) endpoint.
        /// </summary>
        public static RegionEndpoint APNorthEast3 => new RegionEndpoint("ap-northeast-3");
        
        /// <summary>
        /// The Asia Pacific (Singapore) endpoint.
        /// </summary>
        public static RegionEndpoint APSouthEast1 => new RegionEndpoint("ap-southeast-1");
        
        /// <summary>
        /// The Asia Pacific (Sydney) endpoint.
        /// </summary>
        public static RegionEndpoint APSouthEast2 => new RegionEndpoint("ap-southeast-2");
        
        /// <summary>
        /// The Canada (Central) endpoint.
        /// </summary>
        public static RegionEndpoint CACentral1 => new RegionEndpoint("ca-central-1");
        
        /// <summary>
        /// The China (Beijing) endpoint.
        /// </summary>
        public static RegionEndpoint CNNorth1 => new RegionEndpoint("cn-north-1", ChinaEndpointFormat);
        
        /// <summary>
        /// The China (Ningxia) endpoint.
        /// </summary>
        public static RegionEndpoint CNNorthWest1 => new RegionEndpoint("cn-northwest-1", ChinaEndpointFormat);
        
        /// <summary>
        /// The Europe (Frankfurt) endpoint.
        /// </summary>
        public static RegionEndpoint EUCenteral1 => new RegionEndpoint("eu-central-1");
        
        /// <summary>
        /// The Europe (Ireland) endpoint.
        /// </summary>
        public static RegionEndpoint EUWest1 => new RegionEndpoint("eu-west-1");
        
        /// <summary>
        /// The Europe (London) endpoint.
        /// </summary>
        public static RegionEndpoint EUWest2 => new RegionEndpoint("eu-west-2");
        
        /// <summary>
        /// The Europe (Paris) endpoint.
        /// </summary>
        public static RegionEndpoint EUWest3 => new RegionEndpoint("eu-west-3");
        
        /// <summary>
        /// The Europe (Milan) endpoint.
        /// </summary>
        public static RegionEndpoint EUSouth1 => new RegionEndpoint("eu-south-1");
        
        /// <summary>
        /// The Europe (Stockholm) endpoint.
        /// </summary>
        public static RegionEndpoint EUNorth1 => new RegionEndpoint("eu-north-1");
        
        /// <summary>
        /// The Middle East (Bahrain) endpoint.
        /// </summary>
        public static RegionEndpoint MESouth1 => new RegionEndpoint("me-south-1");
        
        /// <summary>
        /// The South America (São Paulo) endpoint.
        /// </summary>
        public static RegionEndpoint SAEast1 => new RegionEndpoint("sa-east-1");
        
        /// <summary>
        /// The AWS GovCloud (US-East) endpoint.
        /// </summary>
        public static RegionEndpoint USGovEast1 => new RegionEndpoint("us-gov-east-1");
        
        /// <summary>
        /// The AWS GovCloud (US) endpoint.
        /// </summary>
        public static RegionEndpoint USGovWest1 => new RegionEndpoint("us-gov-west-1");
    }
}