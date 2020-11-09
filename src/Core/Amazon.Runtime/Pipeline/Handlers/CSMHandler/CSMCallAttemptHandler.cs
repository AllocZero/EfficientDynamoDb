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

using Amazon.Util;
using Amazon.Runtime.Internal.Util;
using System.Globalization;
using System;
using System.Text;
using System.Collections.Generic;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// The CSM handler has the logic for capturing data for CSM attempts when enabled.
    /// </summary>
    public class CSMCallAttemptHandler : PipelineHandler
    {
        /// <summary>
        /// Invokes the CSM handler and captures data for the CSM attempts.
        /// </summary>
        public override void InvokeSync(IExecutionContext executionContext)
        {
            try
            {
                PreInvoke(executionContext);
                base.InvokeSync(executionContext);
            }
            catch (AmazonServiceException e)
            {
                CaptureAmazonException(executionContext.RequestContext.CSMCallAttempt, e);
                throw;
            }
            catch (Exception e)
            {
                CaptureSDKExceptionMessage(executionContext.RequestContext.CSMCallAttempt, e);
                throw;
            }
            finally
            {
                CSMCallAttemptMetricsCapture(executionContext.RequestContext, executionContext.ResponseContext);
                CSMUtilities.SerializetoJsonAndPostOverUDP(executionContext.RequestContext.CSMCallAttempt);
            }
        }
#if AWS_ASYNC_API

        /// <summary>
        /// Calls the PreInvoke and PostInvoke methods before and after calling the next handler 
        /// in the pipeline.
        /// </summary>
        public override async System.Threading.Tasks.Task<T> InvokeAsync<T>(IExecutionContext executionContext)
        {
            try
            {
                PreInvoke(executionContext);
                var response = await base.InvokeAsync<T>(executionContext).ConfigureAwait(false);
                return response;
            }

            catch (AmazonServiceException e)
            {
                CaptureAmazonException(executionContext.RequestContext.CSMCallAttempt, e);
                throw;
            }
            catch (Exception e)
            {
                CaptureSDKExceptionMessage(executionContext.RequestContext.CSMCallAttempt, e);
                throw;
            }
            finally
            {
                CSMCallAttemptMetricsCapture(executionContext.RequestContext, executionContext.ResponseContext);
                CSMUtilities.SerializetoJsonAndPostOverUDPAsync(executionContext.RequestContext.CSMCallAttempt).ConfigureAwait(false);
            }
        }
#elif AWS_APM_API
        /// <summary>
        /// Invokes the CSM handler and captures data for the CSM attempts.
        /// </summary>
        public override IAsyncResult InvokeAsync(IAsyncExecutionContext executionContext)
        {
            PreInvoke(ExecutionContext.CreateFromAsyncContext(executionContext));
            return base.InvokeAsync(executionContext);
        }
        protected override void InvokeAsyncCallback(IAsyncExecutionContext executionContext)
        {
            var syncExecutionContext = ExecutionContext.CreateFromAsyncContext(executionContext);
            if (executionContext.ResponseContext.AsyncResult.Exception != null)
            {
                var exception = executionContext.ResponseContext.AsyncResult.Exception;
                if (exception is AmazonServiceException)
                {
                    CaptureAmazonException(syncExecutionContext.RequestContext.CSMCallAttempt, exception as AmazonServiceException);
                }
                else
                {
                    if(syncExecutionContext.ResponseContext.HttpResponse == null)
                    {
                        CaptureSDKExceptionMessage(syncExecutionContext.RequestContext.CSMCallAttempt, exception);
                    }
                }
            }
            
            CSMCallAttemptMetricsCapture(syncExecutionContext.RequestContext, syncExecutionContext.ResponseContext);
            CSMUtilities.BeginSerializetoJsonAndPostOverUDP(executionContext.RequestContext.CSMCallAttempt);
            base.InvokeAsyncCallback(executionContext);
        }

#endif
        /// <summary>
        /// Method that gets called in the final clause that captures data for each http request
        /// from the request and response context.
        /// </summary>
        protected static void CSMCallAttemptMetricsCapture(IRequestContext requestContext, IResponseContext responseContext)
        {
            requestContext.CSMCallAttempt.Service = requestContext.CSMCallEvent.Service;
            requestContext.CSMCallAttempt.Fqdn = requestContext.Request.GetHeaderValue(HeaderKeys.HostHeader);

            bool useAlternateUserAgentHeader = (requestContext.ClientConfig as ClientConfig)?.UseAlternateUserAgentHeader ?? false;
            requestContext.CSMCallAttempt.UserAgent = requestContext.Request.GetHeaderValue(useAlternateUserAgentHeader ? HeaderKeys.XAmzUserAgentHeader : HeaderKeys.UserAgentHeader);

            requestContext.CSMCallAttempt.SessionToken = requestContext.Request.GetHeaderValue(HeaderKeys.XAmzSecurityTokenHeader);

            requestContext.CSMCallAttempt.Region = requestContext.Request.DeterminedSigningRegion;

            requestContext.CSMCallAttempt.Api = CSMUtilities.
                GetApiNameFromRequest(requestContext.Request.RequestName, requestContext.ServiceMetaData.OperationNameMapping, requestContext.CSMCallAttempt.Service);

            requestContext.CSMCallAttempt.AccessKey = requestContext.ImmutableCredentials.AccessKey;

            requestContext.CSMCallAttempt.AttemptLatency = AWSSDKUtils.ConvertTimeSpanToMilliseconds(requestContext
                .Metrics.StopEvent(Metric.CSMAttemptLatency)
                .ElapsedTime);

            if (responseContext.HttpResponse != null)
            {
                if((int) responseContext.HttpResponse.StatusCode > 0)
                {
                    requestContext.CSMCallAttempt.HttpStatusCode = (int)responseContext.HttpResponse.StatusCode;
                }

                requestContext.CSMCallAttempt.XAmznRequestId = responseContext.HttpResponse.GetHeaderValue(HeaderKeys.RequestIdHeader);

                requestContext.CSMCallAttempt.XAmzRequestId = responseContext.HttpResponse.GetHeaderValue(HeaderKeys.XAmzRequestIdHeader);

                requestContext.CSMCallAttempt.XAmzId2 = responseContext.HttpResponse.GetHeaderValue(HeaderKeys.XAmzId2Header);
            }
        }

        /// <summary>
        /// Executes the OnPreInvoke action as part of pre-invoke.
        /// </summary>
        protected virtual void PreInvoke(IExecutionContext executionContext)
        {
            executionContext.RequestContext.CSMCallAttempt = new MonitoringAPICallAttempt(executionContext.RequestContext);
            executionContext.RequestContext.Metrics.StartEvent(Metric.CSMAttemptLatency);
        }

        /// <summary>
        /// Method to collect data in case of connection exception
        /// </summary>
        private static void CaptureSDKExceptionMessage(MonitoringAPICallAttempt monitoringAPICallAttempt, Exception e)
        {
            monitoringAPICallAttempt.SdkException = e.GetType().Name.ToString().Length <= 128 ? e.GetType().Name.ToString() : string.Empty;
            monitoringAPICallAttempt.SdkExceptionMessage = e.Message.Length <= 512 ? e.Message : string.Empty;
        }

        /// <summary>
        /// Method to collect data in case of Amazon service exception
        /// </summary>
        private static void CaptureAmazonException(MonitoringAPICallAttempt monitoringAPICallAttempt, AmazonServiceException e)
        {
            if ((int)e.StatusCode > 0)
            {
                monitoringAPICallAttempt.HttpStatusCode = (int)e.StatusCode;
            }
            
            if (e.ErrorCode == null)
            {
                CaptureSDKExceptionMessage(monitoringAPICallAttempt, e);
                return;
            }
            monitoringAPICallAttempt.AWSException = e.ErrorCode.Length <= 128 ? e.ErrorCode : string.Empty;
            monitoringAPICallAttempt.AWSExceptionMessage =
                e.Message.Length <= 512 ? e.Message : string.Empty;
        }
    }
}
