using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amazon.Util.Internal.PlatformServices
{
    public enum NetworkStatus
    {
        NotReachable,
        ReachableViaCarrierDataNetwork,
        ReachableViaWiFiNetwork
    }

    public class NetworkStatusEventArgs : EventArgs
    {
        public NetworkStatus Status { get; private set; }

        public NetworkStatusEventArgs(NetworkStatus status)
        {
            this.Status = status;
        }
    }

    public interface INetworkReachability
    {
        NetworkStatus NetworkStatus { get; }

        event EventHandler<NetworkStatusEventArgs> NetworkReachabilityChanged;
    }
}
