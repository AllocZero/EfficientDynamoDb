using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Amazon.Util
{
    internal static class Extensions
    {
        internal static string ToUpper(this String str, CultureInfo culture)
        {
            if (culture != CultureInfo.InvariantCulture)
                throw new ArgumentException("The extension method ToUpper only works for invariant culture");
            return str.ToUpperInvariant();
        }
    }
}
