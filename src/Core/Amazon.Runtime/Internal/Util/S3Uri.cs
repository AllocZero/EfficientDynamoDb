/*******************************************************************************
 *  Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *  Licensed under the Apache License, Version 2.0 (the "License"). You may not use
 *  this file except in compliance with the License. A copy of the License is located at
 *
 *  http://aws.amazon.com/apache2.0
 *
 *  or in the "license" file accompanying this file.
 *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
 *  CONDITIONS OF ANY KIND, either express or implied. See the License for the
 *  specific language governing permissions and limitations under the License.
 * *****************************************************************************
 *    __  _    _  ___
 *   (  )( \/\/ )/ __)
 *   /__\ \    / \__ \
 *  (_)(_) \/\/  (___/
 *
 *  AWS SDK for .NET
 *
 */

using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Amazon.Runtime.Internal.Util
{
    /// <summary>
    /// Uri wrapper that can parse out information (bucket, key, region, style) from an
    /// S3 URI.
    /// </summary>
    public class S3Uri
    {
        private const string S3EndpointPattern = @"^(.+\.)?s3[.-]([a-z0-9-]+)\.";
        //s3-control has a similar pattern to s3-region host names, so we explicitly exclude it
        private const string S3ControlExlusionPattern = @"^(.+\.)?s3-control\.";

        private static readonly Regex S3EndpointRegex = new Regex(S3EndpointPattern, RegexOptions.Compiled);
        private static readonly Regex S3ControlExlusionRegex = new Regex(S3ControlExlusionPattern, RegexOptions.Compiled);

        /// <summary>
        /// True if the URI contains the bucket in the path, false if it contains the bucket in the authority.
        /// </summary>
        public bool IsPathStyle { get; private set; }

        /// <summary>
        /// The bucket name parsed from the URI (or null if no bucket specified).
        /// </summary>
        public string Bucket { get; private set; }

        /// <summary>
        /// The key parsed from the URI (or null if no key specified).
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// The region parsed from the URI (or null if no region specified).
        /// </summary>
        public RegionEndpoint Region { get; set; }

        /// <summary>
        /// Constructs a parser for the S3 URI specified as a string.
        /// </summary>
        /// <param name="uri">The S3 URI to be parsed.</param>
        public S3Uri(string uri)
            : this(new Uri(uri))
        {
        }

        /// <summary>
        /// Constructs a parser for the S3 URI specified as a Uri instance.
        /// </summary>
        /// <param name="uri">The S3 URI to be parsed.</param>
        public S3Uri(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            if (string.IsNullOrEmpty(uri.Host))
                throw new ArgumentException("Invalid URI - no hostname present");

            var match = S3EndpointRegex.Match(uri.Host);
            if (!match.Success && !S3ControlExlusionRegex.Match(uri.Host).Success)
                throw new ArgumentException("Invalid S3 URI - hostname does not appear to be a valid S3 endpoint");

            // for host style urls:
            //   group 0 is bucketname plus 's3' prefix and possible region code
            //   group 1 is bucket name
            //   group 2 will be region or 'amazonaws' if US Classic region
            // for path style urls:
            //   group 0 will be s3 prefix plus possible region code
            //   group 1 will be empty
            //   group 2 will be region or 'amazonaws' if US Classic region
            var bucketNameGroup = match.Groups[1];
            if (string.IsNullOrEmpty(bucketNameGroup.Value))
            {
                // no bucket name in the authority, parse it from the path
                this.IsPathStyle = true;

                // grab the encoded path so we don't run afoul of '/'s in the bucket name
                var path = uri.AbsolutePath;
                if (path.Equals("/"))
                {
                    this.Bucket = null;
                    this.Key = null;
                }
                else
                {
                    var index = path.IndexOf('/', 1);
                    if (index == -1)
                    {
                        // https://s3.amazonaws.com/bucket
                        this.Bucket = Decode(path.Substring(1));
                        this.Key = null;
                    }
                    else if (index == (path.Length - 1))
                    {
                        // https://s3.amazonaws.com/bucket/
                        this.Bucket = Decode(path.Substring(1, index)).TrimEnd('/');
                        this.Key = null;
                    }
                    else
                    {
                        // https://s3.amazonaws.com/bucket/key
                        this.Bucket = Decode(path.Substring(1, index)).TrimEnd('/');
                        this.Key = Decode(path.Substring(index + 1));
                    }
                }
            }
            else
            {
                // bucket name in the host, path is the object key
                this.IsPathStyle = false;

                // remove any trailing '.' from the prefix to get the bucket name
                this.Bucket = bucketNameGroup.Value.TrimEnd('.');
                this.Key = uri.AbsolutePath.Equals("/") ? null : uri.AbsolutePath.Substring(1);
            }

            if (match.Groups.Count > 2)
            {
                // US 'classic' urls will not have a region code in the endpoint
                var regionGroupValue = match.Groups[2].Value;
                if (regionGroupValue.Equals("amazonaws", StringComparison.Ordinal)
                    || regionGroupValue.Equals("external-1", StringComparison.Ordinal))
                    this.Region = RegionEndpoint.USEast1;
                else
                    this.Region = RegionEndpoint.GetBySystemName(regionGroupValue);
            }
        }


        public static bool IsS3Uri(Uri uri)
        {
            return S3EndpointRegex.Match(uri.Host).Success && !S3ControlExlusionRegex.Match(uri.Host).Success;
        }

        /// <summary>
        /// Percent-decodes the given string, with a fast path for strings that are not
        /// percent-encoded.
        /// </summary>
        /// <param name="s">The string to decode</param>
        /// <returns>The decoded string</returns>
        static string Decode(string s)
        {
            if (s == null)
                return null;

            for (var i = 0; i < s.Length; ++i)
            {
                if (s[i] == '%')
                    return Decode(s, i);
            }

            return s;
        }

        /// <summary>
        /// Percent-decodes the given string.
        /// </summary>
        /// <param name="s">The string to decode</param>
        /// <param name="firstPercent">The index of the first '%' in the string</param>
        /// <returns>The decoded string</returns>
        static string Decode(string s, int firstPercent)
        {
            var sb = new StringBuilder(s.Substring(0, firstPercent));

            AppendDecoded(sb, s, firstPercent);

            for (var i = firstPercent + 3; i < s.Length; ++i)
            {
                if (s[i] == '%')
                {
                    AppendDecoded(sb, s, i);
                    i += 2;
                }
                else
                    sb.Append(s[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Decodes the percent-encoded character at the given index in the string
        /// and appends the decoded value to the string under construction.
        /// </summary>
        /// <param name="builder">
        /// The string under construction to which the decoded character will be 
        /// appended.
        /// </param>
        /// <param name="s">The string being decoded.</param>
        /// <param name="index">The index of the '%' character in the string.</param>
        static void AppendDecoded(StringBuilder builder, string s, int index)
        {
            if (index > s.Length - 3)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                                                                  "Invalid percent-encoded string '{0}'",
                                                                  s));

            var first = s[index + 1];
            var second = s[index + 2];

            var decoded = (char)(FromHex(first) << 4 | FromHex(second));
            builder.Append(decoded);
        }

        /// <summary>
        /// Converts a hex character (0-9A-Fa-f) into its corresponding quad value.
        /// </summary>
        /// <param name="c">The hex character</param>
        /// <returns>The quad value</returns>
        static int FromHex(char c)
        {
            if (c < '0')
            {
                throw new InvalidOperationException(
                    "Invalid percent-encoded string: bad character '" + c + "' in "
                    + "escape sequence.");
            }
            if (c <= '9')
            {
                return (c - '0');
            }

            if (c < 'A')
            {
                throw new InvalidOperationException(
                    "Invalid percent-encoded string: bad character '" + c + "' in "
                    + "escape sequence.");
            }
            if (c <= 'F')
            {
                return (c - 'A') + 10;
            }

            if (c < 'a')
            {
                throw new InvalidOperationException(
                    "Invalid percent-encoded string: bad character '" + c + "' in "
                    + "escape sequence.");
            }
            if (c <= 'f')
            {
                return (c - 'a') + 10;
            }

            throw new InvalidOperationException(
                "Invalid percent-encoded string: bad character '" + c + "' in "
                + "escape sequence.");
        }
    }
}
