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

namespace Amazon.Runtime.Internal.Util
{
    /// <summary>
    /// Utilities for converting objects to strings. Used by the marshaller classes.
    /// </summary>
    public static class StringUtils
    {
        private static readonly Encoding UTF_8 = Encoding.UTF8;

        public static string FromString(String value) 
        {
            return value;
        }

        public static string FromStringWithSlashEncoding(String value)
        {
            return AWSSDKUtils.UrlEncodeSlash(FromString(value));
        }

        public static string FromString(ConstantClass value)
        {
            return value == null ? "" : value.Intern().Value;
        }

        public static string FromMemoryStream(MemoryStream value)
        {
            return Convert.ToBase64String(value.ToArray());
        }

        public static string FromInt(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
        
        public static string FromInt(int? value)
        {
            if (value != null)
                return value.Value.ToString(CultureInfo.InvariantCulture);

            return null;
        }

        public static string FromLong(long value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static string FromBool(bool value)
        {
            return value ? "true" : "false";
        }

        [Obsolete("This method doesn't handle correctly non-UTC DateTimes. Use FromDateTimeToISO8601 instead.", false)]
        public static string FromDateTime(DateTime value)
        {
            return value.ToString(AWSSDKUtils.ISO8601DateFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a DateTime to ISO8601 formatted string.
        /// </summary>
        public static string FromDateTimeToISO8601(DateTime value)
        {
            return value.ToUniversalTime().ToString(AWSSDKUtils.ISO8601DateFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a DateTime to RFC822 formatted string.
        /// </summary>
        public static string FromDateTimeToRFC822(DateTime value)
        {
            return value.ToUniversalTime().ToString(
                AWSSDKUtils.RFC822DateFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a DateTime to Unix epoch time formatted string.
        /// </summary>
        public static string FromDateTimeToUnixTimestamp(DateTime value)
        {
            return AWSSDKUtils.ConvertToUnixEpochSecondsString(value);
        }

        public static string FromDouble(double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static string FromDecimal(decimal value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static long Utf8ByteLength(string value)
        {
            if (value == null)
            {
                return 0;
            }
            return UTF_8.GetByteCount(value);
        }
    }
}
