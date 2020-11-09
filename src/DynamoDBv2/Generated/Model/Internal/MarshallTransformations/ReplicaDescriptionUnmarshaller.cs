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
    /// Response Unmarshaller for ReplicaDescription Object
    /// </summary>  
    public class ReplicaDescriptionUnmarshaller : IUnmarshaller<ReplicaDescription, XmlUnmarshallerContext>, IUnmarshaller<ReplicaDescription, JsonUnmarshallerContext>
    {
        /// <summary>
        /// Unmarshaller the response from the service to the response class.
        /// </summary>  
        /// <param name="context"></param>
        /// <returns></returns>
        ReplicaDescription IUnmarshaller<ReplicaDescription, XmlUnmarshallerContext>.Unmarshall(XmlUnmarshallerContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Unmarshaller the response from the service to the response class.
        /// </summary>  
        /// <param name="context"></param>
        /// <returns></returns>
        public ReplicaDescription Unmarshall(JsonUnmarshallerContext context)
        {
            context.Read();
            if (context.CurrentTokenType == JsonToken.Null) 
                return null;

            ReplicaDescription unmarshalledObject = new ReplicaDescription();
        
            int targetDepth = context.CurrentDepth;
            while (context.ReadAtDepth(targetDepth))
            {
                if (context.TestExpression("GlobalSecondaryIndexes", targetDepth))
                {
                    var unmarshaller = new ListUnmarshaller<ReplicaGlobalSecondaryIndexDescription, ReplicaGlobalSecondaryIndexDescriptionUnmarshaller>(ReplicaGlobalSecondaryIndexDescriptionUnmarshaller.Instance);
                    unmarshalledObject.GlobalSecondaryIndexes = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("KMSMasterKeyId", targetDepth))
                {
                    var unmarshaller = StringUnmarshaller.Instance;
                    unmarshalledObject.KMSMasterKeyId = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("ProvisionedThroughputOverride", targetDepth))
                {
                    var unmarshaller = ProvisionedThroughputOverrideUnmarshaller.Instance;
                    unmarshalledObject.ProvisionedThroughputOverride = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("RegionName", targetDepth))
                {
                    var unmarshaller = StringUnmarshaller.Instance;
                    unmarshalledObject.RegionName = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("ReplicaInaccessibleDateTime", targetDepth))
                {
                    var unmarshaller = DateTimeUnmarshaller.Instance;
                    unmarshalledObject.ReplicaInaccessibleDateTime = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("ReplicaStatus", targetDepth))
                {
                    var unmarshaller = StringUnmarshaller.Instance;
                    unmarshalledObject.ReplicaStatus = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("ReplicaStatusDescription", targetDepth))
                {
                    var unmarshaller = StringUnmarshaller.Instance;
                    unmarshalledObject.ReplicaStatusDescription = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("ReplicaStatusPercentProgress", targetDepth))
                {
                    var unmarshaller = StringUnmarshaller.Instance;
                    unmarshalledObject.ReplicaStatusPercentProgress = unmarshaller.Unmarshall(context);
                    continue;
                }
            }
          
            return unmarshalledObject;
        }


        private static ReplicaDescriptionUnmarshaller _instance = new ReplicaDescriptionUnmarshaller();        

        /// <summary>
        /// Gets the singleton.
        /// </summary>  
        public static ReplicaDescriptionUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}