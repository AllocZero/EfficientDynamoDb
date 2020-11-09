// /*******************************************************************************
//  *  Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
//  *  Licensed under the Apache License, Version 2.0 (the "License"). You may not use
//  *  this file except in compliance with the License. A copy of the License is located at
//  *
//  *  http://aws.amazon.com/apache2.0
//  *
//  *  or in the "license" file accompanying this file.
//  *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
//  *  CONDITIONS OF ANY KIND, either express or implied. See the License for the
//  *  specific language governing permissions and limitations under the License.
//  * *****************************************************************************
//  *    __  _    _  ___
//  *   (  )( \/\/ )/ __)
//  *   /__\ \    / \__ \
//  *  (_)(_) \/\/  (___/
//  *
//  *  AWS SDK for .NET
//  *
//  */

using System;
using System.Net;

namespace Amazon.Runtime.Internal
{
    public interface IAmazonSecurityProtocolManager
    {
        bool IsSecurityProtocolSystemDefault();
        void UpdateProtocolsToSupported();
    }

    public class AmazonSecurityProtocolManager : IAmazonSecurityProtocolManager
    {
        private const SecurityProtocolType Tls11 = (SecurityProtocolType)0x00000300; // 0x00000300 adds support for TLS 1.1
        private const SecurityProtocolType Tls12 = (SecurityProtocolType)0x00000C00; // 0x00000C00 adds support for TLS 1.2
        private const SecurityProtocolType SupportedTls = Tls11 | Tls12;
        private const SecurityProtocolType SystemDefault = 0x00000000; // 0x00000000 SystemDefault was introduced in .NET 4.7

        public bool IsSecurityProtocolSystemDefault()
        {
            return ServicePointManager.SecurityProtocol == SystemDefault;
        }

        public void UpdateProtocolsToSupported()
        {
            var existingSecurityProtocol = ServicePointManager.SecurityProtocol;
            try
            {
                ServicePointManager.SecurityProtocol |= SupportedTls;
            }
            catch (NotSupportedException ex)
            {
                ServicePointManager.SecurityProtocol = existingSecurityProtocol;
                throw new NotSupportedException(
                    "TLS version 1.1 or 1.2 are not supported on this system. Some AWS services will refuse traffic." +
                    " Please consider updating to a system that supports newer security protocols.", ex);
            }
        }
    }
}