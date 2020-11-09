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

using Amazon.Runtime.Internal.Util;
using System;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// The abstract base class for exception handlers.
    /// </summary>
    /// <typeparam name="T">The exception type.</typeparam>
    public abstract class ExceptionHandler<T> : IExceptionHandler<T> where T : Exception
    {
        private ILogger _logger;

        protected ILogger Logger { get { return _logger; } }

        protected ExceptionHandler(ILogger logger)
        {
            _logger = logger;
        }

        public bool Handle(IExecutionContext executionContext, Exception exception)
        {
            return HandleException(executionContext, exception as T);
        }

        public abstract bool HandleException(IExecutionContext executionContext, T exception);
    }
}
