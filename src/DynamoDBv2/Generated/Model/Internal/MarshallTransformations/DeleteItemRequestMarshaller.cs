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
    /// DeleteItem Request Marshaller
    /// </summary>       
    public class DeleteItemRequestMarshaller : IMarshaller<IRequest, DeleteItemRequest> , IMarshaller<IRequest,AmazonWebServiceRequest>
    {
        /// <summary>
        /// Marshaller the request object to the HTTP request.
        /// </summary>  
        /// <param name="input"></param>
        /// <returns></returns>
        public IRequest Marshall(AmazonWebServiceRequest input)
        {
            return this.Marshall((DeleteItemRequest)input);
        }

        /// <summary>
        /// Marshaller the request object to the HTTP request.
        /// </summary>  
        /// <param name="publicRequest"></param>
        /// <returns></returns>
        public IRequest Marshall(DeleteItemRequest publicRequest)
        {
            IRequest request = new DefaultRequest(publicRequest, "Amazon.DynamoDBv2");
            string target = "DynamoDB_20120810.DeleteItem";
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
                if(publicRequest.IsSetConditionalOperator())
                {
                    context.Writer.WritePropertyName("ConditionalOperator");
                    context.Writer.Write(publicRequest.ConditionalOperator);
                }

                if(publicRequest.IsSetConditionExpression())
                {
                    context.Writer.WritePropertyName("ConditionExpression");
                    context.Writer.Write(publicRequest.ConditionExpression);
                }

                if(publicRequest.IsSetExpected())
                {
                    context.Writer.WritePropertyName("Expected");
                    context.Writer.WriteObjectStart();
                    foreach (var publicRequestExpectedKvp in publicRequest.Expected)
                    {
                        context.Writer.WritePropertyName(publicRequestExpectedKvp.Key);
                        var publicRequestExpectedValue = publicRequestExpectedKvp.Value;

                        context.Writer.WriteObjectStart();

                        var marshaller = ExpectedAttributeValueMarshaller.Instance;
                        marshaller.Marshall(publicRequestExpectedValue, context);

                        context.Writer.WriteObjectEnd();
                    }
                    context.Writer.WriteObjectEnd();
                }

                if(publicRequest.IsSetExpressionAttributeNames())
                {
                    context.Writer.WritePropertyName("ExpressionAttributeNames");
                    context.Writer.WriteObjectStart();
                    foreach (var publicRequestExpressionAttributeNamesKvp in publicRequest.ExpressionAttributeNames)
                    {
                        context.Writer.WritePropertyName(publicRequestExpressionAttributeNamesKvp.Key);
                        var publicRequestExpressionAttributeNamesValue = publicRequestExpressionAttributeNamesKvp.Value;

                            context.Writer.Write(publicRequestExpressionAttributeNamesValue);
                    }
                    context.Writer.WriteObjectEnd();
                }

                if(publicRequest.IsSetExpressionAttributeValues())
                {
                    context.Writer.WritePropertyName("ExpressionAttributeValues");
                    context.Writer.WriteObjectStart();
                    foreach (var publicRequestExpressionAttributeValuesKvp in publicRequest.ExpressionAttributeValues)
                    {
                        context.Writer.WritePropertyName(publicRequestExpressionAttributeValuesKvp.Key);
                        var publicRequestExpressionAttributeValuesValue = publicRequestExpressionAttributeValuesKvp.Value;

                        context.Writer.WriteObjectStart();

                        var marshaller = AttributeValueMarshaller.Instance;
                        marshaller.Marshall(publicRequestExpressionAttributeValuesValue, context);

                        context.Writer.WriteObjectEnd();
                    }
                    context.Writer.WriteObjectEnd();
                }

                if(publicRequest.IsSetKey())
                {
                    context.Writer.WritePropertyName("Key");
                    context.Writer.WriteObjectStart();
                    foreach (var publicRequestKeyKvp in publicRequest.Key)
                    {
                        context.Writer.WritePropertyName(publicRequestKeyKvp.Key);
                        var publicRequestKeyValue = publicRequestKeyKvp.Value;

                        context.Writer.WriteObjectStart();

                        var marshaller = AttributeValueMarshaller.Instance;
                        marshaller.Marshall(publicRequestKeyValue, context);

                        context.Writer.WriteObjectEnd();
                    }
                    context.Writer.WriteObjectEnd();
                }

                if(publicRequest.IsSetReturnConsumedCapacity())
                {
                    context.Writer.WritePropertyName("ReturnConsumedCapacity");
                    context.Writer.Write(publicRequest.ReturnConsumedCapacity);
                }

                if(publicRequest.IsSetReturnItemCollectionMetrics())
                {
                    context.Writer.WritePropertyName("ReturnItemCollectionMetrics");
                    context.Writer.Write(publicRequest.ReturnItemCollectionMetrics);
                }

                if(publicRequest.IsSetReturnValues())
                {
                    context.Writer.WritePropertyName("ReturnValues");
                    context.Writer.Write(publicRequest.ReturnValues);
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
        private static DeleteItemRequestMarshaller _instance = new DeleteItemRequestMarshaller();        

        internal static DeleteItemRequestMarshaller GetInstance()
        {
            return _instance;
        }

        /// <summary>
        /// Gets the singleton.
        /// </summary>  
        public static DeleteItemRequestMarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

    }
}