
using System.Globalization;

namespace Amazon.Util.Internal.PlatformServices
{
    public class EnvironmentInfo : IEnvironmentInfo 
    {
        public EnvironmentInfo()
        {
            this.Platform = InternalSDKUtils.DetermineOS();
            this.PlatformUserAgent = InternalSDKUtils.PlatformUserAgent();
            this.FrameworkUserAgent = InternalSDKUtils.DetermineFramework();
        }

        public string Platform { get; }

        public string PlatformUserAgent { get; }

        public string FrameworkUserAgent { get; }
    }
}
