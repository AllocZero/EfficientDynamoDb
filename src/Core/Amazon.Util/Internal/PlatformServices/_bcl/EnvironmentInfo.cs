using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Win32;

namespace Amazon.Util.Internal.PlatformServices
{
    public class EnvironmentInfo : IEnvironmentInfo
    {
        public EnvironmentInfo()
        {
            this.Platform = Environment.OSVersion.Platform.ToString();
            this.PlatformUserAgent = Environment.OSVersion.VersionString.Replace(" ", "_");
            this.FrameworkUserAgent =
                string.Format(CultureInfo.InvariantCulture,
                ".NET_Runtime/{0}.{1} .NET_Framework/{2}",
                 Environment.Version.Major,
                 Environment.Version.MajorRevision,
                 InternalSDKUtils.DetermineFramework());
        }

        public string Platform { get; }

        public string PlatformUserAgent { get; }

        public string FrameworkUserAgent { get; }

    }
}
