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
using System.Net;
using System.Net.Sockets;
using System.Text;
#if AWS_ASYNC_API
using System.Threading.Tasks;
#endif
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Util;

namespace Amazon.Runtime
{
    /// <summary>
    /// Class that posts the CSM event to a UDP portconfig
    /// This is a singleton class and is created once 
    /// per AmazonServiceClient.
    /// </summary>
    internal sealed class MonitoringListener:IDisposable
    {
        private static readonly MonitoringListener csmMonitoringListenerInstance = new MonitoringListener();
        /// <summary>
        /// The CSMevents are always posted to the localhost
        /// </summary>
        private readonly string _host;
        private readonly UdpClient _udpClient;
        private readonly Logger logger;
        private readonly int _port;
        private MonitoringListener()
        {
            _host = DeterminedCSMConfiguration.Instance.CSMConfiguration.Host;
            _port = DeterminedCSMConfiguration.Instance.CSMConfiguration.Port;
            _udpClient = new UdpClient();
            logger = Logger.GetLogger(typeof(MonitoringListener));
        }

        static MonitoringListener()
        {
            
        }
        public static MonitoringListener Instance
        {
            get
            {
                return csmMonitoringListenerInstance;
            }
        }

        /// <summary>
        /// Method to post UDP datagram for sync calls
        /// </summary>
        public void PostMessagesOverUDP(string response)
        {
#if BCL
            try
            {
                _udpClient.Send(Encoding.UTF8.GetBytes(response),
                        Encoding.UTF8.GetBytes(response).Length, _host, _port);
            }
            catch (Exception e)
            {
                // If UDP post fails, the errors is logged and is returned without rethrowing the exception
                logger.InfoFormat("Error when posting UDP datagrams. " + e.Message);
            }
#endif
        }

        /// <summary>
        /// Method to post UDP datagram for async calls
        /// </summary>
#if AWS_ASYNC_API
        public async Task PostMessagesOverUDPAsync(string response)
        {
            try
            {
                await _udpClient.SendAsync(Encoding.UTF8.GetBytes(response),
                    Encoding.UTF8.GetBytes(response).Length, _host, _port).ConfigureAwait(false);
            }
            catch(Exception e)
            {
                // If UDP post fails, the errors is logged and is returned without rethrowing the exception
                logger.InfoFormat("Error when posting UDP datagrams. " + e.Message);
            }
        }

        /// <summary>
        /// Method to post UDP datagram for bcl35 async calls
        /// </summary>
#else
        public void BeginPostMessagesOverUDPInvoke(string response)
        {
            try
            {
                _udpClient.BeginSend(Encoding.UTF8.GetBytes(response),
                        Encoding.UTF8.GetBytes(response).Length, _host, _port, new AsyncCallback(EndSendMessagesOverUDPInvoke), _udpClient);
            }
            catch (Exception e)
            {
                // If UDP post fails, the errors is logged and is returned without rethrowing the exception
                logger.InfoFormat("Error when posting UDP datagrams. " + e.Message);
            }
        }

        private void EndSendMessagesOverUDPInvoke(IAsyncResult ar)
        {
            try
            {
                var udpClient = (UdpClient)ar.AsyncState;
                if (!ar.IsCompleted)
                {
                    ar.AsyncWaitHandle.WaitOne();
                }
                udpClient.EndSend(ar);
            }
            catch (Exception e)
            {
                // If UDP post fails, the errors is logged and is returned without rethrowing the exception
                logger.InfoFormat("Error when posting UDP datagrams. " + e.Message);
            }
        }
#endif
        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
#if NETSTANDARD
                _udpClient.Dispose();
#else
                _udpClient.Close();
#endif
            }

            _disposed = true;
        }
    }
}
