using System.Collections.Generic;

namespace ThirdParty.BouncyCastle.Utilities
{
    internal abstract class Platform
    {
        internal static System.Collections.IList CreateArrayList()
        {
            return new List<object>();
        }
        internal static System.Collections.IList CreateArrayList(int capacity)
        {
            return new List<object>(capacity);
        }
        internal static System.Collections.IList CreateArrayList(System.Collections.ICollection collection)
        {
            System.Collections.IList result = new List<object>(collection.Count);
            foreach (object o in collection)
            {
                result.Add(o);
            }
            return result;
        }
        internal static System.Collections.IList CreateArrayList(System.Collections.IEnumerable collection)
        {
            System.Collections.IList result = new List<object>();
            foreach (object o in collection)
            {
                result.Add(o);
            }
            return result;
        }
    }
}