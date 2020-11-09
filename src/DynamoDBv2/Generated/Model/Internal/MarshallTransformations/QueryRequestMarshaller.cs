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
    /// Query Request Marshaller
    /// </summary>       
    public class QueryRequestMarshaller : IMarshaller<IRequest, QueryRequest> , IMarshaller<IRequest,AmazonWebServiceRequest>
    {
        /// <summary>
        /// Marshaller the request object to the HTTP request.
        /// </summary>  
        /// <param name="input"></param>
        /// <returns></returns>
        public IRequest Marshall(AmazonWebServiceRequest input)
        {
            return this.Marshall((QueryRequest)input);
        }

        /// <summary>
        /// Marshaller the request object to the HTTP request.
        /// </summary>  
        /// <param name="publicRequest"></param>
        /// <returns></returns>
        public IRequest Marshall(QueryRequest publicRequest)
        {
            IRequest request = new DefaultRequest(publicRequest, "Amazon.DynamoDBv2");
            string target = "DynamoDB_20120810.Query";
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
                if(publicRequest.IsSetAttributesToGet())
                {
                    context.Writer.WritePropertyName("AttributesToGet");
                    context.Writer.WriteArrayStart();
                    foreach(var publicRequestAttributesToGetListValue in publicRequest.AttributesToGet)
                    {
                            context.Writer.Write(publicRequestAttributesToGetListValue);
                    }
                    context.Writer.WriteArrayEnd();
                }

                if(publicRequest.IsSetConditionalOperator())
                {
                    context.Writer.WritePropertyName("ConditionalOperator");
                    context.Writer.Write(publicRequest.ConditionalOperator);
                }

                if(publicRequest.IsSetConsistentRead())
                {
                    context.Writer.WritePropertyName("ConsistentRead");
                    context.Writer.Write(publicRequest.ConsistentRead);
                }

                if(publicRequest.IsSetExclusiveStartKey())
                {
                    context.Writer.WritePropertyName("ExclusiveStartKey");
                    context.Writer.WriteObjectStart();
                    foreach (var publicRequestExclusiveStartKeyKvp in publicRequest.ExclusiveStartKey)
                    {
                        context.Writer.WritePropertyName(publicRequestExclusiveStartKeyKvp.Key);
                        var publicRequestExclusiveStartKeyValue = publicRequestExclusiveStartKeyKvp.Value;

                        context.Writer.WriteObjectStart();

                        var marshaller = AttributeValueMarshaller.Instance;
                        marshaller.Marshall(publicRequestExclusiveStartKeyValue, context);

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

                if(publicRequest.IsSetFilterExpression())
                {
                    context.Writer.WritePropertyName("FilterExpression");
                    context.Writer.Write(publicRequest.FilterExpression);
                }

                if(publicRequest.IsSetIndexName())
                {
                    context.Writer.WritePropertyName("IndexName");
                    context.Writer.Write(publicRequest.IndexName);
                }

                if(publicRequest.IsSetKeyConditionExpression())
                {
                    context.Writer.WritePropertyName("KeyConditionExpression");
                    context.Writer.Write(publicRequest.KeyConditionExpression);
                }

                if(publicRequest.IsSetKeyConditions())
                {
                    context.Writer.WritePropertyName("KeyConditions");
                    context.Writer.WriteObjectStart();
                    foreach (var publicRequestKeyConditionsKvp in publicRequest.KeyConditions)
                    {
                        context.Writer.WritePropertyName(publicRequestKeyConditionsKvp.Key);
                        var publicRequestKeyConditionsValue = publicRequestKeyConditionsKvp.Value;

                        context.Writer.WriteObjectStart();

                        var marshaller = ConditionMarshaller.Instance;
                        marshaller.Marshall(publicRequestKeyConditionsValue, context);

                        context.Writer.WriteObjectEnd();
                    }
                    context.Writer.WriteObjectEnd();
                }

                if(publicRequest.IsSetLimit())
                {
                    context.Writer.WritePropertyName("Limit");
                    context.Writer.Write(publicRequest.Limit);
                }

                if(publicRequest.IsSetProjectionExpression())
                {
                    context.Writer.WritePropertyName("ProjectionExpression");
                    context.Writer.Write(publicRequest.ProjectionExpression);
                }

                if(publicRequest.IsSetQueryFilter())
                {
                    context.Writer.WritePropertyName("QueryFilter");
                    context.Writer.WriteObjectStart();
                    foreach (var publicRequestQueryFilterKvp in publicRequest.QueryFilter)
                    {
                        context.Writer.WritePropertyName(publicRequestQueryFilterKvp.Key);
                        var publicRequestQueryFilterValue = publicRequestQueryFilterKvp.Value;

                        context.Writer.WriteObjectStart();

                        var marshaller = ConditionMarshaller.Instance;
                        marshaller.Marshall(publicRequestQueryFilterValue, context);

                        context.Writer.WriteObjectEnd();
                    }
                    context.Writer.WriteObjectEnd();
                }

                if(publicRequest.IsSetReturnConsumedCapacity())
                {
                    context.Writer.WritePropertyName("ReturnConsumedCapacity");
                    context.Writer.Write(publicRequest.ReturnConsumedCapacity);
                }

                if(publicRequest.IsSetScanIndexForward())
                {
                    context.Writer.WritePropertyName("ScanIndexForward");
                    context.Writer.Write(publicRequest.ScanIndexForward);
                }

                if(publicRequest.IsSetSelect())
                {
                    context.Writer.WritePropertyName("Select");
                    context.Writer.Write(publicRequest.Select);
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
        private static QueryRequestMarshaller _instance = new QueryRequestMarshaller();        

        internal static QueryRequestMarshaller GetInstance()
        {
            return _instance;
        }

        /// <summary>
        /// Gets the singleton.
        /// </summary>  
        public static QueryRequestMarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

    }
}