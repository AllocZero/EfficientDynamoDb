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

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Xml;
using System.IO;
using Amazon.Util;

namespace Amazon.Runtime.Internal.Transform
{
    /// <summary>
    /// Interface for unmarshallers which unmarshall objects from response data.
    /// The Unmarshallers are stateless, and only encode the rules for what data 
    /// in the XML stream goes into what members of an object. 
    /// </summary>
    /// <typeparam name="T">The type of object the unmarshaller returns</typeparam>
    /// <typeparam name="R">The type of the XML unmashaller context, which contains the
    /// state during parsing of the XML stream. Usually an instance of 
    /// <c>Amazon.Runtime.Internal.Transform.UnmarshallerContext</c>.</typeparam>
    public interface IUnmarshaller<T, R>
    {
        /// <summary>
        /// Given the current position in the XML stream, extract a T.
        /// </summary>
        /// <param name="input">The XML parsing context</param>
        /// <returns>An object of type T populated with data from the XML stream.</returns>
        T Unmarshall(R input);
    }
}
