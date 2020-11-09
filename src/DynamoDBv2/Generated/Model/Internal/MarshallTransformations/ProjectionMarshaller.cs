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
    /// Projection Marshaller
    /// </summary>       
    public class ProjectionMarshaller : IRequestMarshaller<Projection, JsonMarshallerContext> 
    {
        /// <summary>
        /// Unmarshaller the response from the service to the response class.
        /// </summary>  
        /// <param name="requestObject"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public void Marshall(Projection requestObject, JsonMarshallerContext context)
        {
            if(requestObject.IsSetNonKeyAttributes())
            {
                context.Writer.WritePropertyName("NonKeyAttributes");
                context.Writer.WriteArrayStart();
                foreach(var requestObjectNonKeyAttributesListValue in requestObject.NonKeyAttributes)
                {
                        context.Writer.Write(requestObjectNonKeyAttributesListValue);
                }
                context.Writer.WriteArrayEnd();
            }

            if(requestObject.IsSetProjectionType())
            {
                context.Writer.WritePropertyName("ProjectionType");
                context.Writer.Write(requestObject.ProjectionType);
            }

        }

        /// <summary>
        /// Singleton Marshaller.
        /// </summary>  
        public readonly static ProjectionMarshaller Instance = new ProjectionMarshaller();

    }
}