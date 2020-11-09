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
    /// Response Unmarshaller for ReplicaAutoScalingDescription Object
    /// </summary>  
    public class ReplicaAutoScalingDescriptionUnmarshaller : IUnmarshaller<ReplicaAutoScalingDescription, XmlUnmarshallerContext>, IUnmarshaller<ReplicaAutoScalingDescription, JsonUnmarshallerContext>
    {
        /// <summary>
        /// Unmarshaller the response from the service to the response class.
        /// </summary>  
        /// <param name="context"></param>
        /// <returns></returns>
        ReplicaAutoScalingDescription IUnmarshaller<ReplicaAutoScalingDescription, XmlUnmarshallerContext>.Unmarshall(XmlUnmarshallerContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Unmarshaller the response from the service to the response class.
        /// </summary>  
        /// <param name="context"></param>
        /// <returns></returns>
        public ReplicaAutoScalingDescription Unmarshall(JsonUnmarshallerContext context)
        {
            context.Read();
            if (context.CurrentTokenType == JsonToken.Null) 
                return null;

            ReplicaAutoScalingDescription unmarshalledObject = new ReplicaAutoScalingDescription();
        
            int targetDepth = context.CurrentDepth;
            while (context.ReadAtDepth(targetDepth))
            {
                if (context.TestExpression("GlobalSecondaryIndexes", targetDepth))
                {
                    var unmarshaller = new ListUnmarshaller<ReplicaGlobalSecondaryIndexAutoScalingDescription, ReplicaGlobalSecondaryIndexAutoScalingDescriptionUnmarshaller>(ReplicaGlobalSecondaryIndexAutoScalingDescriptionUnmarshaller.Instance);
                    unmarshalledObject.GlobalSecondaryIndexes = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("RegionName", targetDepth))
                {
                    var unmarshaller = StringUnmarshaller.Instance;
                    unmarshalledObject.RegionName = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("ReplicaProvisionedReadCapacityAutoScalingSettings", targetDepth))
                {
                    var unmarshaller = AutoScalingSettingsDescriptionUnmarshaller.Instance;
                    unmarshalledObject.ReplicaProvisionedReadCapacityAutoScalingSettings = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("ReplicaProvisionedWriteCapacityAutoScalingSettings", targetDepth))
                {
                    var unmarshaller = AutoScalingSettingsDescriptionUnmarshaller.Instance;
                    unmarshalledObject.ReplicaProvisionedWriteCapacityAutoScalingSettings = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("ReplicaStatus", targetDepth))
                {
                    var unmarshaller = StringUnmarshaller.Instance;
                    unmarshalledObject.ReplicaStatus = unmarshaller.Unmarshall(context);
                    continue;
                }
            }
          
            return unmarshalledObject;
        }


        private static ReplicaAutoScalingDescriptionUnmarshaller _instance = new ReplicaAutoScalingDescriptionUnmarshaller();        

        /// <summary>
        /// Gets the singleton.
        /// </summary>  
        public static ReplicaAutoScalingDescriptionUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}