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
 * Do not modify this file. This file is generated from the dynamodbstreams-2012-08-10.normal.json service model.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
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
    /// Response Unmarshaller for Record Object
    /// </summary>  
    public class RecordUnmarshaller : IUnmarshaller<Record, XmlUnmarshallerContext>, IUnmarshaller<Record, JsonUnmarshallerContext>
    {
        /// <summary>
        /// Unmarshaller the response from the service to the response class.
        /// </summary>  
        /// <param name="context"></param>
        /// <returns></returns>
        Record IUnmarshaller<Record, XmlUnmarshallerContext>.Unmarshall(XmlUnmarshallerContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Unmarshaller the response from the service to the response class.
        /// </summary>  
        /// <param name="context"></param>
        /// <returns></returns>
        public Record Unmarshall(JsonUnmarshallerContext context)
        {
            context.Read();
            if (context.CurrentTokenType == JsonToken.Null) 
                return null;

            Record unmarshalledObject = new Record();
        
            int targetDepth = context.CurrentDepth;
            while (context.ReadAtDepth(targetDepth))
            {
                if (context.TestExpression("awsRegion", targetDepth))
                {
                    var unmarshaller = StringUnmarshaller.Instance;
                    unmarshalledObject.AwsRegion = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("dynamodb", targetDepth))
                {
                    var unmarshaller = StreamRecordUnmarshaller.Instance;
                    unmarshalledObject.Dynamodb = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("eventID", targetDepth))
                {
                    var unmarshaller = StringUnmarshaller.Instance;
                    unmarshalledObject.EventID = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("eventName", targetDepth))
                {
                    var unmarshaller = StringUnmarshaller.Instance;
                    unmarshalledObject.EventName = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("eventSource", targetDepth))
                {
                    var unmarshaller = StringUnmarshaller.Instance;
                    unmarshalledObject.EventSource = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("eventVersion", targetDepth))
                {
                    var unmarshaller = StringUnmarshaller.Instance;
                    unmarshalledObject.EventVersion = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("userIdentity", targetDepth))
                {
                    var unmarshaller = IdentityUnmarshaller.Instance;
                    unmarshalledObject.UserIdentity = unmarshaller.Unmarshall(context);
                    continue;
                }
            }
          
            return unmarshalledObject;
        }


        private static RecordUnmarshaller _instance = new RecordUnmarshaller();        

        /// <summary>
        /// Gets the singleton.
        /// </summary>  
        public static RecordUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}