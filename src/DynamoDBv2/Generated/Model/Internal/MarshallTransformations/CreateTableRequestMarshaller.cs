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
    /// CreateTable Request Marshaller
    /// </summary>       
    public class CreateTableRequestMarshaller : IMarshaller<IRequest, CreateTableRequest> , IMarshaller<IRequest,AmazonWebServiceRequest>
    {
        /// <summary>
        /// Marshaller the request object to the HTTP request.
        /// </summary>  
        /// <param name="input"></param>
        /// <returns></returns>
        public IRequest Marshall(AmazonWebServiceRequest input)
        {
            return this.Marshall((CreateTableRequest)input);
        }

        /// <summary>
        /// Marshaller the request object to the HTTP request.
        /// </summary>  
        /// <param name="publicRequest"></param>
        /// <returns></returns>
        public IRequest Marshall(CreateTableRequest publicRequest)
        {
            IRequest request = new DefaultRequest(publicRequest, "Amazon.DynamoDBv2");
            string target = "DynamoDB_20120810.CreateTable";
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
                if(publicRequest.IsSetAttributeDefinitions())
                {
                    context.Writer.WritePropertyName("AttributeDefinitions");
                    context.Writer.WriteArrayStart();
                    foreach(var publicRequestAttributeDefinitionsListValue in publicRequest.AttributeDefinitions)
                    {
                        context.Writer.WriteObjectStart();

                        var marshaller = AttributeDefinitionMarshaller.Instance;
                        marshaller.Marshall(publicRequestAttributeDefinitionsListValue, context);

                        context.Writer.WriteObjectEnd();
                    }
                    context.Writer.WriteArrayEnd();
                }

                if(publicRequest.IsSetBillingMode())
                {
                    context.Writer.WritePropertyName("BillingMode");
                    context.Writer.Write(publicRequest.BillingMode);
                }

                if(publicRequest.IsSetGlobalSecondaryIndexes())
                {
                    context.Writer.WritePropertyName("GlobalSecondaryIndexes");
                    context.Writer.WriteArrayStart();
                    foreach(var publicRequestGlobalSecondaryIndexesListValue in publicRequest.GlobalSecondaryIndexes)
                    {
                        context.Writer.WriteObjectStart();

                        var marshaller = GlobalSecondaryIndexMarshaller.Instance;
                        marshaller.Marshall(publicRequestGlobalSecondaryIndexesListValue, context);

                        context.Writer.WriteObjectEnd();
                    }
                    context.Writer.WriteArrayEnd();
                }

                if(publicRequest.IsSetKeySchema())
                {
                    context.Writer.WritePropertyName("KeySchema");
                    context.Writer.WriteArrayStart();
                    foreach(var publicRequestKeySchemaListValue in publicRequest.KeySchema)
                    {
                        context.Writer.WriteObjectStart();

                        var marshaller = KeySchemaElementMarshaller.Instance;
                        marshaller.Marshall(publicRequestKeySchemaListValue, context);

                        context.Writer.WriteObjectEnd();
                    }
                    context.Writer.WriteArrayEnd();
                }

                if(publicRequest.IsSetLocalSecondaryIndexes())
                {
                    context.Writer.WritePropertyName("LocalSecondaryIndexes");
                    context.Writer.WriteArrayStart();
                    foreach(var publicRequestLocalSecondaryIndexesListValue in publicRequest.LocalSecondaryIndexes)
                    {
                        context.Writer.WriteObjectStart();

                        var marshaller = LocalSecondaryIndexMarshaller.Instance;
                        marshaller.Marshall(publicRequestLocalSecondaryIndexesListValue, context);

                        context.Writer.WriteObjectEnd();
                    }
                    context.Writer.WriteArrayEnd();
                }

                if(publicRequest.IsSetProvisionedThroughput())
                {
                    context.Writer.WritePropertyName("ProvisionedThroughput");
                    context.Writer.WriteObjectStart();

                    var marshaller = ProvisionedThroughputMarshaller.Instance;
                    marshaller.Marshall(publicRequest.ProvisionedThroughput, context);

                    context.Writer.WriteObjectEnd();
                }

                if(publicRequest.IsSetSSESpecification())
                {
                    context.Writer.WritePropertyName("SSESpecification");
                    context.Writer.WriteObjectStart();

                    var marshaller = SSESpecificationMarshaller.Instance;
                    marshaller.Marshall(publicRequest.SSESpecification, context);

                    context.Writer.WriteObjectEnd();
                }

                if(publicRequest.IsSetStreamSpecification())
                {
                    context.Writer.WritePropertyName("StreamSpecification");
                    context.Writer.WriteObjectStart();

                    var marshaller = StreamSpecificationMarshaller.Instance;
                    marshaller.Marshall(publicRequest.StreamSpecification, context);

                    context.Writer.WriteObjectEnd();
                }

                if(publicRequest.IsSetTableName())
                {
                    context.Writer.WritePropertyName("TableName");
                    context.Writer.Write(publicRequest.TableName);
                }

                if(publicRequest.IsSetTags())
                {
                    context.Writer.WritePropertyName("Tags");
                    context.Writer.WriteArrayStart();
                    foreach(var publicRequestTagsListValue in publicRequest.Tags)
                    {
                        context.Writer.WriteObjectStart();

                        var marshaller = TagMarshaller.Instance;
                        marshaller.Marshall(publicRequestTagsListValue, context);

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
        private static CreateTableRequestMarshaller _instance = new CreateTableRequestMarshaller();        

        internal static CreateTableRequestMarshaller GetInstance()
        {
            return _instance;
        }

        /// <summary>
        /// Gets the singleton.
        /// </summary>  
        public static CreateTableRequestMarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

    }
}