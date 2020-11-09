using Amazon.Runtime.Internal.Transform;

namespace AWSSDK.Core.NetStandard.Amazon.Runtime.Pipeline.HttpHandler
{
    public static class HttpHandlerConfig
    {
        public static IWebResponseData CachedResponse { get; set; }
        
        private static bool _isCacheEnabled;

        public static bool IsCacheEnabled
        {
            get => _isCacheEnabled;
            set
            {
                _isCacheEnabled = value;
                if (!value)
                    CachedResponse = null;
            }
        }
    }
}