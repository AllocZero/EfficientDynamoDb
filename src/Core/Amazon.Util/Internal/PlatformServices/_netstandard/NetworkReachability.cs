using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics.CodeAnalysis;

namespace Amazon.Util.Internal.PlatformServices
{
    public class NetworkReachability : INetworkReachability
    {
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public NetworkStatus NetworkStatus
        {
            get { throw new NotImplementedException(ServiceFactory.NotImplementedErrorMessage); }
        }

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public event EventHandler<NetworkStatusEventArgs> NetworkReachabilityChanged
        {
            add
            {
                throw new NotImplementedException(ServiceFactory.NotImplementedErrorMessage);
            }
            remove
            {
                throw new NotImplementedException(ServiceFactory.NotImplementedErrorMessage);
            }
        }
    }
}
