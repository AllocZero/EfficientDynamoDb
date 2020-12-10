using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using EfficientDynamoDb.Context.Config;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Signing.Utils;

namespace EfficientDynamoDb.Internal.Signing.Builders
{
    internal static class CanonicalRequestBuilder
    {
        /// <summary>
        /// Gets or sets the header value separator. The default value is ", " and it is defined in
        /// <see href="https://github.com/dotnet/corefx/blob/master/src/System.Net.Http/src/System/Net/Http/Headers/HttpHeaderParser.cs">
        /// HttpHeaderParser</see> in the .NET source code. It is used when serializing a header
        /// with multiple values into a HTTP request. For some other languages this separator is
        /// plainly ",", but Microsoft has chosen to go with ", ".
        /// </summary>
        private static string HeaderValueSeparator { get; } = ", ";

        /// <returns>
        /// The first value is the canonical request, the second value is the signed headers.
        /// </returns>
        public static void Build(HttpRequestMessage request, in Span<byte> contentHash, in SigningMetadata metadata, ref NoAllocStringBuilder builder, ref NoAllocStringBuilder signedHeadersBuilder)
        {
            // var builder = new StringBuilder();

            // The HTTP request method (GET, PUT, POST, etc.), followed by a newline character
            builder.Append(request.Method.ToString()); // No allocation, returns existing string value
            builder.Append('\n');

            // Add the canonical URI parameter, followed by a newline character. The canonical URI
            // is the URI-encoded version of the absolute path component of the URI, which is
            // everything in the URI from the HTTP host to the question mark character ("?") that
            // begins the query string parameters (if any).
            //
            // Normalize URI paths according to <see href="https://tools.ietf.org/html/rfc3986">RFC
            // 3986</see>. Remove redundant and relative path components. Each path segment must be
            // URI-encoded twice (
            // <see href="https://docs.aws.amazon.com/AmazonS3/latest/API/sigv4-query-string-auth.html">
            // except for Amazon S3 which only gets URI-encoded once</see>).
            // For dynamodb url is always the same
            builder.Append('/');
            builder.Append('\n');
            // AppendCanonicalResourcePath(ref builder, RegionEndpoint.ServiceName, request.RequestUri);

            // Add the canonical query string, followed by a newline character. If the request does
            // not include a query string, use an empty string (essentially, a blank line).
            //
            // To construct the canonical query string, complete the following steps:
            //
            // a. Sort the parameter names by character code point in ascending order. Parameters
            //    with duplicate names should be sorted by value. For example, a parameter name
            //    that begins with the uppercase letter F precedes a parameter name that begins
            //    with a lowercase letter b.
            // b. URI-encode each parameter name and value according to the following rules:
            //    - Do not URI-encode any of the unreserved characters that RFC 3986 defines: A-Z,
            //      a-z, 0-9, hyphen ( - ), underscore ( _ ), period ( . ), and tilde ( ~ ).
            //    - Percent-encode all other characters with %XY, where X and Y are hexadecimal
            //      characters (0-9 and uppercase A-F). For example, the space character must be
            //      encoded as %20 (not using '+', as some encoding schemes do) and extended UTF-8
            //      characters must be in the form %XY%ZA%BC.
            // c. Build the canonical query string by starting with the first parameter name in the
            //    sorted list.
            // d. For each parameter, append the URI-encoded parameter name, followed by the equals
            //    sign character (=), followed by the URI-encoded parameter value. Use an empty
            //    string for parameters that have no value.
            // e. Append the ampersand character (&) after each parameter value, except for the
            //    last value in the list.
            AppendSortedQueryString(ref builder, request.RequestUri.Query);

            // Add the canonical headers, followed by a newline character. The canonical headers
            // consist of a list of all the HTTP headers that you are including with the signed
            // request.
            //
            // To create the canonical headers list, convert all header names to lowercase and
            // remove leading spaces and trailing spaces. Convert sequential spaces in the header
            // value to a single space.
            //
            // Build the canonical headers list by sorting the (lowercase) headers by character
            // code and then iterating through the header names. Construct each header according to
            // the following rules:
            //
            // - Append the lowercase header name followed by a colon.
            // - Append a comma-separated list of values for that header. Do not sort the values in
            //   headers that have multiple values.
            //   PLEASE NOTE: Microsoft has chosen to separate the header values with ", ", not ","
            //   as defined by the Canonical Request algorithm.
            // - Append a new line ('\n').

            if (metadata.DefaultRequestHeaders.Any())
                AppendSortedHeadersFromRequest(ref builder, ref signedHeadersBuilder, request.Headers, metadata.DefaultRequestHeaders);
            else
                AppendStaticSortedHeaders(ref builder, ref signedHeadersBuilder, request, in metadata);

            builder.Append('\n');

            // Add the signed headers, followed by a newline character. This value is the list of
            // headers that you included in the canonical headers. By adding this list of headers,
            // you tell AWS which headers in the request are part of the signing process and which
            // ones AWS can ignore (for example, any additional headers added by a proxy) for
            // purposes of validating the request.
            //
            // To create the signed headers list, convert all header names to lowercase, sort them
            // by character code, and use a semicolon to separate the header names.
            //
            // Build the signed headers list by iterating through the collection of header names,
            // sorted by lowercase character code. For each header name except the last, append a
            // semicolon (';') to the header name to separate it from the following header name.
            builder.Append(signedHeadersBuilder.GetBuffer());
            builder.Append('\n');

            // Use a hash (digest) function like SHA256 to create a hashed value from the payload
            // in the body of the HTTP or HTTPS request.
            //
            // If the payload is empty, use an empty string as the input to the hash function.
            foreach (var item in contentHash)
            {
                builder.Append(HexAlphabet.Lowercase[item >> 4]);
                builder.Append(HexAlphabet.Lowercase[item & 0xF]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AppendStaticSortedHeaders(ref NoAllocStringBuilder builder, ref NoAllocStringBuilder signedHeadersBuilder, HttpRequestMessage request, in SigningMetadata metadata)
        {
            builder.Append("host:");
            builder.Append(request.Headers.GetValues("host").First());
            builder.Append('\n');
            builder.Append("x-amz-date:");
            builder.Append(request.Headers.GetValues("x-amz-date").First());
            builder.Append('\n');

            signedHeadersBuilder.Append("host;x-amz-date");

            if (!metadata.Credentials.UseToken || metadata.Credentials.Token == null)
                return;
            
            builder.Append(HeaderKeys.XAmzSecurityTokenHeader);
            builder.Append(':');
            builder.Append(metadata.Credentials.Token);
            builder.Append('\n');

            signedHeadersBuilder.Append(';');
            signedHeadersBuilder.Append(HeaderKeys.XAmzSecurityTokenHeader);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AppendCanonicalResourcePath(ref NoAllocStringBuilder builder, string serviceName, Uri requestUri)
        {
            var path = serviceName == ServiceNames.S3
                ? requestUri.LocalPath
                : requestUri.AbsolutePath.Replace("//", "/");
            
            var pathSegments = path.Split('/').Select(pathSegment => UrlEncoder.Encode(pathSegment, false));
            
            builder.Append(string.Join("/", pathSegments));
            builder.Append('\n');
        }

        private static void AppendSortedQueryString(ref NoAllocStringBuilder builder, string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                var queryParameters = HttpUtility.ParseQueryString(query);

                var parsedParameters = new Dictionary<string, List<string>>(queryParameters.Count);
                foreach (string parameterName in queryParameters)
                {
                    var parameterValues = queryParameters.GetValues(parameterName)!;
                    if (!parsedParameters.TryGetValue(parameterName, out var cachedParameterValues))
                        parsedParameters[parameterName] = parameterValues.ToList();

                    else
                        cachedParameterValues.AddRange(parameterValues);
                }

                var isFirst = true;
                foreach (var parameter in parsedParameters.OrderBy(x => x.Key, StringComparer.Ordinal))
                {
                    foreach (var parameterValue in parameter.Value.OrderBy(x => x, StringComparer.Ordinal))
                    {
                        if (!isFirst)
                            builder.Append('&');

                        builder.Append(UrlEncoder.Encode(parameter.Key, false));
                        builder.Append('=');
                        builder.Append(UrlEncoder.Encode(parameterValue, false));
                        isFirst = false;
                    }
                }
            }

            builder.Append('\n');
        }

        private static void AppendSortedHeadersFromRequest(ref NoAllocStringBuilder builder, ref NoAllocStringBuilder signedHeadersBuilder, HttpRequestHeaders headers, HttpRequestHeaders defaultHeaders)
        {
            var sortedHeaders = new SortedDictionary<string, List<string>>(StringComparer.Ordinal);

            void AddHeader(KeyValuePair<string, IEnumerable<string>> header)
            {
                var headerName = header.Key.ToLowerInvariant();

                // Create header if it doesn't already exist
                if (!sortedHeaders.TryGetValue(headerName, out var headerValues))
                {
                    headerValues = new List<string>();
                    sortedHeaders.Add(headerName, headerValues);
                }
                
                headerValues.AddRange(header.Value);
            }

            void AddDefaultDotnetHeaders()
            {
                foreach (var defaultHeader in defaultHeaders)
                {
                    // On .NET Framework or .NET Core we only add header values if they're not
                    // already added on the message. Note that we don't merge collections: If both
                    // the default headers and the message have set some values for a certain
                    // header, then we don't try to merge the values.
                    if (!sortedHeaders.ContainsKey(defaultHeader.Key.ToLowerInvariant()))
                    {
                        AddHeader(defaultHeader);
                    }
                }
            }

            void AddDefaultMonoHeaders()
            {
                foreach (var defaultHeader in defaultHeaders)
                {
                    // On Mono we add header values indifferent of whether the header already exists
                    AddHeader(defaultHeader);
                }
            }

            // Add headers
            foreach (var header in headers)
            {
                AddHeader(header);
            }

            // Add default headers
            if (EnvironmentProbe.IsMono)
            {
                AddDefaultMonoHeaders();
            }
            else
            {
                AddDefaultDotnetHeaders();
            }
            
            var isFirstHeaderKey = true;
            foreach (var header in sortedHeaders)
            {
                builder.Append(header.Key);
                builder.Append(':');

                var isFirstHeaderValue = true;
                foreach (var headerValue in header.Value)
                {
                    if(!isFirstHeaderValue)
                        builder.Append(HeaderValueSeparator);

                    builder.Append(headerValue);
                    isFirstHeaderValue = false;
                }
                builder.Append('\n');
                
                if (!isFirstHeaderKey)
                    signedHeadersBuilder.Append(';');
                
                signedHeadersBuilder.Append(header.Key);
                
                isFirstHeaderKey = false;
            }
        }
    }
}