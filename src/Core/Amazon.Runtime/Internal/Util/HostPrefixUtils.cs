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
using System.Globalization;
using System.Text;
using System.IO;

using Amazon.Util;
using System.Text.RegularExpressions;

namespace Amazon.Runtime.Internal.Util
{
    /// <summary>
    /// Utilities for validating label values for host prefix templates
    /// </summary>
    public static class HostPrefixUtils
    {
        private static Regex labelValidationRegex = new Regex(@"^[A-Za-z0-9\-]+$", RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Validates a label value
        /// </summary>        
        public static bool IsValidLabelValue(String value) 
        {
            // Make sure value is not null or empty
            if (string.IsNullOrEmpty(value))
                return false;

            // Check if the value is between 1 and 63 characters
            if (value.Length < 1 || value.Length > 63)
                return false;
                        
            // Check that the value only contains:
            //  uppercase letters, lowercase letters, numbers,
            //  dashes (-)
            if (!labelValidationRegex.IsMatch(value))
                return false;

            return true;
        }
                        
    }
}
