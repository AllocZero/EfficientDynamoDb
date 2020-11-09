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

using Amazon.Runtime.Internal.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
#if AWS_ASYNC_API
using System.Threading.Tasks;
#endif
using ThirdParty.Json.LitJson;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// Utility class for CSM.
    /// Contains logic to serialize CSM events to Json.
    /// </summary>
    public static class CSMUtilities
    {
        private const string requestKey = "Request";
#if AWS_ASYNC_API
        public static Task SerializetoJsonAndPostOverUDPAsync(MonitoringAPICall monitoringAPICall)
        {
            var monitoringAPICallAttempt = monitoringAPICall as MonitoringAPICallAttempt;
            if (monitoringAPICallAttempt != null)
            {
                if (CreateUDPMessage(monitoringAPICallAttempt, out string response))
                {
                    return MonitoringListener.Instance.PostMessagesOverUDPAsync(response);
                }
            }
            else
            {
                if (CreateUDPMessage((MonitoringAPICallEvent)monitoringAPICall, out string response))
                {
                    return MonitoringListener.Instance.PostMessagesOverUDPAsync(response);
                }
            }
            return Task.FromResult(0);
        }
#else
        public static void BeginSerializetoJsonAndPostOverUDP(MonitoringAPICall monitoringAPICall)
        {
            string response = string.Empty;
            var monitoringAPICallAttempt = monitoringAPICall as MonitoringAPICallAttempt;
            if (monitoringAPICallAttempt != null)
            {
                if (CreateUDPMessage(monitoringAPICallAttempt, out response))
                {
                    MonitoringListener.Instance.BeginPostMessagesOverUDPInvoke(response);
                }
            }
            else
            {
                if (CreateUDPMessage((MonitoringAPICallEvent)monitoringAPICall, out response))
                {
                    MonitoringListener.Instance.BeginPostMessagesOverUDPInvoke(response);
                }
            }
        }
#endif
        public static void SerializetoJsonAndPostOverUDP(MonitoringAPICall monitoringAPICall)
        {
            string response = string.Empty;
            var monitoringAPICallAttempt = monitoringAPICall as MonitoringAPICallAttempt;
            if (monitoringAPICallAttempt != null)
            {
                if (CreateUDPMessage(monitoringAPICallAttempt, out response))
                {
                    MonitoringListener.Instance.PostMessagesOverUDP(response);
                }
            }
            else
            {
                if (CreateUDPMessage((MonitoringAPICallEvent)monitoringAPICall, out response))
                {
                    MonitoringListener.Instance.PostMessagesOverUDP(response);
                }
            }
        }

        /// <summary>
        /// Method to retrieve the API name from the request name.
        /// </summary>
        public static string GetApiNameFromRequest(string requestName, IDictionary<string, string> serviceApiNameMapping, string serviceName)
        {
            var logger = Logger.GetLogger(typeof(CSMUtilities));
            // Verify that the requestName has the requestKey(Request) as a suffix.
            if (requestName.EndsWith(requestKey, StringComparison.Ordinal))
            {
                // The API name is extracted from the requestName like so -
                // PutObjectRequest is converted PutObject by removing the
                // 'Request' keyword.
                var apiName = requestName.Substring(0, requestName.Length - requestKey.Length);
                if (serviceApiNameMapping.Count > 0)
                {
                    string actualApiName;
                    if(serviceApiNameMapping.TryGetValue(apiName, out actualApiName))
                    {
                        apiName = actualApiName;
                    }
                }
                return apiName;
            }
            else
            {
                logger.InfoFormat(string.Format(CultureInfo.InvariantCulture, "Invalid request name: Request {0} does not end with the keyword '{1}'. Investigate possible generator bug for service:{2} and operation name:{3}", requestName, requestKey, serviceName, requestName));
                return string.Empty;
            }
        }

        /// <summary>
        /// Method to serialize MonitoringAPICallAttempt CSM event to json.
        /// </summary>
        private static bool CreateUDPMessage(MonitoringAPICallAttempt monitoringAPICallAttempt, out string response)
        {
            JsonWriter jw = new JsonWriter();

            jw.WriteObjectStart();

            jw = CreateUDPMessage(monitoringAPICallAttempt, jw);

            if (!string.IsNullOrEmpty(monitoringAPICallAttempt.AccessKey))
            {
                jw.WritePropertyName("AccessKey");
                jw.Write(monitoringAPICallAttempt.AccessKey);
            }

            if (!string.IsNullOrEmpty(monitoringAPICallAttempt.AWSException))
            {
                jw.WritePropertyName("AWSException");
                jw.Write(monitoringAPICallAttempt.AWSException);
            }

            if (!string.IsNullOrEmpty(monitoringAPICallAttempt.Fqdn))
            {
                jw.WritePropertyName("Fqdn");
                jw.Write(monitoringAPICallAttempt.Fqdn);
            }

            if (!string.IsNullOrEmpty(monitoringAPICallAttempt.SdkException))
            {
                jw.WritePropertyName("SdkException");
                jw.Write(monitoringAPICallAttempt.SdkException);
            }

            if (!string.IsNullOrEmpty(monitoringAPICallAttempt.AWSExceptionMessage))
            {
                jw.WritePropertyName("AWSExceptionMessage");
                jw.Write(monitoringAPICallAttempt.AWSExceptionMessage);
            }

            if (!string.IsNullOrEmpty(monitoringAPICallAttempt.SdkExceptionMessage))
            {
                jw.WritePropertyName("SdkExceptionMessage");
                jw.Write(monitoringAPICallAttempt.SdkExceptionMessage);
            }

            if (!string.IsNullOrEmpty(monitoringAPICallAttempt.SessionToken))
            {
                jw.WritePropertyName("SessionToken");
                jw.Write(monitoringAPICallAttempt.SessionToken);
            }

            if (!string.IsNullOrEmpty(monitoringAPICallAttempt.XAmzId2))
            {
                jw.WritePropertyName("XAmzId2");
                jw.Write(monitoringAPICallAttempt.XAmzId2);
            }

            if (!string.IsNullOrEmpty(monitoringAPICallAttempt.XAmznRequestId))
            {
                jw.WritePropertyName("XAmznRequestId");
                jw.Write(monitoringAPICallAttempt.XAmznRequestId);
            }

            if (!string.IsNullOrEmpty(monitoringAPICallAttempt.XAmzRequestId))
            {
                jw.WritePropertyName("XAmzRequestId");
                jw.Write(monitoringAPICallAttempt.XAmzRequestId);
            }

            if (monitoringAPICallAttempt.HttpStatusCode != null)
            {
                jw.WritePropertyName("HttpStatusCode");
                jw.Write((int)monitoringAPICallAttempt.HttpStatusCode);
            }

            jw.WritePropertyName("AttemptLatency");
            jw.Write(monitoringAPICallAttempt.AttemptLatency);

            jw.WriteObjectEnd();
            response = jw.ToString();
            
            return ASCIIEncoding.Unicode.GetByteCount(response) <= (8 * 1024);
        }

        /// <summary>
        /// Method to serialize MonitoringAPICallEvent CSM event to json.
        /// </summary>
        private static bool CreateUDPMessage(MonitoringAPICallEvent monitoringAPICallEvent, out string response)
        {
            JsonWriter jw = new JsonWriter();

            jw.WriteObjectStart();

            jw = CreateUDPMessage(monitoringAPICallEvent, jw);

            jw.WritePropertyName("Latency");
            jw.Write(monitoringAPICallEvent.Latency);

            jw.WritePropertyName("AttemptCount");
            jw.Write(monitoringAPICallEvent.AttemptCount);

            jw.WritePropertyName("MaxRetriesExceeded");
            int maxRetriesValue = 0;
            if (monitoringAPICallEvent.IsLastExceptionRetryable)
            {
                maxRetriesValue = 1;
            }
            jw.Write(maxRetriesValue);

            if (monitoringAPICallEvent.FinalHttpStatusCode != null)
            {
                jw.WritePropertyName("FinalHttpStatusCode");
                jw.Write((int)monitoringAPICallEvent.FinalHttpStatusCode);
            }

            if (!string.IsNullOrEmpty(monitoringAPICallEvent.FinalAWSException))
            {
                jw.WritePropertyName("FinalAWSException");
                jw.Write(monitoringAPICallEvent.FinalAWSException);
            }

            if (!string.IsNullOrEmpty(monitoringAPICallEvent.FinalSdkException))
            {
                jw.WritePropertyName("FinalSdkException");
                jw.Write(monitoringAPICallEvent.FinalSdkException);
            }

            if (!string.IsNullOrEmpty(monitoringAPICallEvent.FinalAWSExceptionMessage))
            {
                jw.WritePropertyName("FinalAWSExceptionMessage");
                jw.Write(monitoringAPICallEvent.FinalAWSExceptionMessage);
            }

            if (!string.IsNullOrEmpty(monitoringAPICallEvent.FinalSdkExceptionMessage))
            {
                jw.WritePropertyName("FinalSdkExceptionMessage");
                jw.Write(monitoringAPICallEvent.FinalSdkExceptionMessage);
            }

            jw.WriteObjectEnd();
            response = jw.ToString();

            return ASCIIEncoding.Unicode.GetByteCount(response) <= (8 * 1024);
        }

        /// <summary>
        /// Method to serialize MonitoringAPICall CSM event to json.
        /// </summary>
        private static JsonWriter CreateUDPMessage(MonitoringAPICall monitoringAPICall, JsonWriter jw)
        {
            string response = string.Empty;

            if (!string.IsNullOrEmpty(monitoringAPICall.Api))
            {
                jw.WritePropertyName("Api");
                jw.Write(monitoringAPICall.Api);
            }

            if (!string.IsNullOrEmpty(monitoringAPICall.Service))
            {
                jw.WritePropertyName("Service");
                jw.Write(monitoringAPICall.Service);
            }

            if (!string.IsNullOrEmpty(monitoringAPICall.ClientId))
            {
                jw.WritePropertyName("ClientId");
                jw.Write(monitoringAPICall.ClientId);
            }

            jw.WritePropertyName("Version");
            jw.Write(monitoringAPICall.Version);


            if (!string.IsNullOrEmpty(monitoringAPICall.Type))
            {
                jw.WritePropertyName("Type");
                jw.Write(monitoringAPICall.Type);
            }

            jw.WritePropertyName("Timestamp");
            jw.Write(monitoringAPICall.Timestamp);

            if (!string.IsNullOrEmpty(monitoringAPICall.Region))
            {
                jw.WritePropertyName("Region");
                jw.Write(monitoringAPICall.Region);
            }

            if (!string.IsNullOrEmpty(monitoringAPICall.UserAgent))
            {
                jw.WritePropertyName("UserAgent");
                jw.Write(monitoringAPICall.UserAgent);
            }

            return jw;
        }
    }
}
