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

/*
 * Do not modify this file. This file is generated from the dynamodb-2012-08-10.normal.json service model.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;
using Amazon.Runtime.Internal.Util;
using ThirdParty.Json.LitJson;

namespace Amazon.DynamoDBv2.Model.Internal.MarshallTransformations
{
    /// <summary>
    /// UpdateContributorInsights Request Marshaller
    /// </summary>       
    public class UpdateContributorInsightsRequestMarshaller : IMarshaller<IRequest, UpdateContributorInsightsRequest> , IMarshaller<IRequest,AmazonWebServiceRequest>
    {
        /// <summary>
        /// Marshaller the request object to the HTTP request.
        /// </summary>  
        /// <param name="input"></param>
        /// <returns></returns>
        public IRequest Marshall(AmazonWebServiceRequest input)
        {
            return this.Marshall((UpdateContributorInsightsRequest)input);
        }

        /// <summary>
        /// Marshaller the request object to the HTTP request.
        /// </summary>  
        /// <param name="publicRequest"></param>
        /// <returns></returns>
        public IRequest Marshall(UpdateContributorInsightsRequest publicRequest)
        {
            IRequest request = new DefaultRequest(publicRequest, "Amazon.DynamoDBv2");
            string target = "DynamoDB_20120810.UpdateContributorInsights";
            request.Headers["X-Amz-Target"] = target;
            request.Headers["Content-Type"] = "application/x-amz-json-1.0";
            request.Headers[Amazon.Util.HeaderKeys.XAmzApiVersion] = "2012-08-10";            
            request.HttpMethod = "POST";

            request.ResourcePath = "/";
            request.MarshallerVersion = 2;
            using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                JsonWriter writer = new JsonWriter(stringWriter);
                writer.WriteObjectStart();
                var context = new JsonMarshallerContext(request, writer);
                if(publicRequest.IsSetContributorInsightsAction())
                {
                    context.Writer.WritePropertyName("ContributorInsightsAction");
                    context.Writer.Write(publicRequest.ContributorInsightsAction);
                }

                if(publicRequest.IsSetIndexName())
                {
                    context.Writer.WritePropertyName("IndexName");
                    context.Writer.Write(publicRequest.IndexName);
                }

                if(publicRequest.IsSetTableName())
                {
                    context.Writer.WritePropertyName("TableName");
                    context.Writer.Write(publicRequest.TableName);
                }

        
                writer.WriteObjectEnd();
                string snippet = stringWriter.ToString();
                request.Content = System.Text.Encoding.UTF8.GetBytes(snippet);
            }


            return request;
        }
        private static UpdateContributorInsightsRequestMarshaller _instance = new UpdateContributorInsightsRequestMarshaller();        

        internal static UpdateContributorInsightsRequestMarshaller GetInstance()
        {
            return _instance;
        }

        /// <summary>
        /// Gets the singleton.
        /// </summary>  
        public static UpdateContributorInsightsRequestMarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

    }
}