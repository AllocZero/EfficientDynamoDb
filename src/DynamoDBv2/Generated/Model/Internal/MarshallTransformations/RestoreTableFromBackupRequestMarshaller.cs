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
    /// RestoreTableFromBackup Request Marshaller
    /// </summary>       
    public class RestoreTableFromBackupRequestMarshaller : IMarshaller<IRequest, RestoreTableFromBackupRequest> , IMarshaller<IRequest,AmazonWebServiceRequest>
    {
        /// <summary>
        /// Marshaller the request object to the HTTP request.
        /// </summary>  
        /// <param name="input"></param>
        /// <returns></returns>
        public IRequest Marshall(AmazonWebServiceRequest input)
        {
            return this.Marshall((RestoreTableFromBackupRequest)input);
        }

        /// <summary>
        /// Marshaller the request object to the HTTP request.
        /// </summary>  
        /// <param name="publicRequest"></param>
        /// <returns></returns>
        public IRequest Marshall(RestoreTableFromBackupRequest publicRequest)
        {
            IRequest request = new DefaultRequest(publicRequest, "Amazon.DynamoDBv2");
            string target = "DynamoDB_20120810.RestoreTableFromBackup";
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
                if(publicRequest.IsSetBackupArn())
                {
                    context.Writer.WritePropertyName("BackupArn");
                    context.Writer.Write(publicRequest.BackupArn);
                }

                if(publicRequest.IsSetBillingModeOverride())
                {
                    context.Writer.WritePropertyName("BillingModeOverride");
                    context.Writer.Write(publicRequest.BillingModeOverride);
                }

                if(publicRequest.IsSetGlobalSecondaryIndexOverride())
                {
                    context.Writer.WritePropertyName("GlobalSecondaryIndexOverride");
                    context.Writer.WriteArrayStart();
                    foreach(var publicRequestGlobalSecondaryIndexOverrideListValue in publicRequest.GlobalSecondaryIndexOverride)
                    {
                        context.Writer.WriteObjectStart();

                        var marshaller = GlobalSecondaryIndexMarshaller.Instance;
                        marshaller.Marshall(publicRequestGlobalSecondaryIndexOverrideListValue, context);

                        context.Writer.WriteObjectEnd();
                    }
                    context.Writer.WriteArrayEnd();
                }

                if(publicRequest.IsSetLocalSecondaryIndexOverride())
                {
                    context.Writer.WritePropertyName("LocalSecondaryIndexOverride");
                    context.Writer.WriteArrayStart();
                    foreach(var publicRequestLocalSecondaryIndexOverrideListValue in publicRequest.LocalSecondaryIndexOverride)
                    {
                        context.Writer.WriteObjectStart();

                        var marshaller = LocalSecondaryIndexMarshaller.Instance;
                        marshaller.Marshall(publicRequestLocalSecondaryIndexOverrideListValue, context);

                        context.Writer.WriteObjectEnd();
                    }
                    context.Writer.WriteArrayEnd();
                }

                if(publicRequest.IsSetProvisionedThroughputOverride())
                {
                    context.Writer.WritePropertyName("ProvisionedThroughputOverride");
                    context.Writer.WriteObjectStart();

                    var marshaller = ProvisionedThroughputMarshaller.Instance;
                    marshaller.Marshall(publicRequest.ProvisionedThroughputOverride, context);

                    context.Writer.WriteObjectEnd();
                }

                if(publicRequest.IsSetSSESpecificationOverride())
                {
                    context.Writer.WritePropertyName("SSESpecificationOverride");
                    context.Writer.WriteObjectStart();

                    var marshaller = SSESpecificationMarshaller.Instance;
                    marshaller.Marshall(publicRequest.SSESpecificationOverride, context);

                    context.Writer.WriteObjectEnd();
                }

                if(publicRequest.IsSetTargetTableName())
                {
                    context.Writer.WritePropertyName("TargetTableName");
                    context.Writer.Write(publicRequest.TargetTableName);
                }

        
                writer.WriteObjectEnd();
                string snippet = stringWriter.ToString();
                request.Content = System.Text.Encoding.UTF8.GetBytes(snippet);
            }


            return request;
        }
        private static RestoreTableFromBackupRequestMarshaller _instance = new RestoreTableFromBackupRequestMarshaller();        

        internal static RestoreTableFromBackupRequestMarshaller GetInstance()
        {
            return _instance;
        }

        /// <summary>
        /// Gets the singleton.
        /// </summary>  
        public static RestoreTableFromBackupRequestMarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

    }
}