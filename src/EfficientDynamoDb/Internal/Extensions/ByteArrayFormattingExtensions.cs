using System;
using System.Globalization;
using System.Text;

namespace EfficientDynamoDb.Internal.Extensions
{
    public static class ByteArrayFormattingExtensions
    {
        public static string ToHex(this byte[] array, bool lowercase)
        {
            var stringBuilder = new StringBuilder(array.Length * 2);
            
            foreach (var item in array)
                stringBuilder.Append(item.ToString(lowercase ? "x2" : "X2",  CultureInfo.InvariantCulture));

            return stringBuilder.ToString();
        }
    }
}