using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;


namespace Amazon.Util.Internal
{
    /// <summary>
    /// Root AWS config
    /// </summary>
    public partial class RootConfig
    {
        public CSMConfig CSMConfig { get; set; }
        public LoggingConfig Logging { get; private set; }
        public ProxyConfig Proxy { get; private set; }
        public string EndpointDefinition { get; set; }
        public string Region { get; set; }
        public string ProfileName { get; set; }
        public string ProfilesLocation { get; set; }
        public RegionEndpoint RegionEndpoint
        {
            get
            {
                if (string.IsNullOrEmpty(Region))
                    return null;
                return RegionEndpoint.GetBySystemName(Region);
            }
            set
            {
                if (value == null)
                    Region = null;
                else
                    Region = value.SystemName;
            }
        }
        public bool UseSdkCache { get; set; }

        public bool CorrectForClockSkew { get; set; }

        public bool UseAlternateUserAgentHeader { get; set; }

        public string ApplicationName { get; set; }

        public bool? CSMEnabled { get; set; }
        public string CSMClientId { get; set; }
        public int? CSMPort { get; set; }

        private const string _rootAwsSectionName = "aws";
        public RootConfig()
        {
            CSMConfig = new CSMConfig();
            Logging = new LoggingConfig();
            Proxy = new ProxyConfig();

            EndpointDefinition = AWSConfigs._endpointDefinition;
            Region = AWSConfigs._awsRegion;
            ProfileName = AWSConfigs._awsProfileName;
            ProfilesLocation = AWSConfigs._awsAccountsLocation;
            UseSdkCache = AWSConfigs._useSdkCache;
            CorrectForClockSkew = true;

#if !NETSTANDARD
            var root = AWSConfigs.GetSection<AWSSection>(_rootAwsSectionName);

            Logging.Configure(root.Logging);
            Proxy.Configure(root.Proxy);
            CSMConfig.Configure(root.CSMConfig);
            ServiceSections = root.ServiceSections;
            if (root.UseSdkCache.HasValue)
                UseSdkCache = root.UseSdkCache.Value;

            EndpointDefinition = Choose(EndpointDefinition, root.EndpointDefinition);
            Region = Choose(Region, root.Region);
            ProfileName = Choose(ProfileName, root.ProfileName);
            ProfilesLocation = Choose(ProfilesLocation, root.ProfilesLocation);
            ApplicationName = Choose(ApplicationName, root.ApplicationName);
            if (root.CorrectForClockSkew.HasValue)
                CorrectForClockSkew = root.CorrectForClockSkew.Value;
#endif
        }

        // If a is not null-or-empty, returns a; otherwise, returns b.
        private static string Choose(string a, string b)
        {
            return (string.IsNullOrEmpty(a) ? b : a);
        }

        IDictionary<string, XElement> ServiceSections { get; set; }
        public XElement GetServiceSection(string service)
        {
            XElement section;
            if (ServiceSections.TryGetValue(service, out section))
                return section;

            return null;
        }
    }

}
