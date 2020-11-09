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
    /// Response Unmarshaller for AutoScalingSettingsDescription Object
    /// </summary>  
    public class AutoScalingSettingsDescriptionUnmarshaller : IUnmarshaller<AutoScalingSettingsDescription, XmlUnmarshallerContext>, IUnmarshaller<AutoScalingSettingsDescription, JsonUnmarshallerContext>
    {
        /// <summary>
        /// Unmarshaller the response from the service to the response class.
        /// </summary>  
        /// <param name="context"></param>
        /// <returns></returns>
        AutoScalingSettingsDescription IUnmarshaller<AutoScalingSettingsDescription, XmlUnmarshallerContext>.Unmarshall(XmlUnmarshallerContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Unmarshaller the response from the service to the response class.
        /// </summary>  
        /// <param name="context"></param>
        /// <returns></returns>
        public AutoScalingSettingsDescription Unmarshall(JsonUnmarshallerContext context)
        {
            context.Read();
            if (context.CurrentTokenType == JsonToken.Null) 
                return null;

            AutoScalingSettingsDescription unmarshalledObject = new AutoScalingSettingsDescription();
        
            int targetDepth = context.CurrentDepth;
            while (context.ReadAtDepth(targetDepth))
            {
                if (context.TestExpression("AutoScalingDisabled", targetDepth))
                {
                    var unmarshaller = BoolUnmarshaller.Instance;
                    unmarshalledObject.AutoScalingDisabled = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("AutoScalingRoleArn", targetDepth))
                {
                    var unmarshaller = StringUnmarshaller.Instance;
                    unmarshalledObject.AutoScalingRoleArn = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("MaximumUnits", targetDepth))
                {
                    var unmarshaller = LongUnmarshaller.Instance;
                    unmarshalledObject.MaximumUnits = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("MinimumUnits", targetDepth))
                {
                    var unmarshaller = LongUnmarshaller.Instance;
                    unmarshalledObject.MinimumUnits = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("ScalingPolicies", targetDepth))
                {
                    var unmarshaller = new ListUnmarshaller<AutoScalingPolicyDescription, AutoScalingPolicyDescriptionUnmarshaller>(AutoScalingPolicyDescriptionUnmarshaller.Instance);
                    unmarshalledObject.ScalingPolicies = unmarshaller.Unmarshall(context);
                    continue;
                }
            }
          
            return unmarshalledObject;
        }


        private static AutoScalingSettingsDescriptionUnmarshaller _instance = new AutoScalingSettingsDescriptionUnmarshaller();        

        /// <summary>
        /// Gets the singleton.
        /// </summary>  
        public static AutoScalingSettingsDescriptionUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}