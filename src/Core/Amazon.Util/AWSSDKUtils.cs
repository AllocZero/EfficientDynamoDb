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
 */

using Amazon.Runtime.Internal.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;
using System.Net;
using Amazon.Runtime.Internal;
using Amazon.Runtime;
using Amazon.Util.Internal;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;
#if AWS_ASYNC_API
using System.Threading.Tasks;
#endif
#if NETSTANDARD
using System.Net.Http;
using System.Runtime.InteropServices;
#endif

namespace Amazon.Util
{
    /// <summary>
    /// This class defines utilities and constants that can be used by 
    /// all the client libraries of the SDK.
    /// </summary>
    public static partial class AWSSDKUtils
    {
#region Internal Constants

        internal const string DefaultRegion = "us-east-1";
        internal const string DefaultGovRegion = "us-gov-west-1";

        private const char WindowsDirectorySeparatorChar = '\\';
        private const char WindowsAltDirectorySeparatorChar = '/';
        private const char WindowsVolumeSeparatorChar = ':';

        private const char SlashChar = '/';
        private const string Slash = "/";
        private const string EncodedSlash = "%2F";

        internal const int DefaultMaxRetry = 3;
        private const int DefaultConnectionLimit = 50;
        private const int DefaultMaxIdleTime = 50 * 1000; // 50 seconds

        private const int MaxIsSetMethodsCacheSize = 50;

        private static readonly Regex CompressWhitespaceRegex = new Regex("\\s+", RegexOptions.Compiled);

        public static readonly DateTime EPOCH_START = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public const int DefaultBufferSize = 8192;

        // Default value of progress update interval for streaming is 100KB.
        public const long DefaultProgressUpdateInterval = 102400;

        internal static Dictionary<int, string> RFCEncodingSchemes = new Dictionary<int, string>
        {
            { 3986,  ValidUrlCharacters },
            { 1738,  ValidUrlCharactersRFC1738 }
        };

        internal const string S3Accelerate = "s3-accelerate";
        internal const string S3Control = "s3-control";

        private const int DefaultMarshallerVersion = 1;

        private static readonly string _userAgent = InternalSDKUtils.BuildUserAgentString(string.Empty);

#endregion

#region Public Constants


        /// <summary>
        /// The user agent string header
        /// </summary>
        public const string UserAgentHeader = "User-Agent";


        /// <summary>
        /// The Set of accepted and valid Url characters per RFC3986. 
        /// Characters outside of this set will be encoded.
        /// </summary>
        public const string ValidUrlCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        /// <summary>
        /// The Set of accepted and valid Url characters per RFC1738. 
        /// Characters outside of this set will be encoded.
        /// </summary>
        public const string ValidUrlCharactersRFC1738 = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.";

        /// <summary>
        /// The set of accepted and valid Url path characters per RFC3986.
        /// </summary>
        private static string ValidPathCharacters = DetermineValidPathCharacters();

        // Checks which path characters should not be encoded
        // This set will be different for .NET 4 and .NET 4.5, as
        // per http://msdn.microsoft.com/en-us/library/hh367887%28v=vs.110%29.aspx
        private static string DetermineValidPathCharacters()
        {
            const string basePathCharacters = "/:'()!*[]$";

            var sb = new StringBuilder();
            foreach (var c in basePathCharacters)
            {
                var escaped = Uri.EscapeUriString(c.ToString());
                if (escaped.Length == 1 && escaped[0] == c)
                    sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// The string representing Url Encoded Content in HTTP requests
        /// </summary>
        public const string UrlEncodedContent = "application/x-www-form-urlencoded; charset=utf-8";

        /// <summary>
        /// The GMT Date Format string. Used when parsing date objects
        /// </summary>
        public const string GMTDateFormat = "ddd, dd MMM yyyy HH:mm:ss \\G\\M\\T";

        /// <summary>
        /// The ISO8601Date Format string. Used when parsing date objects
        /// </summary>
        public const string ISO8601DateFormat = "yyyy-MM-dd\\THH:mm:ss.fff\\Z";

        /// <summary>
        /// The ISO8601Date Format string. Used when parsing date objects
        /// </summary>
        public const string ISO8601DateFormatNoMS = "yyyy-MM-dd\\THH:mm:ss\\Z";

        /// <summary>
        /// The ISO8601 Basic date/time format string. Used when parsing date objects
        /// </summary>
        public const string ISO8601BasicDateTimeFormat = "yyyyMMddTHHmmssZ";

        /// <summary>
        /// The ISO8601 basic date format. Used during AWS4 signature computation.
        /// </summary>
        public const string ISO8601BasicDateFormat = "yyyyMMdd";

        /// <summary>
        /// The RFC822Date Format string. Used when parsing date objects
        /// </summary>
        public const string RFC822DateFormat = "ddd, dd MMM yyyy HH:mm:ss \\G\\M\\T";

#endregion



#region Internal Methods

        /// <summary>
        /// Returns an extension of a path.
        /// This has the same behavior as System.IO.Path.GetExtension, but does not
        /// check the path for invalid characters.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetExtension(string path)
        {
            if (path == null)
                return null;
            int length = path.Length;
            int index = length;

            while (--index >= 0)
            {
                char ch = path[index];
                if (ch == '.')
                {
                    if (index != length - 1)
                        return path.Substring(index, length - index);
                    else
                        return string.Empty;
                }
                else if (IsPathSeparator(ch))
                    break;
            }
            return string.Empty;
        }

        // Checks if the character is one \ / :
        private static bool IsPathSeparator(char ch)
        {
            return (ch == '\\' ||
                    ch == '/' ||
                    ch == ':');
        }

        /*
         * Determines the string to be signed based on the input parameters for
         * AWS Signature Version 2
         */
        internal static string CalculateStringToSignV2(ParameterCollection parameterCollection, string serviceUrl)
        {
            StringBuilder data = new StringBuilder("POST\n", 512);
            var sortedParameters = parameterCollection.GetSortedParametersList();
            Uri endpoint = new Uri(serviceUrl);

            data.Append(endpoint.Host);
            data.Append("\n");
            string uri = endpoint.AbsolutePath;
            if (uri == null || uri.Length == 0)
            {
                uri = "/";
            }

            data.Append(AWSSDKUtils.UrlEncode(uri, true));
            data.Append("\n");
            foreach (KeyValuePair<string, string> pair in sortedParameters)
            {
                if (pair.Value != null)
                {
                    data.Append(AWSSDKUtils.UrlEncode(pair.Key, false));
                    data.Append("=");
                    data.Append(AWSSDKUtils.UrlEncode(pair.Value, false));
                    data.Append("&");
                }
            }

            string result = data.ToString();
            return result.Remove(result.Length - 1);
        }

        /**
         * Convert request parameters to Url encoded query string
         */
        internal static string GetParametersAsString(IRequest request)
        {
            return GetParametersAsString(request.ParameterCollection);
        }

        /**
         * Convert Dictionary of parameters to Url encoded query string
         */
        internal static string GetParametersAsString(ParameterCollection parameterCollection)
        {
            var sortedParameters = parameterCollection.GetSortedParametersList();

            StringBuilder data = new StringBuilder(512);
            foreach (var kvp in sortedParameters)
            {
                var key = kvp.Key;
                var value = kvp.Value;
                if (value != null)
                {
                    data.Append(key);
                    data.Append('=');
                    data.Append(AWSSDKUtils.UrlEncode(value, false));
                    data.Append('&');
                }
            }
            string result = data.ToString();
            if (result.Length == 0)
                return string.Empty;

            return result.Remove(result.Length - 1);
        }

        /// <summary>
        /// Returns the canonicalized resource path for the service endpoint with single URL encoded path segments.
        /// </summary>
        /// <param name="endpoint">Endpoint URL for the request</param>
        /// <param name="resourcePath">Resource path for the request</param>
        /// <remarks>
        /// If resourcePath begins or ends with slash, the resulting canonicalized
        /// path will follow suit.
        /// </remarks>
        /// <returns>Canonicalized resource path for the endpoint</returns>
        public static string CanonicalizeResourcePath(Uri endpoint, string resourcePath)
        {
            // This overload is kept for backward compatibility in existing code bases.
            return CanonicalizeResourcePath(endpoint, resourcePath, false, null, DefaultMarshallerVersion);
        }

        /// <summary>
        /// Returns the canonicalized resource path for the service endpoint
        /// </summary>
        /// <param name="endpoint">Endpoint URL for the request</param>
        /// <param name="resourcePath">Resource path for the request</param>
        /// <param name="detectPreEncode">If true pre URL encode path segments if necessary.
        /// S3 is currently the only service that does not expect pre URL encoded segments.</param>
        /// <remarks>
        /// If resourcePath begins or ends with slash, the resulting canonicalized
        /// path will follow suit.
        /// </remarks>
        /// <returns>Canonicalized resource path for the endpoint</returns>
        public static string CanonicalizeResourcePath(Uri endpoint, string resourcePath, bool detectPreEncode)
        {
            // This overload is kept for backward compatibility in existing code bases.
            return CanonicalizeResourcePath(endpoint, resourcePath, detectPreEncode, null, DefaultMarshallerVersion);
        }

        /// <summary>
        /// Returns the canonicalized resource path for the service endpoint
        /// </summary>
        /// <param name="endpoint">Endpoint URL for the request</param>
        /// <param name="resourcePath">Resource path for the request</param>
        /// <param name="detectPreEncode">If true pre URL encode path segments if necessary.
        /// S3 is currently the only service that does not expect pre URL encoded segments.</param>
        /// <param name="pathResources">Dictionary of key/value parameters containing the values for the ResourcePath key replacements</param>
        /// <param name="marshallerVersion">The version of the marshaller that constructed the request object.</param>
        /// <remarks>
        /// If resourcePath begins or ends with slash, the resulting canonicalized
        /// path will follow suit.
        /// </remarks>
        /// <returns>Canonicalized resource path for the endpoint</returns>
        public static string CanonicalizeResourcePath(Uri endpoint, string resourcePath, bool detectPreEncode, IDictionary<string, string> pathResources, int marshallerVersion)
        {
            if (endpoint != null)
            {
                var path = endpoint.AbsolutePath;
                if (string.IsNullOrEmpty(path) || string.Equals(path, Slash, StringComparison.Ordinal))
                    path = string.Empty;

                if (!string.IsNullOrEmpty(resourcePath) && resourcePath.StartsWith(Slash, StringComparison.Ordinal))
                    resourcePath = resourcePath.Substring(1);

                if (!string.IsNullOrEmpty(resourcePath))
                    path = path + Slash + resourcePath;

                resourcePath = path;
            }

            if (string.IsNullOrEmpty(resourcePath))
                return Slash;

            
            
            IEnumerable<string> encodedSegments;
            if(marshallerVersion >= 2)
            {
                encodedSegments = AWSSDKUtils.SplitResourcePathIntoSegments(resourcePath, pathResources);
            }
            else
            {
                //split path at / into segments
                encodedSegments = resourcePath.Split(new char[] { SlashChar }, StringSplitOptions.None);                
            }
            
            var pathWasPreEncoded = false;
            if (detectPreEncode)
            {
                if (endpoint == null)
                    throw new ArgumentNullException(nameof(endpoint), "A non-null endpoint is necessary to decide whether or not to pre URL encode.");

                // S3 is a special case.  For S3 skip the pre encode.
                // For everything else URL pre encode the resource path segments.
                if (!S3Uri.IsS3Uri(endpoint))
                {
                    if(marshallerVersion >= 2)
                    {
                        encodedSegments = encodedSegments.Select(segment => UrlEncode(segment, true).Replace(Slash, EncodedSlash));
                    }
                    else
                    {
                        encodedSegments = encodedSegments.Select(segment => ProtectEncodedSlashUrlEncode(segment, true));
                    }
                    
                    pathWasPreEncoded = true;
                }
            }

            var canonicalizedResourcePath = string.Empty;
            if(marshallerVersion >= 2)
            {
                canonicalizedResourcePath = AWSSDKUtils.JoinResourcePathSegments(encodedSegments, false);
            }
            else
            {
                // Encode for canonicalization
                encodedSegments = encodedSegments.Select(segment => UrlEncode(segment, false));

                // join the encoded segments with /
                canonicalizedResourcePath = string.Join(Slash, encodedSegments.ToArray());
            }
            
            // Get the logger each time (it's cached) because we shouldn't store it in a static variable.
            Logger.GetLogger(typeof(AWSSDKUtils)).DebugFormat("{0} encoded {1}{2} for canonicalization: {3}",
                pathWasPreEncoded ? "Double" : "Single",
                resourcePath,
                endpoint == null ? "" : " with endpoint " + endpoint.AbsoluteUri,
                canonicalizedResourcePath);

            return canonicalizedResourcePath;
        }

        /// <summary>
        /// Splits the resourcePath at / into segments then resolves any keys with the path resource values. Greedy
        /// key values will be split into multiple segments at each /.
        /// </summary>
        /// <param name="resourcePath">The patterned resourcePath</param>
        /// <param name="pathResources">The key/value lookup for the patterned resourcePath</param>
        /// <returns>A list of path segments where all keys in the resourcePath have been resolved to one or more path segment values</returns>
        public static IEnumerable<string> SplitResourcePathIntoSegments(string resourcePath, IDictionary<string, string> pathResources)
        {
            var splitChars = new char[] { SlashChar };
            var pathSegments = resourcePath.Split(splitChars, StringSplitOptions.None);
            if(pathResources == null || pathResources.Count == 0)
            {
                return pathSegments;
            }

            //Otherwise there are key/values that need to be resolved
            var resolvedSegments = new List<string>();
            foreach(var segment in pathSegments)
            {
                if (!pathResources.ContainsKey(segment))
                {
                    resolvedSegments.Add(segment);
                    continue;
                }

                //Determine if the path is greedy. If greedy the segment will be split at each / into multiple segments.
                if (segment.EndsWith("+}", StringComparison.Ordinal))
                {
                    resolvedSegments.AddRange(pathResources[segment].Split(splitChars, StringSplitOptions.None));
                }
                else
                {
                    resolvedSegments.Add(pathResources[segment]);
                }
            }

            return resolvedSegments;
        }

        /// <summary>
        /// Joins all path segments with the / character and encodes each segment before joining.
        /// </summary>
        /// <param name="pathSegments">The segments of a URL path split at each / character</param>
        /// <param name="path">If the path property is specified,
        /// the accepted path characters {/+:} are not encoded.</param>
        /// <returns>A joined URL with encoded segments</returns>
        public static string JoinResourcePathSegments(IEnumerable<string> pathSegments, bool path)
        {
            // Encode for canonicalization
            pathSegments = pathSegments.Select(segment => UrlEncode(segment, path));

            if (path)
            {
                pathSegments = pathSegments.Select(segment => segment.Replace(Slash, EncodedSlash));
            }

            // join the encoded segments with /
            return string.Join(Slash, pathSegments.ToArray());
        }

        /// <summary>
        /// Takes a patterned resource path and resolves it using the key/value path resources into
        /// a segmented encoded URL.
        /// </summary>
        /// <param name="resourcePath">The patterned resourcePath</param>
        /// <param name="pathResources">The key/value lookup for the patterned resourcePath</param>
        /// <returns>A segmented encoded URL</returns>
        public static string ResolveResourcePath(string resourcePath, IDictionary<string, string> pathResources)
        {
            if (string.IsNullOrEmpty(resourcePath))
            {
                return resourcePath;
            }

            return JoinResourcePathSegments(SplitResourcePathIntoSegments(resourcePath, pathResources), true);
        }

        /// <summary>
        /// Returns a new string created by joining each of the strings in the
        /// specified list together, with a comma between them.
        /// </summary>
        /// <parma name="strings">The list of strings to join into a single, comma delimited
        /// string list.</parma>
        /// <returns> A new string created by joining each of the strings in the
        /// specified list together, with a comma between strings.</returns>
        public static String Join(List<String> strings)
        {
            StringBuilder result = new StringBuilder();

            Boolean first = true;
            foreach (String s in strings)
            {
                if (!first) result.Append(", ");

                result.Append(s);
                first = false;
            }

            return result.ToString();
        }

        /// <summary>
        /// Attempt to infer the region for a service request based on the endpoint
        /// </summary>
        /// <param name="url">Endpoint to the service to be called</param>
        /// <returns>
        /// Region parsed from the endpoint; DefaultRegion (or DefaultGovRegion)
        /// if it cannot be determined/is not explicit
        /// </returns>
        public static string DetermineRegion(string url)
        {
            var regionEndpoint = RegionFinder.Instance.FindRegion(url);
            return regionEndpoint?.RegionName;
        }

        /// <summary>
        /// Attempt to infer the service name for a request (in short form, eg 'iam') from the
        /// service endpoint.
        /// </summary>
        /// <param name="url">Endpoint to the service to be called</param>
        /// <returns>
        /// Short-form name of the service parsed from the endpoint; empty string if it cannot 
        /// be determined
        /// </returns>
        public static string DetermineService(string url)
        {
            int delimIndex = url.IndexOf("//", StringComparison.Ordinal);
            if (delimIndex >= 0)
                url = url.Substring(delimIndex + 2);

            string[] urlParts = url.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (urlParts == null || urlParts.Length == 0)
                return string.Empty;

            string servicePart = urlParts[0];
            int hyphenated = servicePart.IndexOf('-');
            string service;
            if (hyphenated < 0)
            { service = servicePart; }
            else
            { service = servicePart.Substring(0, hyphenated); }

            // Check for SQS : return "sqs" incase service is determined to be "queue" as per the URL.
            if (service.Equals("queue"))
            {
                return "sqs";
            }
            else
            {
                return service;
            }
        }

        /// <summary>
        /// Utility method for converting Unix epoch seconds to DateTime structure.
        /// </summary>
        /// <param name="seconds">The number of seconds since January 1, 1970.</param>
        /// <returns>Converted DateTime structure</returns>
        public static DateTime ConvertFromUnixEpochSeconds(int seconds)
        {
            return new DateTime(seconds * 10000000L + EPOCH_START.Ticks, DateTimeKind.Utc).ToLocalTime();
        }

        public static int ConvertToUnixEpochSeconds(DateTime dateTime)
        {
            return Convert.ToInt32(GetTimeSpanInTicks(dateTime).TotalSeconds);
        }

        public static string ConvertToUnixEpochSecondsString(DateTime dateTime)
        {
            return Convert.ToInt64(GetTimeSpanInTicks(dateTime).TotalSeconds).ToString(CultureInfo.InvariantCulture);
        }

        [Obsolete("This method isn't named correctly: it returns seconds instead of milliseconds. Use ConvertToUnixEpochSecondsDouble instead.", false)]
        public static double ConvertToUnixEpochMilliSeconds(DateTime dateTime)
        {
            return ConvertToUnixEpochSecondsDouble(dateTime);
        }

        public static double ConvertToUnixEpochSecondsDouble(DateTime dateTime)
        {
            return Math.Round(GetTimeSpanInTicks(dateTime).TotalMilliseconds, 0) / 1000.0;
        }

        public static TimeSpan GetTimeSpanInTicks(DateTime dateTime)
        {
            return new TimeSpan(dateTime.ToUniversalTime().Ticks - EPOCH_START.Ticks);
        }

        public static long ConvertDateTimetoMilliseconds(DateTime dateTime)
        {
            return ConvertTimeSpanToMilliseconds(GetTimeSpanInTicks(dateTime));
        }

        public static long ConvertTimeSpanToMilliseconds(TimeSpan timeSpan)
        {
            return timeSpan.Ticks / TimeSpan.TicksPerMillisecond;
        }

        /// <summary>
        /// Helper function to format a byte array into string
        /// </summary>
        /// <param name="data">The data blob to process</param>
        /// <param name="lowercase">If true, returns hex digits in lower case form</param>
        /// <returns>String version of the data</returns>
        public static string ToHex(byte[] data, bool lowercase)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString(lowercase ? "x2" : "X2", CultureInfo.InvariantCulture));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Calls a specific EventHandler in a background thread
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="args"></param>
        /// <param name="sender"></param>
        public static void InvokeInBackground<T>(EventHandler<T> handler, T args, object sender) where T : EventArgs
        {
            if (handler == null) return;


            var list = handler.GetInvocationList();
            foreach (var call in list)
            {
                var eventHandler = ((EventHandler<T>)call);
                if (eventHandler != null)
                {
                    Dispatcher.Dispatch(() => eventHandler(sender, args));
                }
            }
        }

        private static BackgroundInvoker _dispatcher;

        private static BackgroundInvoker Dispatcher
        {
            get
            {
                if (_dispatcher == null)
                {
                    _dispatcher = new BackgroundInvoker();
                }

                return _dispatcher;
            }
        }

        /// <summary>
        /// Parses a query string of a URL and returns the parameters as a string-to-string dictionary.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ParseQueryParameters(string url)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(url))
            {
                int queryIndex = url.IndexOf('?');
                if (queryIndex >= 0)
                {
                    string queryString = url.Substring(queryIndex + 1);
                    string[] kvps = queryString.Split(new char[] { '&' }, StringSplitOptions.None);
                    foreach (string kvp in kvps)
                    {
                        if (string.IsNullOrEmpty(kvp))
                            continue;

                        string[] nameValuePair = kvp.Split(new char[] { '=' }, 2);
                        string name = nameValuePair[0];
                        string value = nameValuePair.Length == 1 ? null : nameValuePair[1];
                        parameters[name] = value;
                    }
                }
            }

            return parameters;
        }

        internal static bool AreEqual(object[] itemsA, object[] itemsB)
        {
            if (itemsA == null || itemsB == null)
                return (itemsA == itemsB);

            if (itemsA.Length != itemsB.Length)
                return false;

            var length = itemsA.Length;
            for (int i = 0; i < length; i++)
            {
                var a = itemsA[i];
                var b = itemsB[i];
                if (!AreEqual(a, b))
                    return false;
            }

            return true;
        }

        internal static bool AreEqual(object a, object b)
        {
            if (a == null || b == null)
                return (a == b);

            if (object.ReferenceEquals(a, b))
                return true;

            return (a.Equals(b));
        }

        internal static bool DictionariesAreEqual<K,V>(Dictionary<K, V> a, Dictionary<K, V> b)
        {
            if (a == null || b == null)
                return (a == b);

            if (object.ReferenceEquals(a, b))
                return true;

            return a.Count == b.Count && !a.Except(b).Any();
        }

        /// <summary>
        /// Utility method for converting a string to a MemoryStream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static MemoryStream GenerateMemoryStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Utility method for copy the contents of the source stream to the destination stream.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static void CopyStream(Stream source, Stream destination)
        {
            CopyStream(source, destination, DefaultBufferSize);
        }

        /// <summary>
        /// Utility method for copy the contents of the source stream to the destination stream.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="bufferSize"></param>
        public static void CopyStream(Stream source, Stream destination, int bufferSize)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");
            if (bufferSize <= 0)
                throw new ArgumentOutOfRangeException("bufferSize");

            byte[] array = new byte[bufferSize];
            int count;
            while ((count = source.Read(array, 0, array.Length)) != 0)
            {
                destination.Write(array, 0, count);
            }
        }
#endregion

#region Public Methods and Properties

        /// <summary>
        /// Formats the current date as a GMT timestamp
        /// </summary>
        /// <returns>A GMT formatted string representation
        /// of the current date and time
        /// </returns>
        public static string FormattedCurrentTimestampGMT
        {
            get
            {
#pragma warning disable CS0618 // Type or member is obsolete
                DateTime dateTime = AWSSDKUtils.CorrectedUtcNow;
#pragma warning restore CS0618 // Type or member is obsolete
                return dateTime.ToString(GMTDateFormat, CultureInfo.InvariantCulture);
            }
        }



        /// <summary>
        /// Formats the current date as ISO 8601 timestamp
        /// </summary>
        /// <returns>An ISO 8601 formatted string representation
        /// of the current date and time
        /// </returns>
        public static string FormattedCurrentTimestampISO8601
        {
            get
            {
                return GetFormattedTimestampISO8601(0);
            }
        }

        /// <summary>
        /// Gets the ISO8601 formatted timestamp that is minutesFromNow
        /// in the future.
        /// </summary>
        /// <param name="minutesFromNow">The number of minutes from the current instant
        /// for which the timestamp is needed.</param>
        /// <returns>The ISO8601 formatted future timestamp.</returns>
        public static string GetFormattedTimestampISO8601(int minutesFromNow)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            return GetFormattedTimestampISO8601(AWSSDKUtils.CorrectedUtcNow.AddMinutes(minutesFromNow));
#pragma warning restore CS0618 // Type or member is obsolete
        }

        internal static string GetFormattedTimestampISO8601(IClientConfig config)
        {
            return GetFormattedTimestampISO8601(config.CorrectedUtcNow);
        }

        private static string GetFormattedTimestampISO8601(DateTime dateTime)
        {
            DateTime formatted = new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                dateTime.Hour,
                dateTime.Minute,
                dateTime.Second,
                dateTime.Millisecond,
                DateTimeKind.Local
                );
            return formatted.ToString(
                AWSSDKUtils.ISO8601DateFormat,
                CultureInfo.InvariantCulture
                );
        }

        /// <summary>
        /// Formats the current date as ISO 8601 timestamp
        /// </summary>
        /// <returns>An ISO 8601 formatted string representation
        /// of the current date and time
        /// </returns>
        public static string FormattedCurrentTimestampRFC822
        {
            get
            {
                return GetFormattedTimestampRFC822(0);
            }
        }

        /// <summary>
        /// Gets the RFC822 formatted timestamp that is minutesFromNow
        /// in the future.
        /// </summary>
        /// <param name="minutesFromNow">The number of minutes from the current instant
        /// for which the timestamp is needed.</param>
        /// <returns>The ISO8601 formatted future timestamp.</returns>
        public static string GetFormattedTimestampRFC822(int minutesFromNow)
        {
#pragma warning disable CS0612 // Type or member is obsolete
            DateTime dateTime = AWSSDKUtils.CorrectedUtcNow.AddMinutes(minutesFromNow);
#pragma warning restore CS0612 // Type or member is obsolete
            return dateTime.ToString(AWSSDKUtils.RFC822DateFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Determines whether the given string is an absolute path to a root.
        /// </summary>
        /// <param name="path">The hypothetical absolute path.</param>
        /// <returns>True if <paramref name="path"/> is an absolute path.</returns>
        public static bool IsAbsolutePath(string path)
        {
            return IsWindows() ? !IsPartiallyQualifiedForWindows(path) : Path.IsPathRooted(path);
        }

        private static bool IsWindows()
        {
#if NETSTANDARD
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif
            return true;
        }

        #region The code in this region has been minimally adapted from Microsoft's PathInternal.Windows.cs class as of 11/19/2019.  The logic remains the same.
        /// <summary>
        /// Returns true if the path specified is relative to the current drive or working directory.
        /// Returns false if the path is fixed to a specific drive or UNC path.  This method does no
        /// validation of the path (URIs will be returned as relative as a result).
        /// </summary>
        /// <remarks>
        /// Handles paths that use the alternate directory separator.  It is a frequent mistake to
        /// assume that rooted paths (Path.IsPathRooted) are not relative.  This isn't the case.
        /// "C:a" is drive relative- meaning that it will be resolved against the current directory
        /// for C: (rooted, but relative). "C:\a" is rooted and not relative (the current directory
        /// will not be used to modify the path).
        /// </remarks>
        private static bool IsPartiallyQualifiedForWindows(string path)
        {
            if (path.Length < 2)
            {
                // It isn't fixed, it must be relative.  There is no way to specify a fixed
                // path with one character (or less).
                return true;
            }

            if (IsWindowsDirectorySeparator(path[0]))
            {
                // There is no valid way to specify a relative path with two initial slashes or
                // \? as ? isn't valid for drive relative paths and \??\ is equivalent to \\?\
                return !(path[1] == '?' || IsWindowsDirectorySeparator(path[1]));
            }

            // The only way to specify a fixed path that doesn't begin with two slashes
            // is the drive, colon, slash format- i.e. C:\
            return !((path.Length >= 3)
                && (path[1] == WindowsVolumeSeparatorChar)
                && IsWindowsDirectorySeparator(path[2])
                // To match old behavior we'll check the drive character for validity as the path is technically
                // not qualified if you don't have a valid drive. "=:\" is the "=" file's default data stream.
                && IsValidWindowsDriveChar(path[0]));
        }

        /// <summary>
        /// True if the given character is a directory separator.
        /// </summary>
        private static bool IsWindowsDirectorySeparator(char c)
        {
            return c == WindowsDirectorySeparatorChar || c == WindowsAltDirectorySeparatorChar;
        }

        /// <summary>
        /// Returns true if the given character is a valid drive letter
        /// </summary>
        private static bool IsValidWindowsDriveChar(char value)
        {
            return (value >= 'A' && value <= 'Z') || (value >= 'a' && value <= 'z');
        }
        #endregion The code in this region has been minimally adapted from Microsoft's PathInternal.Windows.cs class as of 11/19/2019.  The logic remains the same.

        /// <summary>
        /// URL encodes a string per RFC3986. If the path property is specified,
        /// the accepted path characters {/+:} are not encoded.
        /// </summary>
        /// <param name="data">The string to encode</param>
        /// <param name="path">Whether the string is a URL path or not</param>
        /// <returns>The encoded string</returns>
        public static string UrlEncode(string data, bool path)
        {
            return UrlEncode(3986, data, path);
        }

        /// <summary>
        /// URL encodes a string per the specified RFC. If the path property is specified,
        /// the accepted path characters {/+:} are not encoded.
        /// </summary>
        /// <param name="rfcNumber">RFC number determing safe characters</param>
        /// <param name="data">The string to encode</param>
        /// <param name="path">Whether the string is a URL path or not</param>
        /// <returns>The encoded string</returns>
        /// <remarks>
        /// Currently recognised RFC versions are 1738 (Dec '94) and 3986 (Jan '05). 
        /// If the specified RFC is not recognised, 3986 is used by default.
        /// </remarks>
        public static string UrlEncode(int rfcNumber, string data, bool path)
        {
            StringBuilder encoded = new StringBuilder(data.Length * 2);
            string validUrlCharacters;
            if (!RFCEncodingSchemes.TryGetValue(rfcNumber, out validUrlCharacters))
                validUrlCharacters = ValidUrlCharacters;

            string unreservedChars = String.Concat(validUrlCharacters, (path ? ValidPathCharacters : ""));

            foreach (char symbol in System.Text.Encoding.UTF8.GetBytes(data))
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                {
                    encoded.Append(symbol);
                }
                else
                {
                    encoded.Append("%").Append(string.Format(CultureInfo.InvariantCulture, "{0:X2}", (int)symbol));
                }
            }

            return encoded.ToString();
        }
                
        internal static string UrlEncodeSlash(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return data;
            }

            return data.Replace("/", EncodedSlash);
        }

        /// <summary>
        /// URL encodes a string per the specified RFC with the exception of preserving the encoding of previously encoded slashes.
        /// If the path property is specified, the accepted path characters {/+:} are not encoded. 
        /// </summary>
        /// <param name="data">The string to encode</param>
        /// <param name="path">Whether the string is a URL path or not</param>
        /// <returns>The encoded string with any previously encoded %2F preserved</returns>        
        public static string ProtectEncodedSlashUrlEncode(string data, bool path)
        {
            if (string.IsNullOrEmpty(data))
            {
                return data;
            }

            var index = 0;                                    
            var sb = new StringBuilder();
            var findIndex = data.IndexOf(EncodedSlash, index, StringComparison.OrdinalIgnoreCase);
            while (findIndex != -1)
            {
                sb.Append(UrlEncode(data.Substring(index, findIndex - index), path));
                sb.Append(EncodedSlash);
                index = findIndex + EncodedSlash.Length;
                findIndex = data.IndexOf(EncodedSlash, index, StringComparison.OrdinalIgnoreCase);
            }            

            //If encoded slash was not found return the original data
            if(index == 0)
            {
                return UrlEncode(data, path);
            }

            if(data.Length > index)
            {
                sb.Append(UrlEncode(data.Substring(index), path));
            }
            
            return sb.ToString();
        }
        
        public static void Sleep(TimeSpan ts)
        {
            Sleep((int)ts.TotalMilliseconds);
        }

        /// <summary>
        /// Convert bytes to a hex string
        /// </summary>
        /// <param name="value">Bytes to convert.</param>
        /// <returns>Hexadecimal string representing the byte array.</returns>
        public static string BytesToHexString(byte[] value)
        {
            string hex = BitConverter.ToString(value);
            hex = hex.Replace("-", string.Empty);
            return hex;
        }

        /// <summary>
        /// Convert a hex string to bytes
        /// </summary>
        /// <param name="hex">Hexadecimal string</param>
        /// <returns>Byte array corresponding to the hex string.</returns>
        public static byte[] HexStringToBytes(string hex)
        {
            if (string.IsNullOrEmpty(hex) || hex.Length % 2 == 1)
                throw new ArgumentOutOfRangeException("hex");

            int count = 0;
            byte[] buffer = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                string sub = hex.Substring(i, 2);
                byte b = Convert.ToByte(sub, 16);
                buffer[count] = b;
                count++;
            }

            return buffer;
        }

        /// <summary>
        /// Returns DateTime.UtcNow + ManualClockCorrection when
        /// <seealso cref="AWSConfigs.ManualClockCorrection"/> is set.
        /// This value should be used instead of DateTime.UtcNow to factor in manual clock correction
        /// </summary>
        [Obsolete("This property does not account for endpoint specific clock skew.  Use CorrectClockSkew.GetCorrectedUtcNowForEndpoint() instead.")]
        public static DateTime CorrectedUtcNow
        {
            get
            {
                var now = AWSConfigs.utcNowSource();
                if (AWSConfigs.ManualClockCorrection.HasValue)
                    now += AWSConfigs.ManualClockCorrection.Value;
                return now;
            }
        }

        /// <summary>
        /// Returns true if the string has any bidirectional control characters.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool HasBidiControlCharacters(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            foreach (var c in input)
            {
                if (IsBidiControlChar(c))
                    return true;
            }
            return false;
        }
        private static bool IsBidiControlChar(char c)
        {
            // check general range
            if (c < '\u200E' || c > '\u202E')
                return false;

            // check specific characters
            return (
                c == '\u200E' || // LRM
                c == '\u200F' || // RLM
                c == '\u202A' || // LRE
                c == '\u202B' || // RLE
                c == '\u202C' || // PDF
                c == '\u202D' || // LRO
                c == '\u202E'    // RLO
            );
        }

        public static string DownloadStringContent(Uri uri)
        {
            return DownloadStringContent(uri, TimeSpan.Zero, null);
        }

        public static string DownloadStringContent(Uri uri, TimeSpan timeout)
        {
            return DownloadStringContent(uri, timeout, null);
        }

        public static string DownloadStringContent(Uri uri, IWebProxy proxy)
        {
            return DownloadStringContent(uri, TimeSpan.Zero, proxy);
        }                

        public static string DownloadStringContent(Uri uri, TimeSpan timeout, IWebProxy proxy)
        {
#if NETSTANDARD
            using (var client = CreateClient(uri, timeout, proxy, null))
            {
                return AsyncHelpers.RunSync<string>(() =>
                {
                    return client.GetStringAsync(uri);
                });             
            }
#else
            var request = CreateClient(uri, timeout, proxy, null);

            using (var response = request.GetResponse() as HttpWebResponse)
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
#endif
        }

        /// <summary>
        /// Executes an HTTP request and returns the response as a string. This method
        /// throws WebException and HttpRequestException. In the event HttpRequestException
        /// is thrown the StatusCode is sent as user defined data on the exception under
        /// the key "StatusCode".
        /// </summary>
        /// <param name="uri">The URI to make the request to</param>
        /// <param name="requestType">The request type: GET, PUT, POST</param>
        /// <param name="content">null or the content to send with the request</param>
        /// <param name="timeout">Timeout for the request</param>
        /// <param name="proxy">Proxy for the request</param>
        /// <param name="headers">null or any headers to send with the request</param>
        /// <returns>The response as a string.</returns>
        public static string ExecuteHttpRequest(Uri uri, string requestType, string content, TimeSpan timeout, IWebProxy proxy, IDictionary<string, string> headers)
        {
#if NETSTANDARD
            using (var client = CreateClient(uri, timeout, proxy, headers))
            {                   
                var response = AsyncHelpers.RunSync<HttpResponseMessage>(() =>
                {
                    var requestMessage = new HttpRequestMessage(new HttpMethod(requestType), uri);
                    if(!string.IsNullOrEmpty(content))
                    {
                        requestMessage.Content = new StringContent(content);                     
                    }
                                
                    return client.SendAsync(requestMessage);                    
                });

                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch(HttpRequestException e)
                {
                    var httpRequestException = new HttpRequestException(e.Message, e);
                    httpRequestException.Data.Add(nameof(response.StatusCode), response.StatusCode);

                    response.Dispose();
                    throw httpRequestException;
                }
                            
                try
                {
                    return AsyncHelpers.RunSync<string>(() =>
                    {                    
                        return response.Content.ReadAsStringAsync();                    
                    });
                }
                finally 
                {
                    response.Dispose();
                }
            }
#else
            var request = CreateClient(uri, timeout, proxy, headers);
            request.Method = requestType;
            request.ContentLength = 0;
                        
            if (!string.IsNullOrEmpty(content))
            {
                var contentBytes = Encoding.UTF8.GetBytes(content);
                request.ContentLength = contentBytes.Length;
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(contentBytes, 0, contentBytes.Length);
                }                
            }
                        
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }            
#endif
        }


#if NETSTANDARD
        private static HttpClient CreateClient(Uri uri, TimeSpan timeout, IWebProxy proxy, IDictionary<string, string> headers)
        {
            var client = new HttpClient(new System.Net.Http.HttpClientHandler() { Proxy = proxy });
            
            if (timeout > TimeSpan.Zero)
            {
                client.Timeout = timeout;
            }
                
            //DefaultRequestHeaders should not be used if we reuse the HttpClient. It is currently created for each request.
            client.DefaultRequestHeaders.TryAddWithoutValidation(UserAgentHeader, _userAgent);
            if(headers != null)
            {
                foreach(var nameValue in headers)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(nameValue.Key, nameValue.Value);
                }
            }
                                
            return client;            
        }
#else
        private static HttpWebRequest CreateClient(Uri uri, TimeSpan timeout, IWebProxy proxy, IDictionary<string, string> headers)
        { 
            HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
            request.Proxy = proxy ?? WebRequest.DefaultWebProxy;

            if (timeout > TimeSpan.Zero)
            {
                request.Timeout = (int)timeout.TotalMilliseconds;
            }

            request.UserAgent = _userAgent;
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            return request;
        }
#endif


        public static Stream OpenStream(Uri uri)
        {
            return OpenStream(uri, null);
        }

        public static Stream OpenStream(Uri uri, IWebProxy proxy)
        {
#if NETSTANDARD
            using (var client = new System.Net.Http.HttpClient(new System.Net.Http.HttpClientHandler() { Proxy = proxy }))
            {
                var task = client.GetStreamAsync(uri);
                return task.Result;
            }
#else
            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
            request.Proxy = proxy ?? WebRequest.DefaultWebProxy;
            var asynResult = request.BeginGetResponse(null, null);
            HttpWebResponse response = request.EndGetResponse(asynResult) as HttpWebResponse;
            return response.GetResponseStream();
#endif
        }

        /// <summary>
        /// Utility method that accepts a string and replaces white spaces with a space.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string CompressSpaces(string data)
        {
            if (data == null)
            {
                return null;
            }

            if (data.Length == 0)
            {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();
            var isWhiteSpace = false;
            foreach (var character in data)
            {
                if (!isWhiteSpace | !(isWhiteSpace = char.IsWhiteSpace(character)))
                {
                    stringBuilder.Append(isWhiteSpace ? ' ' : character);
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Runs a process with the below input parameters.
        /// </summary>
        /// <param name="processStartInfo"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public static ProcessExecutionResult RunProcess(ProcessStartInfo processStartInfo)
        {
            using (var process = new Process
            {
                StartInfo = processStartInfo
            })
            {
                var logger = Logger.GetLogger(typeof(AWSSDKUtils));
                logger.InfoFormat("Starting a process with the following ProcessInfo: UseShellExecute - {0} RedirectStandardError - {1}, RedirectStandardOutput - {2}, CreateNoWindow - {3}",  
                    processStartInfo.UseShellExecute, processStartInfo.RedirectStandardError, processStartInfo.RedirectStandardOutput, processStartInfo.CreateNoWindow);
                process.Start();
                logger.InfoFormat("Process started");
                string standardOutput = null;
                var thread = new Thread(() => standardOutput = process.StandardOutput.ReadToEnd());
                thread.Start();
                var standardError = process.StandardError.ReadToEnd();
                thread.Join();
                process.WaitForExit();                

                return new ProcessExecutionResult
                {
                    ExitCode = process.ExitCode,
                    StandardError = standardError,
                    StandardOutput = standardOutput
                };                
            }
        }
#if AWS_ASYNC_API
        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public static async Task<ProcessExecutionResult> RunProcessAsync(ProcessStartInfo processStartInfo)
        {
            var logger = Logger.GetLogger(typeof(AWSSDKUtils));
            using (var process = new Process
            {
                StartInfo = processStartInfo,
                EnableRaisingEvents = true
            })
            {
                var tcs = new TaskCompletionSource<object>();
                process.Exited += (s, ea) => tcs.SetResult(null);
                logger.InfoFormat("Starting a process with the following ProcessInfo: UseShellExecute - {0} RedirectStandardError - {1}, RedirectStandardOutput - {2}, CreateNoWindow - {3}",  
                    processStartInfo.UseShellExecute, processStartInfo.RedirectStandardError, processStartInfo.RedirectStandardOutput, processStartInfo.CreateNoWindow);                
                process.Start();
                logger.InfoFormat("Process started");

                var standardErrorTask = process.StandardError.ReadToEndAsync();
                var standardOutputTask = process.StandardOutput.ReadToEndAsync();

                await Task.WhenAll(tcs.Task, standardErrorTask, standardOutputTask).ConfigureAwait(false);

                return new ProcessExecutionResult
                {
                    ExitCode = process.ExitCode,
                    StandardError = standardErrorTask.Result,
                    StandardOutput = standardOutputTask.Result
                };
            }

        }
#endif

#if !NETSTANDARD13
        /// <summary>
        /// This method allows to check whether a property of an object returned by a service call
        /// is set. This method is needed to discriminate whether a field is not set (not present in
        /// the service response) or if it is set to the default value for its type. Using this
        /// method is not required for nullable properties (non-ValueType and Nullable) because
        /// they will be simply set to null when not included in the service response.
        /// This method can also be used on objects used as part of service requests.
        /// This method doesn't support objects that are part of the S3 service model.
        /// </summary>
        /// <param name="awsServiceObject">An object that is used in an AWS service request or is
        /// returned as part of an AWS service response.</param>
        /// <param name="propertyName">The name of the property of awsServiceObject to check.</param>
        /// <returns>True if the property is set, otherwise false.</returns>
        public static bool IsPropertySet(object awsServiceObject, string propertyName)
        {
            var type = awsServiceObject.GetType();
            var nameSpace = type.Namespace;

            if (!nameSpace.StartsWith("Amazon.", StringComparison.Ordinal) || !nameSpace.EndsWith(".Model", StringComparison.Ordinal))
            {
                throw new ArgumentException("IsPropertySet can be used only on Amazon Model classes");
            }
            else if (nameSpace == "Amazon.S3.Model")
            {
                throw new ArgumentException("IsPropertySet doesn't support S3");
            }

            var key = new IsSetMethodsCacheKey(type, propertyName);

            //We cache the result of GetIsPropertySetMethodInfo even if it is null
            var method = IsSetMethodsCache.GetOrAdd(key, (k) => k.Type.GetMethod("IsSet" + k.PropertyName,
                                                                                 BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                                                                                 null,
                                                                                 new Type[0],
                                                                                 new ParameterModifier[0]));
            if (method == null)
            {
                throw new ArgumentException("Could not find an IsSet method for property " + key);
            }

            object result = method.Invoke(awsServiceObject, new object[0]);

            if (!(result is bool))
            {
                throw new ArgumentException("The IsSet method for property " + key + " didn't return a bool");
            }

            return (bool)result;
        }
#endif

        #endregion

        #region Private Methods, Static Fields and Classes

#if !NETSTANDARD13
        private static LruCache<IsSetMethodsCacheKey, MethodInfo> IsSetMethodsCache = new LruCache<IsSetMethodsCacheKey, MethodInfo>(MaxIsSetMethodsCacheSize);

        private class IsSetMethodsCacheKey
        {
            public readonly Type Type;
            public readonly string PropertyName;
            public IsSetMethodsCacheKey(Type type, string propertyName)
            {
                Type = type;
                PropertyName = propertyName;
            }
            public override bool Equals(object other)
            {
                var otherKey = other as IsSetMethodsCacheKey;
                if (otherKey == null)
                {
                    return false;
                }
                return Type == otherKey.Type && PropertyName == otherKey.PropertyName;
            }

            public override int GetHashCode()
            {
                return Type.GetHashCode() ^ PropertyName.GetHashCode();
            }

            public override string ToString()
            {
                return Type.FullName + "." + PropertyName;
            }
        }
#endif
#endregion
    }

    public class JitteredDelay
    {
        private TimeSpan _maxDelay;
        private TimeSpan _variance;
        private TimeSpan _baseIncrement;
        private Random _rand = null;
        private int _count = 0;

        public JitteredDelay(TimeSpan baseIncrement, TimeSpan variance)
            : this(baseIncrement, variance, new TimeSpan(0, 0, 30))
        {
        }

        public JitteredDelay(TimeSpan baseIncrement, TimeSpan variance, TimeSpan maxDelay)
        {
            _baseIncrement = baseIncrement;
            _variance = variance;
            _maxDelay = maxDelay;
            _rand = new System.Random();
        }

        public TimeSpan GetRetryDelay(int attemptCount)
        {
            long ticks = (_baseIncrement.Ticks * (long)Math.Pow(2, attemptCount) + (long)(_rand.NextDouble() * _variance.Ticks));
            return new TimeSpan(ticks);
        }

        public TimeSpan Next()
        {
            long nextTick = GetRetryDelay(_count + 1).Ticks;
            if (nextTick < _maxDelay.Ticks)
            {
                _count++;
            }
            else
            {
                nextTick = _maxDelay.Ticks;
            }
            return new TimeSpan(nextTick);
        }

        public void Reset()
        {
            _count = 0;
        }
    }
}
