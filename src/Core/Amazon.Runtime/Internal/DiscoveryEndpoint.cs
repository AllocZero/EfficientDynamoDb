/*
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 * 
 *  http://aws.amazon.com/apache2.0
 * 
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */

using System;
using System.Diagnostics.CodeAnalysis;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// Class containing the data for one endpoint returned from a endpoint discovery request
    /// <para>
    /// This class is only intended for internal use inside the AWS client libraries.
    /// Callers shouldn't ever interact directly with objects of this class.
    /// </para>
    /// </summary>    
    public abstract class DiscoveryEndpointBase
    {
        private DateTime _createdOn;
        private string _address;
        private long _cachePeriodInMinutes;
        private object objectExtendLock = new object();

        /// <summary>
        /// Constructs a new DiscoveryEndpoint
        /// </summary>
        /// <param name="address">The address of the endpoint</param>        
        /// <param name="cachePeriodInMinutes">The cache period for the endpoint in minutes</param>
        [SuppressMessage("AwsSdkRules", "CR1003:PreventDateTimeNowUseRule",
            Justification = "The DateTime value is never used on the server.")]
        protected DiscoveryEndpointBase(string address, long cachePeriodInMinutes)
        {
            Address = address;
            CachePeriodInMinutes = cachePeriodInMinutes;
            _createdOn = DateTime.UtcNow;
        }

        /// <summary>
        /// The address of the endpoint.
        /// </summary>
        public string Address
        {
            get { return _address; }
            protected set
            {
                var address = value;

                // A null endpoint is allowed when endpoint discovery is not required.
                if (address != null)
                {
                    // Only http schemes are allowed, and we assume that if it does not start with an http scheme,
                    // it should be defaulted to https.
                    if (!address.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                        !address.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    {
                        address = "https://" + address;
                    }
                }

                _address = address;
            }
        }

        /// <summary>
        /// The cache period for the endpoint in minutes
        /// </summary>
        public long CachePeriodInMinutes
        {
            get { return _cachePeriodInMinutes; }
            protected set { _cachePeriodInMinutes = value; }
        }

        /// <summary>
        /// Calculates if this endpoint has expired
        /// </summary>
        /// <returns>A boolean value indicating if the cache period has expired</returns>
        [SuppressMessage("AwsSdkRules", "CR1003:PreventDateTimeNowUseRule",
            Justification = "The DateTime value is never used on the server.")]
        public bool HasExpired()
        {
            TimeSpan timespan = DateTime.UtcNow - _createdOn;
            return timespan.TotalMinutes > CachePeriodInMinutes ? true : false;
        }

        /// <summary>
        /// Extends the endpoint expiration by the specified number of minutes from now.
        /// </summary>        
        [SuppressMessage("AwsSdkRules", "CR1003:PreventDateTimeNowUseRule",
            Justification = "The DateTime value is never used on the server.")]
        public void ExtendExpiration(long minutes)
        {
            //Lock for this instance of the object against multiple extends
            lock (objectExtendLock)
            {
                CachePeriodInMinutes = minutes;
                _createdOn = DateTime.UtcNow;
            }
        }
    }

    /// <summary>
    /// Class containing the data for one endpoint returned from a endpoint discovery request
    /// <para>
    /// This class is only intended for internal use inside the AWS client libraries.
    /// Callers shouldn't ever interact directly with objects of this class.
    /// </para>
    /// </summary>    
    public class DiscoveryEndpoint : DiscoveryEndpointBase
    {
        public DiscoveryEndpoint(string address, long cachePeriodInMinutes) : base(address, cachePeriodInMinutes)
        {
        }
    }
}