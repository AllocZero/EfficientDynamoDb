using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EfficientDynamoDb.Internal.Signing.Utils
{
    internal static class UrlEncoder
    {
        /// <summary>
        /// The set of accepted and valid Url path characters per RFC3986.
        /// </summary>
        private static readonly string ValidPathCharacters = DetermineValidPathCharacters();
        
        private static readonly Dictionary<int, string> RfcEncodingSchemes = new Dictionary<int, string>()
        {
            {3986, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~"},
            {1738, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_."}
        };
        
        /// <summary>
        /// URL encodes a string per RFC3986. If the path property is specified,
        /// the accepted path characters {/+:} are not encoded.
        /// </summary>
        /// <param name="data">The string to encode</param>
        /// <param name="path">Whether the string is a URL path or not</param>
        /// <returns>The encoded string</returns>
        public static string Encode(string data, bool path) => Encode(3986, data, path);

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
        public static string Encode(int rfcNumber, string data, bool path)
        {
            var stringBuilder = new StringBuilder(data.Length * 2);
            
            if (!RfcEncodingSchemes.TryGetValue(rfcNumber, out var str1))
                str1 = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
            
            string str2 = str1 + (path ? ValidPathCharacters : "");
            foreach (var b in Encoding.UTF8.GetBytes(data))
            {
                var ch = (char) b;
                if (str2.IndexOf(ch) != -1)
                    stringBuilder.Append(ch);
                else
                    // ReSharper disable once UseFormatSpecifierInInterpolation to avoid boxing
                    stringBuilder.Append($"%{((int) ch).ToString("X2", CultureInfo.InvariantCulture)}");
            }
            return stringBuilder.ToString();
        }
        
        private static string DetermineValidPathCharacters()
        {
            var stringBuilder = new StringBuilder();
            foreach (var ch in "/:'()!*[]$")
            {
                var str = Uri.EscapeUriString(ch.ToString());
                if (str.Length == 1 && str[0] == ch)
                    stringBuilder.Append(ch);
            }
            return stringBuilder.ToString();
        }
    }
}