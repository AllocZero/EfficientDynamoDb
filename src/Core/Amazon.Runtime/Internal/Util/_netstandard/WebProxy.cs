using System;
using System.Net;

namespace Amazon.Runtime.Internal.Util
{
    
    /// <summary>
    /// Custom WebProxy implementation that creates a webproxy based on 
    /// user inputs(Host name and port number)
    /// </summary>
    public class WebProxy : IWebProxy
    {

        public WebProxy(string proxyUri)
        : this(new Uri(proxyUri))
        {
        }

        /// <summary>
        /// Set the ProxyUri
        /// </summary>
        public WebProxy(Uri proxyUri)
        {
            this.ProxyUri =  proxyUri;
        }

        /// <summary>
        /// Create a Uri based on the user inputs(Host name and port number)
        /// </summary>
        public WebProxy(string proxyHost, int proxyPort): this(new Uri("http://"+ proxyHost + ":" + proxyPort))
        {
        }

        /// <summary>
        /// Proxy Uri property
        /// </summary>
        public Uri ProxyUri { get; set; }

        /// <summary>
        /// Proxy Credentials property
        /// </summary>
        public ICredentials Credentials { get; set; }

        /// <summary>
        /// Getter to fetch the set proxy
        /// </summary>
        public Uri GetProxy(Uri destination)
        {
            return this.ProxyUri;
        }

        /// <summary>
        /// Method to determine if the proxy should be bypassed when accessing an internet source
        /// </summary>
        public bool IsBypassed(Uri host)
        {
            return false; /* Proxy all requests */
        }


    }
}