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
    /// UpdateGlobalTableSettings Request Marshaller
    /// </summary>       
    public class UpdateGlobalTableSettingsRequestMarshaller : IMarshaller<IRequest, UpdateGlobalTableSettingsRequest> , IMarshaller<IRequest,AmazonWebServiceRequest>
    {
        /// <summary>
        /// Marshaller the request object to the HTTP request.
        /// </summary>  
        /// <param name="input"></param>
        /// <returns></returns>
        public IRequest Marshall(AmazonWebServiceRequest input)
        {
            return this.Marshall((UpdateGlobalTableSettingsRequest)input);
        }

        /// <summary>
        /// Marshaller the request object to the HTTP request.
        /// </summary>  
        /// <param name="publicRequest"></param>
        /// <returns></returns>
        public IRequest Marshall(UpdateGlobalTableSettingsRequest publicRequest)
        {
            IRequest request = new DefaultRequest(publicRequest, "Amazon.DynamoDBv2");
            string target = "DynamoDB_20120810.UpdateGlobalTableSettings";
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
                if(publicRequest.IsSetGlobalTableBillingMode())
                {
                    context.Writer.WritePropertyName("GlobalTableBillingMode");
                    context.Writer.Write(publicRequest.GlobalTableBillingMode);
                }

                if(publicRequest.IsSetGlobalTableGlobalSecondaryIndexSettingsUpdate())
                {
                    context.Writer.WritePropertyName("GlobalTableGlobalSecondaryIndexSettingsUpdate");
                    context.Writer.WriteArrayStart();
                    foreach(var publicRequestGlobalTableGlobalSecondaryIndexSettingsUpdateListValue in publicRequest.GlobalTableGlobalSecondaryIndexSettingsUpdate)
                    {
                        context.Writer.WriteObjectStart();

                        var marshaller = GlobalTableGlobalSecondaryIndexSettingsUpdateMarshaller.Instance;
                        marshaller.Marshall(publicRequestGlobalTableGlobalSecondaryIndexSettingsUpdateListValue, context);

                        context.Writer.WriteObjectEnd();
                    }
                    context.Writer.WriteArrayEnd();
                }

                if(publicRequest.IsSetGlobalTableName())
                {
                    context.Writer.WritePropertyName("GlobalTableName");
                    context.Writer.Write(publicRequest.GlobalTableName);
                }

                if(publicRequest.IsSetGlobalTableProvisionedWriteCapacityAutoScalingSettingsUpdate())
                {
                    context.Writer.WritePropertyName("GlobalTableProvisionedWriteCapacityAutoScalingSettingsUpdate");
                    context.Writer.WriteObjectStart();

                    var marshaller = AutoScalingSettingsUpdateMarshaller.Instance;
                    marshaller.Marshall(publicRequest.GlobalTableProvisionedWriteCapacityAutoScalingSettingsUpdate, context);

                    context.Writer.WriteObjectEnd();
                }

                if(publicRequest.IsSetGlobalTableProvisionedWriteCapacityUnits())
                {
                    context.Writer.WritePropertyName("GlobalTableProvisionedWriteCapacityUnits");
                    context.Writer.Write(publicRequest.GlobalTableProvisionedWriteCapacityUnits);
                }

                if(publicRequest.IsSetReplicaSettingsUpdate())
                {
                    context.Writer.WritePropertyName("ReplicaSettingsUpdate");
                    context.Writer.WriteArrayStart();
                    foreach(var publicRequestReplicaSettingsUpdateListValue in publicRequest.ReplicaSettingsUpdate)
                    {
                        context.Writer.WriteObjectStart();

                        var marshaller = ReplicaSettingsUpdateMarshaller.Instance;
                        marshaller.Marshall(publicRequestReplicaSettingsUpdateListValue, context);

                        context.Writer.WriteObjectEnd();
                    }
                    context.Writer.WriteArrayEnd();
                }

        
                writer.WriteObjectEnd();
                string snippet = stringWriter.ToString();
                request.Content = System.Text.Encoding.UTF8.GetBytes(snippet);
            }


            return request;
        }
        private static UpdateGlobalTableSettingsRequestMarshaller _instance = new UpdateGlobalTableSettingsRequestMarshaller();        

        internal static UpdateGlobalTableSettingsRequestMarshaller GetInstance()
        {
            return _instance;
        }

        /// <summary>
        /// Gets the singleton.
        /// </summary>  
        public static UpdateGlobalTableSettingsRequestMarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

    }
}