using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics.CodeAnalysis;

namespace Amazon.Util.Internal.PlatformServices
{
    public class ApplicationInfo : IApplicationInfo
    {
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public string AppTitle
        {
            get
            {
                throw new NotImplementedException(ServiceFactory.NotImplementedErrorMessage);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public string AppVersionName
        {
            get
            {
                throw new NotImplementedException(ServiceFactory.NotImplementedErrorMessage);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public string AppVersionCode
        {
            get
            {
                throw new NotImplementedException(ServiceFactory.NotImplementedErrorMessage);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public string PackageName
        {
            get
            {
                throw new NotImplementedException(ServiceFactory.NotImplementedErrorMessage);
            }
        }
    }
}