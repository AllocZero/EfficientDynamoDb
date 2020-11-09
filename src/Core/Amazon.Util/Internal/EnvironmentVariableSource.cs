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

namespace Amazon.Util.Internal
{
    /// <summary>
    /// Singleton class that holds the property of type IEnvironmentVariableRetreiver.
    /// This property can hold an instance of type EnvironmentVariableRetreiver which has a wrapper 
    /// method for 
    /// public static string GetEnvironmentVariable(string variable)
    /// or can be mocked for testing purposes.
    /// </summary>
    public sealed class EnvironmentVariableSource
    {
        private static readonly EnvironmentVariableSource instance = new EnvironmentVariableSource();

        private EnvironmentVariableSource()
        {
        }

        static EnvironmentVariableSource()
        {
        }
        public IEnvironmentVariableRetriever EnvironmentVariableRetriever { get; set; } = new EnvironmentVariableRetriever();

        public static EnvironmentVariableSource Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
