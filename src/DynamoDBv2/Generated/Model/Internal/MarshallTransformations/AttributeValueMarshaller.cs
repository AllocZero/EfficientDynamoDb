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
    /// AttributeValue Marshaller
    /// </summary>       
    public class AttributeValueMarshaller : IRequestMarshaller<AttributeValue, JsonMarshallerContext> 
    {
        /// <summary>
        /// Unmarshaller the response from the service to the response class.
        /// </summary>  
        /// <param name="requestObject"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public void Marshall(AttributeValue requestObject, JsonMarshallerContext context)
        {
            if(requestObject.IsSetB())
            {
                context.Writer.WritePropertyName("B");
                context.Writer.Write(StringUtils.FromMemoryStream(requestObject.B));
            }

            if(requestObject.IsSetBOOL())
            {
                context.Writer.WritePropertyName("BOOL");
                context.Writer.Write(requestObject.BOOL);
            }

            if(requestObject.IsSetBS())
            {
                context.Writer.WritePropertyName("BS");
                context.Writer.WriteArrayStart();
                foreach(var requestObjectBSListValue in requestObject.BS)
                {
                    context.Writer.Write(StringUtils.FromMemoryStream(requestObjectBSListValue));
                }
                context.Writer.WriteArrayEnd();
            }

            if(requestObject.IsSetL())
            {
                context.Writer.WritePropertyName("L");
                context.Writer.WriteArrayStart();
                foreach(var requestObjectLListValue in requestObject.L)
                {
                    context.Writer.WriteObjectStart();

                    var marshaller = AttributeValueMarshaller.Instance;
                    marshaller.Marshall(requestObjectLListValue, context);

                    context.Writer.WriteObjectEnd();
                }
                context.Writer.WriteArrayEnd();
            }

            if(requestObject.IsSetM())
            {
                context.Writer.WritePropertyName("M");
                context.Writer.WriteObjectStart();
                foreach (var requestObjectMKvp in requestObject.M)
                {
                    context.Writer.WritePropertyName(requestObjectMKvp.Key);
                    var requestObjectMValue = requestObjectMKvp.Value;

                    context.Writer.WriteObjectStart();

                    var marshaller = AttributeValueMarshaller.Instance;
                    marshaller.Marshall(requestObjectMValue, context);

                    context.Writer.WriteObjectEnd();
                }
                context.Writer.WriteObjectEnd();
            }

            if(requestObject.IsSetN())
            {
                context.Writer.WritePropertyName("N");
                context.Writer.Write(requestObject.N);
            }

            if(requestObject.IsSetNS())
            {
                context.Writer.WritePropertyName("NS");
                context.Writer.WriteArrayStart();
                foreach(var requestObjectNSListValue in requestObject.NS)
                {
                        context.Writer.Write(requestObjectNSListValue);
                }
                context.Writer.WriteArrayEnd();
            }

            if(requestObject.IsSetNULL())
            {
                context.Writer.WritePropertyName("NULL");
                context.Writer.Write(requestObject.NULL);
            }

            if(requestObject.IsSetS())
            {
                context.Writer.WritePropertyName("S");
                context.Writer.Write(requestObject.S);
            }

            if(requestObject.IsSetSS())
            {
                context.Writer.WritePropertyName("SS");
                context.Writer.WriteArrayStart();
                foreach(var requestObjectSSListValue in requestObject.SS)
                {
                        context.Writer.Write(requestObjectSSListValue);
                }
                context.Writer.WriteArrayEnd();
            }

        }

        /// <summary>
        /// Singleton Marshaller.
        /// </summary>  
        public readonly static AttributeValueMarshaller Instance = new AttributeValueMarshaller();

    }
}