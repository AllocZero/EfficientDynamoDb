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

using Amazon.Runtime.Internal;

using System;
using System.Xml;

namespace Amazon.Runtime.Internal.Transform
{
    /// <summary>
    ///    Response Unmarshaller for all Errors
    /// </summary>
    public class ErrorResponseUnmarshaller : IUnmarshaller<ErrorResponse, XmlUnmarshallerContext>
    {
        /// <summary>
        /// Build an ErrorResponse from XML 
        /// </summary>
        /// <param name="context">The XML parsing context. 
        /// Usually an <c>Amazon.Runtime.Internal.UnmarshallerContext</c>.</param>
        /// <returns>An <c>ErrorResponse</c> object.</returns>
        public ErrorResponse Unmarshall(XmlUnmarshallerContext context)
        {
            var response = new ErrorResponse()
            {
                // this default value will get overwritten if it's included in the XML
                Type = ErrorType.Unknown
            };

            PopulateErrorResponseFromXmlIfPossible(context, response);

            // if it wasn't possible to get a message from the XML then set a useful message here
            if (string.IsNullOrEmpty(response.Message))
            {
                if (string.IsNullOrEmpty(response.Code))
                {
                    if (string.IsNullOrEmpty(context.ResponseBody))
                        response.Message = "The service returned an error. See inner exception for details.";
                    else
                        response.Message = "The service returned an error with HTTP Body: " + context.ResponseBody;
                }
                else
                {
                    response.Message = "The service returned an error with Error Code " + response.Code + " and HTTP Body: " + context.ResponseBody;
                }
            }

            return response;
        }

        private static void PopulateErrorResponseFromXmlIfPossible(XmlUnmarshallerContext context, ErrorResponse response)
        {
            while (TryReadContext(context))
            {
                if (context.IsStartElement)
                {
                    if (context.TestExpression("Error/Type"))
                    {
                        try
                        {
                            response.Type = (ErrorType)Enum.Parse(typeof(ErrorType), StringUnmarshaller.GetInstance().Unmarshall(context), true);
                        }
                        catch (ArgumentException)
                        {
                            response.Type = ErrorType.Unknown;
                        }
                        continue;
                    }
                    if (context.TestExpression("Error/Code"))
                    {
                        response.Code = StringUnmarshaller.GetInstance().Unmarshall(context);
                        continue;
                    }
                    if (context.TestExpression("Error/Message"))
                    {
                        response.Message = StringUnmarshaller.GetInstance().Unmarshall(context);
                        continue;
                    }
                    if (context.TestExpression("RequestId"))
                    {
                        response.RequestId = StringUnmarshaller.GetInstance().Unmarshall(context);
                    }
                }
            }
        }

        private static bool TryReadContext(XmlUnmarshallerContext context)
        {
            try
            {
                return context.Read();
            }
            catch (XmlException)
            {
                return false;
            }
        }

        private static ErrorResponseUnmarshaller instance;

        /// <summary>
        /// Return an instance of and ErrorResponseUnmarshaller.
        /// </summary>
        /// <returns></returns>
        public static ErrorResponseUnmarshaller GetInstance()
        {
            if (instance == null)
                instance = new ErrorResponseUnmarshaller();

            return instance;
        }
    }
}
